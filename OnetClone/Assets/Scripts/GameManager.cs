using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Zenject;

public class GameManager : MonoBehaviour
{
	public enum GameState {Shuffling,Playing,Win,Lose};
	public GameState gameState{get;set;}
	public static event Action OnWin;
	public static event Action OnLevelStart;
	
	[Inject] TileSpawner levelSpawner;
	[Inject] Timer timer;
	[SerializeField] TextMeshProUGUI winScreenScoreText;
	public int currentLevel;
	public int remainedTiles;
	public int remainedShuffles =6;
	public int currentScore =0;
	private const int MAX_TILES = 128;
	public bool levelIsLoaded = false;
	
	void OnEnable() 
	{
		Timer.OnTimerFinished+=GameOver;
		OnLevelStart+=LoadLevel;
		TileSelectionHandler.OnTilesMatch+=AddScore;
	}
	void OnDisable() 
	{
		Timer.OnTimerFinished-=GameOver;	
	}
	void Start()
	{
		gameState = GameState.Shuffling;
	}
	void Update()
	{
		if(remainedTiles == 0 && gameState == GameState.Playing)
		{
			gameState= GameState.Win;
		}
		if(remainedShuffles < 0 || timer.remainedTime <= 0)
		{
			Debug.Log("Game Over!");
		}
		switch (gameState)
		{
			case GameState.Shuffling:
				print("shuffling");
				OnLevelStart?.Invoke();
				break;
			case GameState.Playing:
				levelIsLoaded=false;
				break;
			case GameState.Win:
				OnWin?.Invoke();
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
	public void LoadLevel()
	{
		if (levelIsLoaded == false)
		{
			remainedTiles = MAX_TILES;
			currentLevel++;
			levelIsLoaded = true;
			gameState = GameState.Playing;
		}
	}
	public void AddScore()
	{
		currentScore+=10;
		winScreenScoreText.text = currentScore.ToString("D6");
	}
	void GameOver()
	{
		gameState=GameState.Lose;
	}
}
