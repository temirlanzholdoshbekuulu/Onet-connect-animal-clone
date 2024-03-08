using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TileSelectionHandler : MonoBehaviour
{
	[Inject] GameManager gameManager;
	[Inject] TilePathFinder matchingPathFinder;
	[Inject] Board board;
	[SerializeField] Transform emptyTile;
	[SerializeField] PopingTile tilePop;
	private GameObject selectedTile1;
	private GameObject selectedTile2;
	public static event Action OnTilesMatch;
	
	void Update()
	{
		if (Input.GetMouseButtonDown(0) && gameManager.gameState == GameManager.GameState.Playing)
		{
			HandleSelection();
		}
	}
	
	void HandleSelection()
	{
		Vector2 raycastPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		RaycastHit2D hit = Physics2D.Raycast(raycastPosition, Vector2.zero);
		
		if (selectedTile1 == null)
		{
			HandleFirstSelection(hit);
		}
		else
		{
			HandleSecondSelection(hit);
		}
	}
	
	void HandleFirstSelection(RaycastHit2D hit)
	{
		if (hit.collider != null)
		{
			selectedTile1 = hit.collider.gameObject;
			selectedTile1.GetComponent<SpriteRenderer>().material.color -= new Color(0, 0, 0, 0.3f);
		}
	}
	
	void HandleSecondSelection(RaycastHit2D hit)
	{
		if (hit.collider != null && hit.collider.gameObject != selectedTile1.gameObject)
		{
			selectedTile2 = hit.collider.gameObject;
			selectedTile2.GetComponent<SpriteRenderer>().material.color -= new Color(0, 0, 0, 0.3f);
			if(selectedTile1.GetComponent<Tile>().name == selectedTile2.GetComponent<Tile>().name)
			{
				matchingPathFinder.renderLine = true;
				if (matchingPathFinder.IsPathValid(selectedTile1.GetComponent<Tile>(), selectedTile2.GetComponent<Tile>()) == true)
				{
					GameObject firstTilePop = Instantiate(tilePop.gameObject,selectedTile1.transform.position,Quaternion.identity);
					firstTilePop.GetComponent<SpriteRenderer>().sprite = selectedTile1.GetComponent<SpriteRenderer>().sprite;
					GameObject secondTilePop = Instantiate(tilePop.gameObject,selectedTile2.transform.position,Quaternion.identity);
					secondTilePop.GetComponent<SpriteRenderer>().sprite = selectedTile2.GetComponent<SpriteRenderer>().sprite;
					selectedTile1.GetComponent<Tile>().MakeEmpty();
					selectedTile2.GetComponent<Tile>().MakeEmpty();
					selectedTile1 = null;
					selectedTile2 = null;
					matchingPathFinder.board = board;
					TilesMatch();
				}
				else if(matchingPathFinder.IsPathValid(selectedTile1.GetComponent<Tile>(), selectedTile2.GetComponent<Tile>()) == false)
				{
					selectedTile1.GetComponent<SpriteRenderer>().material.color += new Color(0, 0, 0,0.3f);
					selectedTile1 = selectedTile2;
					selectedTile2 = null;

				}
			}
			else
			{
				selectedTile1.GetComponent<SpriteRenderer>().material.color += new Color(0, 0, 0,0.3f);
				selectedTile1 = selectedTile2;
				selectedTile2 = null;
			}
		}
	}
	public void TilesMatch()
	{
		OnTilesMatch?.Invoke();
	}
}