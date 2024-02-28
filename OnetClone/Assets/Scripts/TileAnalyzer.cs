using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TileAnalyzer : MonoBehaviour
{
	[Inject] GameManager gameManager;
	public ReshuffleTiles reshuffleTiles;
	public TilePathFinder tilePathFinder;
	public TileSelectionHandler tileSelectionHandler;
	public Board board;
	private List<Coroutine> pulseCoroutines = new List<Coroutine>();
	private SpriteRenderer highlightedTile1,highlightedTile2;

	void OnEnable()
	{
		tileSelectionHandler.OnTilesMatch += CheckAndReshuffle;
		tileSelectionHandler.OnTilesMatch += ResetCoroutines;
	}
	void OnDisable()
	{
		tileSelectionHandler.OnTilesMatch -= CheckAndReshuffle;
		tileSelectionHandler.OnTilesMatch -= ResetCoroutines;
	}
	
	public void HighlightMatchingTiles()
	{
		if (gameManager.gameState == GameManager.GameState.Playing && gameManager.remainedTiles > 2)
		{
			FindAndHighlightPairs(true);
		}
	}

	public void CheckAndReshuffle()
	{
		if (gameManager.gameState == GameManager.GameState.Playing && gameManager.remainedTiles > 2)
		{
			if (!FindAndHighlightPairs(false))
			{
				reshuffleTiles.Reshuffle();
				Debug.Log("No matches found");
				foreach (Coroutine coroutine in pulseCoroutines)
				{
					StopCoroutine(coroutine);
				}
				pulseCoroutines.Clear();
			}
		}
	}

	private bool FindAndHighlightPairs(bool shouldHighlight)
	{
		tilePathFinder.renderLine = false;

		for (int x = 0; x < board.gridWidth; x++)
		{
			for (int y = 0; y < board.gridHeight; y++)
			{
				Tile tile1 = board.tiles[x, y];
				if (tile1.isEmpty)
				{
					continue;
				}

				for (int k = 0; k < board.gridWidth; k++)
				{
					for (int l = 0; l < board.gridHeight; l++)
					{
						Tile tile2 = board.tiles[k, l];
						if (tile2.isEmpty || tile1 == tile2 || tile1.name != tile2.name)
						{
							continue;
						}

						if (tilePathFinder.IsPathValid(tile1, tile2))
						{
							if (shouldHighlight)
							{
								highlightedTile1 = tile1.GetComponent<SpriteRenderer>();
								highlightedTile2 = tile2.GetComponent<SpriteRenderer>();
								pulseCoroutines.Add(StartCoroutine(PulseColor(highlightedTile1)));
								pulseCoroutines.Add(StartCoroutine(PulseColor(highlightedTile2)));
							}

							return true;
						}
					}
				}
			}
		}
		return false;
	}
	
	void ResetCoroutines()
	{
		foreach (Coroutine coroutine in pulseCoroutines)
		{
			StopCoroutine(coroutine);
		}
		pulseCoroutines.Clear();
		if (highlightedTile1 != null && highlightedTile2 != null)
		{
			highlightedTile1.color = Color.white;
			highlightedTile2.color = Color.white;
		}
	}
	
	IEnumerator PulseColor(SpriteRenderer spriteRenderer)
	{
		float duration = 1.5f; // duration of one pulse
		float lerpTime = 5f;

		while (true)
		{
			lerpTime += Time.deltaTime;
			float perc = Mathf.Sin(2 * Mathf.PI * lerpTime / duration) / 2 + 0.5f;
			spriteRenderer.color = Color.Lerp(Color.gray, Color.white, perc);
			yield return null;
		}
	}
}