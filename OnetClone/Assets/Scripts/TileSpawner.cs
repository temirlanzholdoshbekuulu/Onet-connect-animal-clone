using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TileSpawner : MonoBehaviour
{
	public int gridHeight;
	public int gridWidth;
	[Inject] Board board;
	[Inject] GameManager gameManager;
	[SerializeField] Transform[] tilePrefabs;
	[SerializeField] Transform emptyTilePrefab;
	public Tile[,] tiles;
	public List<Transform> availableTiles = new List<Transform>();
	public int tilesCount;
	
	void OnEnable()=>GameManager.OnLevelStart+=InitiateGridSpawning;
	void OnDisable()=>GameManager.OnLevelStart-=InitiateGridSpawning;
	
	public void InitiateGridSpawning()
	{
		StartCoroutine("SpawnGrid");
	}
	public IEnumerator SpawnGrid()
	{
		gameManager.remainedTiles = 128;
		tiles = new Tile[gridWidth, gridHeight];
		DuplicateTiles();
		for (int x = 0; x < gridWidth; x++)
		{
			for (int y = 0; y < gridHeight; y++)
			{
				tilesCount ++;
				Tile tile = IsBorderTile(x, y) ? emptyTilePrefab.GetComponent<Tile>() : SelectRandomTile();
				tiles[x, y] = Instantiate(tile, new Vector3(11, 18, 0), Quaternion.identity,board.transform);
				if(!IsBorderTile(x,y))
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
		board.SetTiles(tiles);
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
	bool IsBorderTile(int x, int y)
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