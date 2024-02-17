using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReshuffleTiles : MonoBehaviour
{
	public GameObject grid;
	public LevelSpawner board;
	public List<Tile> tiles = new List<Tile>();
	public List<Vector2> tilePositions = new List<Vector2>();
	public TextMeshProUGUI remainedShuffelsText;
	
	void Awake()
	{
		grid = GameObject.Find("Grid");
		board = GameObject.Find("Grid").GetComponent<LevelSpawner>();
		remainedShuffelsText = GameObject.Find("ShuffleNumText").GetComponent<TextMeshProUGUI>();
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			Reshuffle();
		}
		if (GameManager.Instance != null)
		{
			remainedShuffelsText.text = GameManager.Instance.remainedShuffles.ToString();
		}

	}
	public void Reshuffle()
	{
		
		if (GameManager.Instance.remainedShuffles !=0 && GameManager.Instance.gameState == GameManager.GameState.Playing)
		{
			GameManager.Instance.remainedShuffles--;
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
				int randomIndex = Random.Range(0, tilePositions.Count);
				Vector3 newPosition = tilePositions[randomIndex];
				tile.transform.position = newPosition;
				tiles.Remove(tile);
				tilePositions.RemoveAt(randomIndex);
	
				int x = (int)newPosition.x;
				int y = (int)newPosition.y;
				board.tiles[x, y] = tile;
			}
		}
	}
}
