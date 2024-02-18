using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
	private const int GRID_HEIGHT = 10;
	private const int GRID_WIDTH = 18;
	[SerializeField] LevelSpawner levelSpawner;
	public Tile[,] tiles;
	
	void Start()
	{
		SpawnGrid();
	}

	void Update()
	{
		
	}
	public void SpawnGrid()
	{
		tiles = levelSpawner.SpawnGrid();
	}

}
