using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Board : MonoBehaviour
{
	[Inject] GameManager gameManager;
	public int gridHeight = 10;
	public int gridWidth = 18;
	public Tile[,] tiles;
	public int level;
	private TileMover tileMover;
	[SerializeField] TilesMatchChecker tilesMatchChecker;
	
	void OnEnable()
	{
		GameManager.OnWin+=DeleteBorderTiles;
		GameManager.OnWin+=ClearBoard;
		TileSelectionHandler.OnTilesMatch +=MoveTilesBasedOnLevel;
	}	
	void OnDisable()
	{
		GameManager.OnWin-=DeleteBorderTiles;
		GameManager.OnWin-=ClearBoard;
		TileSelectionHandler.OnTilesMatch -=MoveTilesBasedOnLevel;
	}	
	
	void Start()
	{
		tiles = new Tile[gridWidth, gridHeight];
		tileMover = new TileMover();
	}
	
	public void ClearBoard()
	{
		if (tiles != null)
		{
			foreach (Tile tile in tiles)
			{
				if (tile != null)
				{
					Destroy(tile.gameObject);
				}
			}
		}
		tiles = new Tile[gridWidth, gridHeight];
	}
	
	void MoveTilesBasedOnLevel()
	{
		level = gameManager.currentLevel;
		if (tileMover.GetMovementStrategy() != null)
		{
			tileMover.GetMovementStrategy().OnTilesMoved -= tilesMatchChecker.CheckAndReshuffle;
		}
		switch (level)
		{
			case 1:
				tileMover.SetMovementStrategy(new DoNotCollapse(this, tiles, gridWidth, gridHeight));
				break;
			case 2:
				tileMover.SetMovementStrategy(new CollapseTilesDown(this, tiles, gridWidth, gridHeight));
				break;
			case 3:
				tileMover.SetMovementStrategy(new CollapseTilesUp(this, tiles, gridWidth, gridHeight));
				break;
			case 4:
				tileMover.SetMovementStrategy(new CollapseTilesLeft(this, tiles, gridWidth, gridHeight));
				break;
			case 5:
				tileMover.SetMovementStrategy(new CollapseTilesRight(this, tiles, gridWidth, gridHeight));
				break;
			default:
				break;
		}
		if (tileMover.GetMovementStrategy() != null)
		{
			tileMover.GetMovementStrategy().OnTilesMoved += tilesMatchChecker.CheckAndReshuffle;
			tileMover.MoveTilesBasedOnLevel();
		}
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
	public void ResetTiles()
	{
		foreach (Tile tile in tiles)
		{
			Destroy(tile.gameObject);
		}
		tiles = new Tile[gridWidth, gridHeight];
	}
}
