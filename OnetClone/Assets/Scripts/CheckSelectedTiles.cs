using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSelectedTiles : MonoBehaviour
{
	public LevelManager board;
	public List<Tile> firstEvaluationEmptyTiles = new List<Tile>();
	public List<Tile> secondEvaluationEmptyTiles = new List<Tile>();
	public List<Vector3> linePoints;
	[SerializeField] MatchingLineRenderer Line;

	public bool CheckMatchingPairs(Tile selectedTile1, Tile selectedTile2)
	{
		firstEvaluationEmptyTiles.Clear();
		secondEvaluationEmptyTiles.Clear();
		linePoints.Clear();
		
		if (CheckStraightLine(selectedTile1, selectedTile2, firstEvaluationEmptyTiles, selectedTile2.transform.position)||
		CheckOneBendLine(selectedTile2, selectedTile1, firstEvaluationEmptyTiles, secondEvaluationEmptyTiles, selectedTile1.transform.position)||
		CheckTwoBendsLine(firstEvaluationEmptyTiles, secondEvaluationEmptyTiles,selectedTile1.transform.position,selectedTile2.transform.position))
		{
			return true;
		}
		else return false;
	}
	bool CheckStraightLine(Tile firstTile, Tile secondTile, List<Tile> EmptyTiles, Vector3 secondTilePos)
	{
		return 	CheckDirection1(firstTile, secondTile, EmptyTiles, 1, 0) ||
				CheckDirection1(firstTile, secondTile, EmptyTiles, -1, 0)||
				CheckDirection1(firstTile, secondTile, EmptyTiles, 0, 1) ||
				CheckDirection1(firstTile, secondTile, EmptyTiles, 0, -1);
	}
	bool CheckOneBendLine(Tile firstTile, Tile secondTile, List<Tile> firstList, List<Tile> EmptyTiles, Vector3 firstTilePos)
	{
		return 	CheckDirection2(firstTile, secondTile, firstList, EmptyTiles, 1, 0) ||
				CheckDirection2(firstTile, secondTile, firstList, EmptyTiles, -1, 0)||
				CheckDirection2(firstTile, secondTile, firstList, EmptyTiles, 0, 1) ||
				CheckDirection2(firstTile, secondTile, firstList, EmptyTiles, 0, -1);
	}
	bool CheckTwoBendsLine(List<Tile> firstList, List<Tile> secondList, Vector3 firstTilePos, Vector3 secondTilePos)
	{
		return 	CheckDirection3(firstList, secondList, firstTilePos, secondTilePos, 1, 0) ||	
				CheckDirection3(firstList, secondList, firstTilePos, secondTilePos, -1, 0)||
				CheckDirection3(firstList, secondList, firstTilePos, secondTilePos, 0, 1) ||
				CheckDirection3(firstList, secondList, firstTilePos, secondTilePos, 0, -1);
	}
	bool CheckDirection1(Tile firstTile, Tile secondTile, List<Tile> EmptyTiles, int dx, int dy)
	{
		int x = (int)firstTile.transform.position.x;
		int y = (int)firstTile.transform.position.y;

		for (int i = x + dx, j = y + dy; i >= 0 && i < board.gridWidth && j >= 0 && j < board.gridHeight; i += dx, j += dy)
		{
			if (board.tiles[i, j].transform.position.x == secondTile.transform.position.x && board.tiles[i, j].transform.position.y == secondTile.transform.position.y)
			{
				DrawLine(firstTile.transform.position,secondTile.transform.position);
				return true;
			}
			else if (board.tiles[i, j].isEmpty != true)
			{
				break;
			}
			else
			{
				EmptyTiles.Add(board.tiles[i, j]);
			}
		}
		return false;
	}

	bool CheckDirection2(Tile firstTile, Tile secondTile, List<Tile> firstList, List<Tile> EmptyTiles, int dx, int dy)
	{
		int x = (int)firstTile.transform.position.x;
		int y = (int)firstTile.transform.position.y;

		for (int i = x + dx, j = y + dy; i >= 0 && i < board.gridWidth && j >= 0 && j < board.gridHeight; i += dx, j += dy)
		{
			if (firstList.Contains(board.tiles[i, j]))
			{
				DrawLine(firstTile.transform.position, board.tiles[i, j].transform.position);
				DrawLine(board.tiles[i, j].transform.position, secondTile.transform.position);
				return true;
			}
			else if (board.tiles[i, j].isEmpty != true)
			{
				break;
			}
			else
			{
				EmptyTiles.Add(board.tiles[i, j]);
			}
		}
		return false;
	}

	bool CheckDirection3(List<Tile> firstList, List<Tile> secondList, Vector3 firstTilePos, Vector3 secondTilePos, int dx, int dy)
	{
		foreach (Tile tile in firstList)
		{
			int x = (int)tile.transform.position.x;
			int y = (int)tile.transform.position.y;

			for (int i = x + dx, j = y + dy; i >= 0 && i < board.gridWidth && j >= 0 && j < board.gridHeight; i += dx, j += dy)
			{
				if (secondList.Contains(board.tiles[i, j]))
				{
					DrawLine(firstTilePos, tile.transform.position);
					DrawLine(tile.transform.position, board.tiles[i, j].transform.position);
					DrawLine(board.tiles[i, j].transform.position, secondTilePos);
					return true;
				}
				else if (board.tiles[i, j].isEmpty != true)
				{
					break;
				}
			}
		}
		return false;
	}

	void DrawLine(Vector3 startPoint,Vector3 endPoint)
	{
		var lineInstance = Instantiate(Line,Vector3.zero,Quaternion.identity);
		lineInstance.startPoint = startPoint;
		lineInstance.endPoint = endPoint;
	}
}