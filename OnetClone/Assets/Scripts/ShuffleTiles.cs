using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuffleTiles : MonoBehaviour
{
	public GameObject grid;
	public LevelSpawner board;
	public List<Tile> tiles = new List<Tile>();
	public List<Vector2> tilePositions = new List<Vector2>();
	
	void Awake()
	{
		grid = GameObject.Find("Grid");
		board = GameObject.Find("Grid").GetComponent<LevelSpawner>();
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			Shuffle();
		}
	}
	void Shuffle()
	{
		foreach(Tile tile in grid.transform.GetComponentsInChildren<Tile>())
		{
			if(tile.isEmpty == false)
			{
				tiles.Add(tile);
				tilePositions.Add(tile.transform.position);
			}
		}
		List<Tile> tilesCopy = new List<Tile>(tiles);
		foreach(Tile tile in tilesCopy)
		{
			int randomIndex = UnityEngine.Random.Range(0, tilePositions.Count);
			Vector3 newPosition = tilePositions[randomIndex];
			tile.transform.position = newPosition;
			tiles.Remove(tile);
			tilePositions.RemoveAt(randomIndex);

			// Update the board.tiles grid
			int x = (int)newPosition.x;
			int y = (int)newPosition.y;
			board.tiles[x, y] = tile;
		}
	}
}
