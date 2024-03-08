using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CollapseTilesLeft : ICollapseStrategy
{
	private MonoBehaviour monoBehaviour;
	private Tile[,] tiles;
	private int gridWidth;
	private int gridHeight;

	public CollapseTilesLeft(MonoBehaviour monoBehaviour, Tile[,] tiles, int gridWidth, int gridHeight)
	{
		this.monoBehaviour = monoBehaviour;
		this.tiles = tiles;
		this.gridWidth = gridWidth;
		this.gridHeight = gridHeight;
	}

	public void MoveTiles()
	{
		bool hasMoved;
		do
		{
			hasMoved = false;
			for (int x = gridWidth - 1; x >= 1; x--) // Start from the rightmost column and go left
			{
				for (int y = 1; y < gridHeight - 1; y++)
				{
					if (tiles[x, y].isEmpty)
					{
						// Swap this tile with every tile to its right
						for (int i = x; i < gridWidth - 1; i++)
						{
							if (!tiles[i + 1, y].isEmpty)
							{
								// Swap tiles[i, y] and tiles[i + 1, y]
								Tile temp = tiles[i, y];
								tiles[i, y] = tiles[i + 1, y];
								tiles[i + 1, y] = temp;

								// Smoothly move the tile to the new position
								monoBehaviour.StartCoroutine(MoveTileToPosition(tiles[i, y], new Vector3(i, y, 0)));
								monoBehaviour.StartCoroutine(MoveTileToPosition(tiles[i + 1, y], new Vector3(i + 1, y, 0)));

								hasMoved = true;
							}
							else
							{
								break; // Stop if we reach another empty tile
							}
						}
					}
				}
			}
		} while (hasMoved);
	}

	private IEnumerator MoveTileToPosition(Tile tile, Vector3 targetPosition)
	{
		float duration = 0.2f; // Duration of the movement in seconds
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