using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapseTilesUp : ICollapseStrategy
{
	private MonoBehaviour monoBehaviour;
	private Tile[,] tiles;
	private int gridWidth;
	private int gridHeight;
	private int movingTilesCount = 0;
	public event Action OnTilesMoved;

	public CollapseTilesUp(MonoBehaviour monoBehaviour, Tile[,] tiles, int gridWidth, int gridHeight)
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
			for (int x = 1; x < gridWidth - 1; x++)
			{
				for (int y = 1; y < gridHeight - 1; y++) // Start from the bottom and go up
				{
					if (tiles[x, y].isEmpty)
					{
						// Swap this tile with every tile below it
						for (int i = y; i >= 1; i--)
						{
							if (!tiles[x, i - 1].isEmpty)
							{
								// Swap tiles[x, i] and tiles[x, i - 1]
								Tile temp = tiles[x, i];
								tiles[x, i] = tiles[x, i - 1];
								tiles[x, i - 1] = temp;

								// Smoothly move the tile to the new position
								monoBehaviour.StartCoroutine(MoveTileToPosition(tiles[x, i], new Vector3(x, i, 0)));
								monoBehaviour.StartCoroutine(MoveTileToPosition(tiles[x, i - 1], new Vector3(x, i - 1, 0)));

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
		movingTilesCount++; // Increment the counter when a tile starts moving

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

		movingTilesCount--; // Decrement the counter when a tile finishes moving

		if (movingTilesCount == 0) // If no more tiles are moving
		{
			OnTilesMoved?.Invoke(); // Invoke the event
		}
	}
}