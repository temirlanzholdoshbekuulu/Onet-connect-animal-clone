using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CollapseTilesToCenter : ICollapseStrategy
{
    private MonoBehaviour monoBehaviour;
    private Tile[,] tiles;
    private int gridWidth;
    private int gridHeight;
    private List<Coroutine> activeMovements = new List<Coroutine>();

    public CollapseTilesToCenter(MonoBehaviour monoBehaviour, Tile[,] tiles, int gridWidth, int gridHeight)
    {
        this.monoBehaviour = monoBehaviour;
        this.tiles = tiles;
        this.gridWidth = gridWidth;
        this.gridHeight = gridHeight;
    }

    public IEnumerator MoveTiles()
    {
        bool hasMoved;

        // compute center indices
        int centerLeft = (gridWidth - 1) / 2;   // for both odd/even this is left center index
        int centerRight = gridWidth / 2;        // for even it's the right center; for odd equal to centerLeft
        // right placement start: if width odd, start at center+1 to avoid both placing into same center cell
        int rightStart = (gridWidth % 2 == 1) ? centerRight + 1 : centerRight;

        do
        {
            hasMoved = false;
            activeMovements.Clear();

            // Process each row independently
            for (int y = 0; y < gridHeight; y++)
            {
                // Capture the current row tiles (so we know old positions)
                Tile[] prevRow = new Tile[gridWidth];
                for (int x = 0; x < gridWidth; x++) prevRow[x] = tiles[x, y];

                // Collect left-side non-empty tiles scanning from center outwards (center -> left)
                List<Tile> leftTiles = new List<Tile>();
                for (int x = centerLeft; x >= 0; x--)
                    if (!prevRow[x].isEmpty) leftTiles.Add(prevRow[x]);

                // Collect right-side non-empty tiles scanning from center outwards (centerRight -> right)
                List<Tile> rightTiles = new List<Tile>();
                for (int x = centerRight; x < gridWidth; x++)
                    if (!prevRow[x].isEmpty) rightTiles.Add(prevRow[x]);

                // Collect all empty tile instances in the row so we can reuse them for empty slots
                Queue<Tile> emptyTiles = new Queue<Tile>();
                for (int x = 0; x < gridWidth; x++)
                    if (prevRow[x].isEmpty) emptyTiles.Enqueue(prevRow[x]);

                // Build the new row reference array
                Tile[] newRow = new Tile[gridWidth];

                // Place left tiles starting from centerLeft moving left
                for (int i = 0; i < leftTiles.Count; i++)
                {
                    int targetX = centerLeft - i;
                    if (targetX >= 0 && targetX < gridWidth)
                        newRow[targetX] = leftTiles[i];
                }

                // Place right tiles starting from rightStart moving right
                for (int i = 0; i < rightTiles.Count; i++)
                {
                    int targetX = rightStart + i;
                    if (targetX >= 0 && targetX < gridWidth)
                        newRow[targetX] = rightTiles[i];
                }

                // Fill remaining slots with empty tile objects (preserve the actual empty Tile instances)
                for (int x = 0; x < gridWidth; x++)
                {
                    if (newRow[x] == null)
                    {
                        // if we have an available empty tile instance, use it
                        if (emptyTiles.Count > 0)
                        {
                            newRow[x] = emptyTiles.Dequeue();
                        }
                        else
                        {
                            // fallback: if none, create a placeholder by marking previous tile as empty
                            // (shouldn't usually happen because number of empties + non-empty == width)
                            newRow[x] = prevRow[x];
                            newRow[x].isEmpty = true;
                        }
                    }
                }

                // Map old tile -> oldX for movement calculation
                Dictionary<Tile, int> oldXForTile = new Dictionary<Tile, int>();
                for (int x = 0; x < gridWidth; x++)
                {
                    Tile t = prevRow[x];
                    // if same tile appears multiple times (edge-case), last occurrence wins; usually tiles are unique
                    oldXForTile[t] = x;
                }

                // Now update tiles[,] for this row and animate any tile that moved
                for (int x = 0; x < gridWidth; x++)
                {
                    Tile newTile = newRow[x];
                    Tile oldTileAtPos = tiles[x, y];

                    // If the tile at this grid cell is already the correct reference, nothing to do.
                    if (newTile == oldTileAtPos)
                        continue;

                    // Set the new reference in the grid
                    tiles[x, y] = newTile;

                    // Determine where this tile came from so we can animate it to (x,y)
                    if (oldXForTile.TryGetValue(newTile, out int oldX))
                    {
                        Vector3 targetPosition = new Vector3(x, y, 0);
                        // If oldX == x then maybe the tile was already here but references changed; still snap
                        if (oldX != x || prevRow[oldX] != newTile)
                        {
                            // start movement coroutine for this tile
                            activeMovements.Add(monoBehaviour.StartCoroutine(MoveTileToPosition(newTile, targetPosition)));
                        }
                        else
                        {
                            // tile reference existed at same x but was replaced earlier: still ensure its world position matches
                            activeMovements.Add(monoBehaviour.StartCoroutine(MoveTileToPosition(newTile, targetPosition)));
                        }
                    }
                    else
                    {
                        // newTile not found in old row mapping (shouldn't happen) — just move it to target
                        activeMovements.Add(monoBehaviour.StartCoroutine(MoveTileToPosition(newTile, new Vector3(x, y, 0))));
                    }

                    hasMoved = true;
                }
            } // end for each row

            // Wait for all started movements to finish before next pass
            foreach (var movement in activeMovements)
                yield return movement;

            // one extra frame to settle transforms
            yield return null;

        } while (hasMoved);

        yield return new WaitForEndOfFrame();
    }

    private IEnumerator MoveTileToPosition(Tile tile, Vector3 targetPosition)
    {
        float duration = 0.18f;
        Vector3 startPosition = tile.transform.position;
        float elapsed = 0f;

        // small optimization: if already at target, skip lerp
        if ((startPosition - targetPosition).sqrMagnitude < 0.0001f)
        {
            tile.transform.position = targetPosition;
            yield break;
        }

        while (elapsed < duration)
        {
            tile.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        tile.transform.position = targetPosition;
    }
}
