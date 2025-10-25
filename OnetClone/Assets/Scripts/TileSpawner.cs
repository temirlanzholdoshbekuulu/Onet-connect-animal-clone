using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TileSpawner : MonoBehaviour
{
	[Inject] Board board;
	[Inject] GameManager gameManager;

	[SerializeField] Transform[] tilePrefabs;
	[SerializeField] Transform emptyTilePrefab;

	public int gridHeight;
	public int gridWidth;

	public Tile[,] tiles;
	public List<Transform> availableTiles = new List<Transform>();
	public int tilesCount;

	void OnEnable() => GameManager.OnLevelStart += InitiateGridSpawning;
	void OnDisable() => GameManager.OnLevelStart -= InitiateGridSpawning;

	public void InitiateGridSpawning()
	{
		StartCoroutine(SpawnGrid());
	}

	public IEnumerator SpawnGrid()
	{
		gameManager.remainedTiles = (gridWidth - 2) * (gridHeight - 2);
		tiles = new Tile[gridWidth, gridHeight];
		availableTiles.Clear();
		DuplicateTiles();

		for (int x = 0; x < gridWidth; x++)
		{
			for (int y = 0; y < gridHeight; y++)
			{
				tilesCount++;

				Tile tile = IsBorderTile(x, y)
					? emptyTilePrefab.GetComponent<Tile>()
					: SelectRandomTile();

				tiles[x, y] = Instantiate(tile, new Vector3(11, 18, 0), Quaternion.identity, board.transform);

				if (!IsBorderTile(x, y))
				{
					while (!Mathf.Approximately((tiles[x, y].transform.position - new Vector3(x, y, 0)).sqrMagnitude, 0))
					{
						tiles[x, y].transform.position = Vector3.MoveTowards(
							tiles[x, y].transform.position,
							new Vector3(x, y, 0),
							1500 * Time.deltaTime
						);
						yield return null;
					}
				}
				else
				{
					tiles[x, y].transform.position = new Vector3(x, y, 0);
				}
			}
		}

		board.SetTiles(tiles);
	}

	// --- Duplicate each tile exactly 4 times ---
	void DuplicateTiles()
	{
		availableTiles.Clear();

		foreach (Transform tilePrefab in tilePrefabs)
		{
			for (int i = 0; i < 4; i++) // 4 instances per tile
			{
				availableTiles.Add(tilePrefab);
			}
		}

		// Shuffle the list for randomness
		for (int i = 0; i < availableTiles.Count; i++)
		{
			int rand = Random.Range(i, availableTiles.Count);
			(availableTiles[i], availableTiles[rand]) = (availableTiles[rand], availableTiles[i]);
		}

		// Optional warning if grid too big
		int nonBorderTiles = (gridWidth - 2) * (gridHeight - 2);
		if (availableTiles.Count < nonBorderTiles)
		{
			Debug.LogWarning($"Not enough tiles! Need {nonBorderTiles}, but only have {availableTiles.Count}. Add more tile prefabs or reduce grid size.");
		}
	}

	bool IsBorderTile(int x, int y)
	{
		return x == 0 || x == gridWidth - 1 || y == 0 || y == gridHeight - 1;
	}

	Tile SelectRandomTile()
	{
		if (availableTiles.Count == 0)
		{
			Debug.LogError("No available tiles left to spawn!");
			return tilePrefabs[0].GetComponent<Tile>(); // fallback safeguard
		}

		int randomTileIndex = Random.Range(0, availableTiles.Count);
		Tile randomTile = availableTiles[randomTileIndex].GetComponent<Tile>();
		availableTiles.RemoveAt(randomTileIndex);
		return randomTile;
	}
}
