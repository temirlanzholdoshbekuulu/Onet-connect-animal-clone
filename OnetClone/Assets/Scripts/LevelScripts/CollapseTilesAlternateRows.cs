using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CollapseTilesAlternateRows : ICollapseStrategy
{
    private MonoBehaviour monoBehaviour;
    private Tile[,] tiles;
    private int gridWidth;
    private int gridHeight;
    private List<Coroutine> activeMovements = new List<Coroutine>();

    public CollapseTilesAlternateRows(MonoBehaviour monoBehaviour, Tile[,] tiles, int gridWidth, int gridHeight)
    {
        this.monoBehaviour = monoBehaviour;
        this.tiles = tiles;
        this.gridWidth = gridWidth;
        this.gridHeight = gridHeight;
    }

    public IEnumerator MoveTiles()
    {
        bool hasMoved;
        do
        {
            hasMoved = false;
            activeMovements.Clear();

            // iterate rows in playable range (match your original bounds)
            for (int y = 1; y < gridHeight - 1; y++)
            {
                // EVEN rows -> move RIGHT (collapse toward right)
                if (y % 2 == 0)
                {
                    // scan from left to right looking for empty cells to pull non-empty tiles from the left
                    for (int x = 0; x <= gridWidth - 2; x++)
                    {
                        if (tiles[x, y].isEmpty)
                        {
                            for (int i = x; i > 0; i--)
                            {
                                if (!tiles[i - 1, y].isEmpty)
                                {
                                    // swap tiles[i, y] and tiles[i-1, y]
                                    Tile temp = tiles[i, y];
                                    tiles[i, y] = tiles[i - 1, y];
                                    tiles[i - 1, y] = temp;

                                    // animate moved tiles to their new grid positions
                                    activeMovements.Add(monoBehaviour.StartCoroutine(
                                        MoveTileToPosition(tiles[i, y], new Vector3(i, y, 0))
                                    ));
                                    activeMovements.Add(monoBehaviour.StartCoroutine(
                                        MoveTileToPosition(tiles[i - 1, y], new Vector3(i - 1, y, 0))
                                    ));

                                    hasMoved = true;
                                }
                                else
                                {
                                    break; // no more non-empty tiles to the left
                                }
                            }
                        }
                    }
                }
                // ODD rows -> move LEFT (collapse toward left) — same as your CollapseTilesLeft behavior
                else
                {
                    // scan from right to left looking for empty cells to pull non-empty tiles from the right
                    for (int x = gridWidth - 1; x >= 1; x--)
                    {
                        if (tiles[x, y].isEmpty)
                        {
                            for (int i = x; i < gridWidth - 1; i++)
                            {
                                if (!tiles[i + 1, y].isEmpty)
                                {
                                    // swap tiles[i, y] and tiles[i + 1, y]
                                    Tile temp = tiles[i, y];
                                    tiles[i, y] = tiles[i + 1, y];
                                    tiles[i + 1, y] = temp;

                                    // animate moved tiles to their new grid positions
                                    activeMovements.Add(monoBehaviour.StartCoroutine(
                                        MoveTileToPosition(tiles[i, y], new Vector3(i, y, 0))
                                    ));
                                    activeMovements.Add(monoBehaviour.StartCoroutine(
                                        MoveTileToPosition(tiles[i + 1, y], new Vector3(i + 1, y, 0))
                                    ));

                                    hasMoved = true;
                                }
                                else
                                {
                                    break; // hit another empty tile, stop
                                }
                            }
                        }
                    }
                }
            }

            // wait for all started movements to finish
            foreach (var movement in activeMovements)
            {
                yield return movement;
            }

        } while (hasMoved);

        // ensure transforms settle
        yield return new WaitForEndOfFrame();
    }

    private IEnumerator MoveTileToPosition(Tile tile, Vector3 targetPosition)
    {
        float duration = 0.2f; // same as your other scripts
        Vector3 startPosition = tile.transform.position;
        float elapsed = 0f;

        // skip lerp if already there
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
