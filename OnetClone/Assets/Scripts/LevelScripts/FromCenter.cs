using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FromCenter : ICollapseStrategy
{
    private MonoBehaviour monoBehaviour;
    private Tile[,] tiles;
    private int gridWidth;
    private int gridHeight;
    private List<Coroutine> activeMovements = new List<Coroutine>();

    public FromCenter(MonoBehaviour monoBehaviour, Tile[,] tiles, int gridWidth, int gridHeight)
    {
        this.monoBehaviour = monoBehaviour;
        this.tiles = tiles;
        this.gridWidth = gridWidth;
        this.gridHeight = gridHeight;
    }

    public IEnumerator MoveTiles()
    {
        bool hasMoved;
        int centerX = gridWidth / 2;

        do
        {
            hasMoved = false;
            activeMovements.Clear();

            // Move tiles to the left side
            for (int x = centerX - 1; x >= 1; x--)
            {
                for (int y = 1; y < gridHeight - 1; y++)
                {
                    if (tiles[x, y].isEmpty)
                    {
                        for (int i = x; i < centerX; i++)
                        {
                            if (!tiles[i + 1, y].isEmpty)
                            {
                                SwapTiles(i, y, i + 1, y);
                                hasMoved = true;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }

            // Move tiles to the right side
            for (int x = centerX + 1; x < gridWidth - 1; x++)
            {
                for (int y = 1; y < gridHeight - 1; y++)
                {
                    if (tiles[x, y].isEmpty)
                    {
                        for (int i = x; i > centerX; i--)
                        {
                            if (!tiles[i - 1, y].isEmpty)
                            {
                                SwapTiles(i, y, i - 1, y);
                                hasMoved = true;
                            }
                            else
                            {
                                break;
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

    private void SwapTiles(int x1, int y1, int x2, int y2)
    {
        Tile temp = tiles[x1, y1];
        tiles[x1, y1] = tiles[x2, y2];
        tiles[x2, y2] = temp;

        activeMovements.Add(monoBehaviour.StartCoroutine(MoveTileToPosition(tiles[x1, y1], new Vector3(x1, y1, 0))));
        activeMovements.Add(monoBehaviour.StartCoroutine(MoveTileToPosition(tiles[x2, y2], new Vector3(x2, y2, 0))));
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
