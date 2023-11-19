using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPairs : MonoBehaviour
{
    public Board board;
    public List<Tile> firstEvaluationEmptyTiles = new List<Tile>();
    public List<Tile> secondEvaluationEmptyTiles = new List<Tile>();
    public List<Tile> thirdEvaluationEmptyTiles = new List<Tile>();
    public List<Vector3> linePoints;
    enum Evaluation { First, Second, Third }
    [SerializeField] MatchingLineRenderer Line;


    public bool CheckMatchingPairs(Tile firstTile, Tile secondTile)
    {
        firstEvaluationEmptyTiles.Clear();
        secondEvaluationEmptyTiles.Clear();
        thirdEvaluationEmptyTiles.Clear();
        linePoints.Clear();

        if (CheckDirections1(firstTile, secondTile, firstEvaluationEmptyTiles, secondTile.transform.position) == true)
        {
            return true;
        }
        else if (CheckDirections2(secondTile, firstTile, firstEvaluationEmptyTiles, secondEvaluationEmptyTiles, firstTile.transform.position) == true)
        {
            return true;
        }
        else if (CheckDirections3(firstEvaluationEmptyTiles, secondEvaluationEmptyTiles,firstTile.transform.position,secondTile.transform.position) == true)
        {
            return true;
        }
        else return false;
    }
    
    bool CheckDirections1(Tile firstTile, Tile secondTile, List<Tile> EmptyTiles, Vector3 secondTilePos)
    {
        int x = (int)firstTile.transform.position.x;
        int y = (int)firstTile.transform.position.y;

        for (int i = x + 1; i < board.gridWidth; i++)
        {
            if (board.tiles[i, y].transform.position.x == secondTile.transform.position.x
            && board.tiles[i, y].transform.position.y == secondTile.transform.position.y)
            {
                DrawLine(firstTile.transform.position,secondTile.transform.position);
                return true;
            }
            else if (board.tiles[i, y].isEmpty != true)
            {
                break;
            }
            else
            {
                EmptyTiles.Add(board.tiles[i, y]);
            }
        }
        for (int i = x - 1; i > -1; i--)
        {
            if (board.tiles[i, y].transform.position.x == secondTile.transform.position.x
            && board.tiles[i, y].transform.position.y == secondTile.transform.position.y)
            {
                DrawLine(firstTile.transform.position,secondTile.transform.position);
                return true;
            }
            else if (board.tiles[i, y].isEmpty != true)
            {
                break;
            }
            else
            {
                EmptyTiles.Add(board.tiles[i, y]);
            }
        }
        for (int i = y + 1; i < board.gridHeight; i++)
        {
            if (board.tiles[x, i].transform.position.x == secondTile.transform.position.x
            && board.tiles[x, i].transform.position.y == secondTile.transform.position.y)
            {
                DrawLine(firstTile.transform.position,secondTile.transform.position);
                return true;
            }
            else if (board.tiles[x, i].isEmpty != true)
            {
                break;
            }
            else
            {
                EmptyTiles.Add(board.tiles[x, i]);
            }
        }
        for (int i = y - 1; i > -1; i--)
        {
            if (board.tiles[x, i].transform.position.x == secondTile.transform.position.x
            && board.tiles[x, i].transform.position.y == secondTile.transform.position.y)
            {
                DrawLine(firstTile.transform.position,secondTile.transform.position);
                return true;
            }
            else if (board.tiles[x, i].isEmpty != true)
            {
                break;
            }
            else
            {
                EmptyTiles.Add(board.tiles[x, i]);
            }
        }
        return false;
    }
    bool CheckDirections2(Tile firstTile, Tile secondTile, List<Tile> firstList, List<Tile> EmptyTiles, Vector3 firstTilePos)
    {
        int x = (int)firstTile.transform.position.x;
        int y = (int)firstTile.transform.position.y;

        for (int i = x + 1; i < board.gridWidth; i++)
        {
            if (firstList.Contains(board.tiles[i, y]))
            {
                DrawLine(firstTile.transform.position,board.tiles[i,y].transform.position);
                DrawLine(board.tiles[i,y].transform.position,secondTile.transform.position);
                return true;
            }
            else if (board.tiles[i, y].isEmpty != true)
            {
                break;
            }
            else
            {
                EmptyTiles.Add(board.tiles[i, y]);
            }
        }
        for (int i = x - 1; i > -1; i--)
        {
            if (firstList.Contains(board.tiles[i, y]))
            {
                DrawLine(firstTile.transform.position,board.tiles[i,y].transform.position);
                DrawLine(board.tiles[i,y].transform.position,secondTile.transform.position);
                return true;
            }
            else if (board.tiles[i, y].isEmpty != true)
            {
                break;
            }
            else
            {
                EmptyTiles.Add(board.tiles[i, y]);
            }
        }
        for (int i = y + 1; i < board.gridHeight; i++)
        {
            if (firstList.Contains(board.tiles[x, i]))
            {
                DrawLine(firstTile.transform.position,board.tiles[x,i].transform.position);
                DrawLine(board.tiles[x,i].transform.position,secondTile.transform.position);
                return true;
            }
            else if (board.tiles[x, i].isEmpty != true)
            {
                break;
            }
            else
            {
                EmptyTiles.Add(board.tiles[x, i]);
            }
        }
        for (int i = y - 1; i > -1; i--)
        {
            if (firstList.Contains(board.tiles[x, i]))
            {
                DrawLine(firstTile.transform.position,board.tiles[x,i].transform.position);
                DrawLine(board.tiles[x,i].transform.position,secondTile.transform.position);
                return true;
            }
            else if (board.tiles[x, i].isEmpty != true)
            {
                break;
            }
            else
            {
                EmptyTiles.Add(board.tiles[x, i]);
            }
        }
        return false;
    }

    bool CheckDirections3(List<Tile> firstList, List<Tile> secondList,Vector3 firstTilePos, Vector3 secondTilePos)
    {
        foreach (Tile tile in firstList)
        {
            int x = (int)tile.transform.position.x;
            int y = (int)tile.transform.position.y;

            for (int i = x + 1; i < board.gridWidth; i++)
            {
                if (secondList.Contains(board.tiles[i, y]))
                {
                    DrawLine(firstTilePos,tile.transform.position);
                    DrawLine(tile.transform.position,board.tiles[i,y].transform.position);
                    DrawLine(board.tiles[i,y].transform.position,secondTilePos);
                    return true;
                }
                else if (board.tiles[i, y].isEmpty != true)
                {
                    break;
                }
                else
                {
                    thirdEvaluationEmptyTiles.Add(board.tiles[i, y]);
                }
            }
            for (int i = x - 1; i > -1; i--)
            {
                if (secondList.Contains(board.tiles[i, y]))
                {
                    DrawLine(firstTilePos,tile.transform.position);
                    DrawLine(tile.transform.position,board.tiles[i,y].transform.position);
                    DrawLine(board.tiles[i,y].transform.position,secondTilePos);
                    return true;
                }
                else if (board.tiles[i, y].isEmpty != true)
                {
                    break;
                }
                else
                {
                    thirdEvaluationEmptyTiles.Add(board.tiles[i, y]);
                }
            }
            for (int i = y + 1; i < board.gridHeight; i++)
            {
                if (secondList.Contains(board.tiles[x, i]))
                {
                    DrawLine(firstTilePos,tile.transform.position);
                    DrawLine(tile.transform.position,board.tiles[x,i].transform.position);
                    DrawLine(board.tiles[x,i].transform.position,secondTilePos);
                    return true;
                }
                else if (board.tiles[x, i].isEmpty != true)
                {
                    break;
                }
                else
                {
                    thirdEvaluationEmptyTiles.Add(board.tiles[x, i]);
                }
            }
            for (int i = y - 1; i > -1; i--)
            {
                if (secondList.Contains(board.tiles[x, i]))
                {
                    DrawLine(firstTilePos,tile.transform.position);
                    DrawLine(tile.transform.position,board.tiles[x,i].transform.position);
                    DrawLine(board.tiles[x,i].transform.position,secondTilePos);
                    return true;
                }
                else if (board.tiles[x, i].isEmpty != true)
                {
                    break;
                }
                else
                {
                    thirdEvaluationEmptyTiles.Add(board.tiles[x, i]);
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