using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ReshuffleTiles : MonoBehaviour
{
	[Inject]GameManager gameManager;
	[Inject] TileSpawner tileSpawner;
	[Inject] TilesMatchChecker availableMathingTilesChecker;
	[SerializeField] GameObject grid;
	[SerializeField] TextMeshProUGUI remainedShuffelsText;
	public List<Tile> tiles = new List<Tile>();
	public List<Vector2> tilePositions = new List<Vector2>();
	
	void Awake()
	{
		grid = GameObject.Find("Grid");
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			Reshuffle();
		}
		remainedShuffelsText.text = gameManager.remainedShuffles.ToString();
	}
	public void Reshuffle()
	{
		if (gameManager.remainedShuffles !=0 && gameManager.gameState == GameManager.GameState.Playing)
		{
			gameManager.remainedShuffles--;
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
				tileSpawner.tiles[x, y] = tile;
			}
		}
	}
}
