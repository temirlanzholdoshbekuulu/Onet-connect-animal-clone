using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
	public int gridHeight;
	public int gridWidth;
	[SerializeField] Transform[] tilePrefabs;
	[SerializeField] Transform emptyTilePrefab;
	[SerializeField] CheckSelectedTiles checkPairsClass;
	public Tile[,] tiles;
	public List<Transform> availableTiles = new List<Transform>();
	public int tilesCount;

	void Start()
	{
		CallSpawnGrid();
	}
	void Update() 
	{
		
	}
	public void CallSpawnGrid()
	{
		StartCoroutine("SpawnGrid");
	}
	public IEnumerator SpawnGrid()
	{
		GameManager.Instance.remainedTiles = 128;
		tiles = new Tile[gridWidth, gridHeight];
		DuplicateTiles();
		for (int x = 0; x < gridWidth; x++)
		{
			for (int y = 0; y < gridHeight; y++)
			{
				tilesCount ++;
				Tile tile = IsOnBorder(x, y) ? emptyTilePrefab.GetComponent<Tile>() : SelectRandomTile();
				tiles[x, y] = Instantiate(tile, new Vector3(11, 18, 0), Quaternion.identity, gameObject.transform);
				if(!IsOnBorder(x,y))
				{
					while(!Mathf.Approximately((tiles[x,y].transform.position - new Vector3(x,y,0)).sqrMagnitude,0))
					{
						tiles[x,y].transform.position = Vector3.MoveTowards(tiles[x,y].transform.position,new Vector3(x,y,0),1500* Time.deltaTime);
						yield return null;
					}
				}
				else
				{
					tiles[x,y].transform.position = new Vector3(x,y,0);
				}

			}
		}
	}
	void DuplicateTiles()
	{
		foreach (Transform tilePrefab in tilePrefabs)
		{
			for (int i = 0; i < 4; i++)
			{
				availableTiles.Add(tilePrefab);
			}
		}
	}
	bool IsOnBorder(int x, int y)
	{
		return x == 0 || x == gridWidth-1 || y == 0 || y == gridHeight -1;
	}

	Tile SelectRandomTile()
	{
		int randomTileIndex = Random.Range(0, availableTiles.Count);
		Tile randomTile = availableTiles[randomTileIndex].GetComponent<Tile>();
		availableTiles.RemoveAt(randomTileIndex);
		return randomTile;
	}
}