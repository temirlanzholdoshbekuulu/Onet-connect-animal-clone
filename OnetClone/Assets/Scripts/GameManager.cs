using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance {get;private set;}
	public enum GameState {Playing,Win,Lose};
	public GameState gameState{get;set;}
	public int remainedTiles;
	public int remainedShuffles =6;
	public int currentLevel = 0;
	public int currentScore =0;
	private const int MAX_TILES = 128;
	public static event Action OnWin;
	[SerializeField] LevelSpawner levelSpawner;
	[SerializeField] TextMeshProUGUI winScreenScoreText;
	
	void Awake()
	{
		gameState = GameState.Playing;
		SelectObjects.Instance.OnTilesMatch+=AddScore;
		StartNewLevel();
		if(Instance==null)
		{
			Instance=this;			
		}
		else
		{
			Destroy(gameObject);
		}
	}
	public void StartNewLevel()
	{
		Debug.Log("Spawning new level");
		remainedTiles = MAX_TILES;
		currentLevel++;
	}
	void Update()
	{
		if(remainedTiles == 0)
		{
			gameState= GameState.Win;
		}
		if(remainedShuffles == 0)
		{
			Debug.Log("Game Over!");
		}
		if(remainedShuffles < 0)
		{
			remainedShuffles=0;
		}
		switch (gameState)
		{
			case GameState.Playing:
				break;
			case GameState.Win:
				OnWin();
				Debug.Log("Win");
				break;
			case GameState.Lose:
				Debug.Log("Lose");
				break;
			default:
				Debug.Log("Unknown GameState");
				break;
		}
		
	}
	public void AddScore()
	{
		currentScore+=10;
		winScreenScoreText.text = currentScore.ToString("D6");
	}
}
