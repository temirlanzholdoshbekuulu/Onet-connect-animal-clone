using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	public int gridHeight;
	public int gridWidth;
	[SerializeField] Transform[] tilePrefabs;
	[SerializeField] Transform emptyTilePrefab;
	[SerializeField] CheckSelectedTiles checkPairsClass;
	public Tile[,] tiles;
	public List<Transform> availableTiles = new List<Transform>();
	public int tilesCount;

	private const float MoveSpeed = 1500f;
	private const int DuplicateTimes = 4;

	void Start()
	{
		StartCoroutine(SpawnGrid()); 
	}

	void DuplicateTiles()
	{
		foreach (Transform tilePrefab in tilePrefabs)
		{
			for (int i = 0; i < DuplicateTimes; i++)
			{
				availableTiles.Add(tilePrefab);
			}
		}
	}

	public IEnumerator SpawnGrid()
	{
		tiles = new Tile[gridWidth, gridHeight];
		DuplicateTiles();
		for (int x = 0; x < gridWidth; x++)
		{
			for (int y = 0; y < gridHeight; y++)
			{
				tilesCount++;
				Tile tile = IsOnBorder(x, y) ? emptyTilePrefab.GetComponent<Tile>() : SelectRandomTile();
				tiles[x, y] = Instantiate(tile, new Vector3(x, y, 0), Quaternion.identity, gameObject.transform);
				if (!IsOnBorder(x, y))
				{
					yield return MoveTileToPosition(tiles[x, y], new Vector3(x, y, 0));
				}
				else
				{
					tiles[x, y].transform.position = new Vector3(x, y, 0);
				}
			}
		}
	}

	IEnumerator MoveTileToPosition(Tile tile, Vector3 targetPosition)
	{
		while (!Mathf.Approximately((tile.transform.position - targetPosition).sqrMagnitude, 0))
		{
			tile.transform.position = Vector3.MoveTowards(tile.transform.position, targetPosition, MoveSpeed * Time.deltaTime);
			yield return null;
		}
	}

	bool IsOnBorder(int x, int y)
	{
		return x == 0 || x == gridWidth - 1 || y == 0 || y == gridHeight - 1;
	}

	Tile SelectRandomTile()
	{
		int randomTileIndex = Random.Range(0, availableTiles.Count);
		Tile randomTile = availableTiles[randomTileIndex].GetComponent<Tile>();
		availableTiles.RemoveAt(randomTileIndex);
		return randomTile;
	}
}