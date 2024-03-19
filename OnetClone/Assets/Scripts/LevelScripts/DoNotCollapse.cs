using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNotCollapse : ICollapseStrategy
{
	private MonoBehaviour monoBehaviour;
	private Tile[,] tiles;
	private int gridWidth;
	private int gridHeight;
	public event Action OnTilesMoved;

	public DoNotCollapse(MonoBehaviour monoBehaviour, Tile[,] tiles, int gridWidth, int gridHeight)
	{
		this.monoBehaviour = monoBehaviour;
		this.tiles = tiles;
		this.gridWidth = gridWidth;
		this.gridHeight = gridHeight;
	}
	public void MoveTiles()
	{
		OnTilesMoved?.Invoke();
	}
}
