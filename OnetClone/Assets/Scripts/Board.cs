using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Board : MonoBehaviour
{
	public int gridHeight = 10;
	public int gridWidth = 18;
	public Tile[,] tiles;
	
	void OnEnable()=> GameManager.OnWin+=DeleteBorderTiles;
	void OnDisable()=> GameManager.OnWin-=DeleteBorderTiles;
	
	void Start()
	{
		tiles = new Tile[gridWidth, gridHeight];
	}
	
	public void SetTiles(Tile[,] newTiles)
	{
		tiles = newTiles;
	}
	
	void DeleteBorderTiles()
	{
		foreach (Tile tile in tiles)
		{
			Destroy(tile);
		}
	}

}
