using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CollapseTilesHalfRowsSides : ICollapseStrategy
{
    private MonoBehaviour monoBehaviour;
    private Tile[,] tiles;
    private int gridWidth;
    private int gridHeight;
    private List<Coroutine> activeMovements = new List<Coroutine>();

    public CollapseTilesHalfRowsSides(MonoBehaviour monoBehaviour, Tile[,] tiles, int gridWidth, int gridHeight)
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

            for (int y = 1; y < gridHeight - 1; y++)
            {
                // Top half → move RIGHT (using CollapseTilesRight pattern)
                if (y < gridHeight / 2)
                {
                    for (int x = 0; x < gridWidth - 1; x++)  // Start from leftmost column and go right
                    {
                        if (tiles[x, y].isEmpty)
                        {
                            // Look for a non-empty tile to the left to move right
                            for (int i = x; i >= 1; i--)
                            {
                                if (!tiles[i - 1, y].isEmpty)
                                {
                                    // Swap tiles[i, y] and tiles[i - 1, y]
                                    Tile temp = tiles[i, y];
                                    tiles[i, y] = tiles[i - 1, y];
                                    tiles[i - 1, y] = temp;

                                    // Start movement coroutines
                                    activeMovements.Add(monoBehaviour.StartCoroutine(
                                        MoveTileToPosition(tiles[i, y], new Vector3(i, y, 0))
                                    ));
                                    activeMovements.Add(monoBehaviour.StartCoroutine(
                                        MoveTileToPosition(tiles[i - 1, y], new Vector3(i - 1, y, 0))
                                    ));

                                    hasMoved = true;
                                }
                                else break;
                            }
                        }
                    }
                }
                // Bottom half → move LEFT (using CollapseTilesLeft pattern)
                else
                {
                    for (int x = gridWidth - 1; x >= 1; x--) // Start from rightmost column and go left
                    {
                        if (tiles[x, y].isEmpty)
                        {
                            // Look for a non-empty tile to the right
                            for (int i = x; i < gridWidth - 1; i++)
                            {
                                if (!tiles[i + 1, y].isEmpty)
                                {
                                    // Swap tiles[i, y] and tiles[i + 1, y]
                                    Tile temp = tiles[i, y];
                                    tiles[i, y] = tiles[i + 1, y];
                                    tiles[i + 1, y] = temp;

                                    // Start movement coroutines
                                    activeMovements.Add(monoBehaviour.StartCoroutine(
                                        MoveTileToPosition(tiles[i, y], new Vector3(i, y, 0))
                                    ));
                                    activeMovements.Add(monoBehaviour.StartCoroutine(
                                        MoveTileToPosition(tiles[i + 1, y], new Vector3(i + 1, y, 0))
                                    ));

                                    hasMoved = true;
                                }
                                else break;
                            }
                        }
                    }
                }
            }

            // Wait for all current movements to complete
            foreach (var movement in activeMovements)
            {
                yield return movement;
            }

        } while (hasMoved);

        yield return new WaitForEndOfFrame();
    }

    private IEnumerator MoveTileToPosition(Tile tile, Vector3 targetPosition)
    {
        float duration = 0.2f;
        Vector3 startPosition = tile.transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            tile.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        tile.transform.position = targetPosition;
    }
}
