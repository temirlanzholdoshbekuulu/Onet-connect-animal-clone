using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Zenject;

public class GameManager : MonoBehaviour
{
	public enum GameState {MainMenu,Shuffling,Playing,Win,Lose};
	public GameState gameState{get;set;}
	public static event Action OnLevelStart;
	public static event Action OnWin;
	public static event Action OnLose;
	
	[Inject] TileSpawner levelSpawner;
	[Inject] Timer timer;
	[Inject] Board board;
	[Inject] TilesMatchChecker tilesMatchChecker;
	[SerializeField] TextMeshProUGUI winScreenScoreText;
	public int currentLevel;
	public int remainedTiles;
	public int remainedShuffles =MAX_SHUFFLES;
	public int currentScore =0;
	private const int MAX_TILES = 128;
	private const int MAX_SHUFFLES = 6;
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
		gameState = GameState.MainMenu;
	}
	void Update()
	{
		if(remainedTiles == 0 && gameState == GameState.Playing)
		{
			Win();
		}
		if(levelIsLoaded == true && (remainedShuffles < 0 || timer.remainedTime <= 0))
		{
			GameOver();
		}
		switch (gameState)
		{
			case GameState.MainMenu:
				Debug.Log("Main Menu");
				break;
			case GameState.Shuffling:
				break;
			case GameState.Playing:
				break;
			case GameState.Win:
				break;
			case GameState.Lose:
				break;
			default:
				Debug.Log("Unknown GameState");
				break;
		}
		
	}
	void Win()
	{
		gameState = GameState.Win;
		OnWin?.Invoke();
		if(remainedShuffles < MAX_SHUFFLES)
		{
			remainedShuffles++;
		}
	}
	public void GameOver()
	{
		gameState = GameState.Lose;
		OnLose?.Invoke();
		Debug.Log("Game Over!");
	}
	void Shuffle()
	{
		print("shuffling");
		OnLevelStart?.Invoke();
		Play();
	}
	void Play()
	{
		levelIsLoaded = false;
		gameState = GameState.Playing;
	}
	public void LoadLevel()
	{
		if (levelIsLoaded == false)
		{
			remainedTiles = MAX_TILES;
			currentLevel++;
			levelIsLoaded = true;
			Shuffle();
		}
	}
	public void StartNewLevel()
	{
		if (levelIsLoaded == false)
		{
			remainedTiles = MAX_TILES;
			tilesMatchChecker.ResetHints();
			currentLevel = 0;
			remainedShuffles = 6;
			board.ClearBoard();
			LoadLevel();
		}
	}
	public void AddScore()
	{
		currentScore+=10;
		winScreenScoreText.text = currentScore.ToString("D6");
	}

}
