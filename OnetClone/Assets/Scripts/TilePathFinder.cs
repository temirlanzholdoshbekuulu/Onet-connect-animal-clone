using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TilePathFinder : MonoBehaviour
{
	[Inject] public Board board;
	[SerializeField] PathRenderer Line;
	public bool renderLine;
	public List<Tile> firstPassEmptyTiles = new List<Tile>();
	public List<Tile> secondPassEmptyTiles = new List<Tile>();
	public List<Vector3> linePoints;
	

	public bool IsPathValid(Tile selectedTile1, Tile selectedTile2)
	{
		firstPassEmptyTiles.Clear();
		secondPassEmptyTiles.Clear();
		linePoints.Clear();
		
		if (CheckStraightLine(selectedTile1, selectedTile2, firstPassEmptyTiles, selectedTile2.transform.position)||
		CheckOneBendLine(selectedTile2, selectedTile1, firstPassEmptyTiles, secondPassEmptyTiles, selectedTile1.transform.position)||
		CheckTwoBendsLine(firstPassEmptyTiles, secondPassEmptyTiles,selectedTile1.transform.position,selectedTile2.transform.position))
		{
			return true;
		}
		else return false;
	}
	bool CheckStraightLine(Tile firstTile, Tile secondTile, List<Tile> EmptyTiles, Vector3 secondTilePos)
	{
		return 	IsStraightDirectionMatch(firstTile, secondTile, EmptyTiles, 1, 0) ||
				IsStraightDirectionMatch(firstTile, secondTile, EmptyTiles, -1, 0)||
				IsStraightDirectionMatch(firstTile, secondTile, EmptyTiles, 0, 1) ||
				IsStraightDirectionMatch(firstTile, secondTile, EmptyTiles, 0, -1);
	}
	bool CheckOneBendLine(Tile firstTile, Tile secondTile, List<Tile> firstList, List<Tile> EmptyTiles, Vector3 firstTilePos)
	{
		return 	IsOneBendDirectionMatch(firstTile, secondTile, firstList, EmptyTiles, 1, 0) ||
				IsOneBendDirectionMatch(firstTile, secondTile, firstList, EmptyTiles, -1, 0)||
				IsOneBendDirectionMatch(firstTile, secondTile, firstList, EmptyTiles, 0, 1) ||
				IsOneBendDirectionMatch(firstTile, secondTile, firstList, EmptyTiles, 0, -1);
	}
	bool CheckTwoBendsLine(List<Tile> firstList, List<Tile> secondList, Vector3 firstTilePos, Vector3 secondTilePos)
	{
		return 	IsTwoBendsDirectionMatch(firstList, secondList, firstTilePos, secondTilePos, 1, 0) ||	
				IsTwoBendsDirectionMatch(firstList, secondList, firstTilePos, secondTilePos, -1, 0)||
				IsTwoBendsDirectionMatch(firstList, secondList, firstTilePos, secondTilePos, 0, 1) ||
				IsTwoBendsDirectionMatch(firstList, secondList, firstTilePos, secondTilePos, 0, -1);
	}
	bool IsStraightDirectionMatch(Tile firstTile, Tile secondTile, List<Tile> emptyTilesInPath, int dx, int dy)
	{
		int x = (int)firstTile.transform.position.x;
		int y = (int)firstTile.transform.position.y;

		for (int i = x + dx, j = y + dy; i >= 0 && i < board.gridWidth && j >= 0 && j < board.gridHeight; i += dx, j += dy)
		{
			if (board.tiles[i, j].transform.position.x == secondTile.transform.position.x && board.tiles[i, j].transform.position.y == secondTile.transform.position.y)
			{
				RenderMatchingLine(firstTile.transform.position,secondTile.transform.position);
				return true;
			}
			else if (board.tiles[i, j].isEmpty != true)
			{
				break;
			}
			else
			{
				emptyTilesInPath.Add(board.tiles[i, j]);
			}
		}
		return false;
	}

	bool IsOneBendDirectionMatch(Tile firstTile, Tile secondTile, List<Tile> firstList, List<Tile> emptyTilesInPath, int dx, int dy)
	{
		int x = (int)firstTile.transform.position.x;
		int y = (int)firstTile.transform.position.y;

		for (int i = x + dx, j = y + dy; i >= 0 && i < board.gridWidth && j >= 0 && j < board.gridHeight; i += dx, j += dy)
		{
			if (firstList.Contains(board.tiles[i, j]))
			{
				RenderMatchingLine(firstTile.transform.position, board.tiles[i, j].transform.position);
				RenderMatchingLine(board.tiles[i, j].transform.position, secondTile.transform.position);
				return true;
			}
			else if (board.tiles[i, j].isEmpty != true)
			{
				break;
			}
			else
			{
				emptyTilesInPath.Add(board.tiles[i, j]);
			}
		}
		return false;
	}

	bool IsTwoBendsDirectionMatch(List<Tile> firstList, List<Tile> secondList, Vector3 firstTilePos, Vector3 secondTilePos, int dx, int dy)
	{
		foreach (Tile tile in firstList)
		{
			int x = (int)tile.transform.position.x;
			int y = (int)tile.transform.position.y;

			for (int i = x + dx, j = y + dy; i >= 0 && i < board.gridWidth && j >= 0 && j < board.gridHeight; i += dx, j += dy)
			{
				if (secondList.Contains(board.tiles[i, j]))
				{
					RenderMatchingLine(firstTilePos, tile.transform.position);
					RenderMatchingLine(tile.transform.position, board.tiles[i, j].transform.position);
					RenderMatchingLine(board.tiles[i, j].transform.position, secondTilePos);
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

	void RenderMatchingLine(Vector3 startPoint,Vector3 endPoint)
	{
		if(renderLine)
		{
			var lineInstance = Instantiate(Line,new Vector3(0,0,-1),Quaternion.identity);
			lineInstance.startPoint = startPoint;
			lineInstance.endPoint = endPoint;
		}
	}
}