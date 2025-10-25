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
	public int remainedShuffles = MAX_SHUFFLES;
	public int currentScore = 0;
	public int highScore = 0;
	private const string SaveKey = "OnetSave";
	private const string HighScoreKey = "OnetHighScore";
	private const int MAX_TILES = 128;
	private const int MAX_SHUFFLES = 6;
	public bool levelIsLoaded = false;
	
	void OnEnable() 
	{
		Timer.OnTimerFinished += GameOver;
		OnLevelStart += LoadLevel;
		TileSelectionHandler.OnTilesMatch += AddScore;
		LoadHighScore();
	}
	void OnDisable() 
	{
		Timer.OnTimerFinished-=GameOver;	
	}
	void Start()
	{
		gameState = GameState.MainMenu;
		LoadSessionIfExists();
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
		if (remainedShuffles < MAX_SHUFFLES)
		{
			remainedShuffles++;
		}
		CheckAndSaveHighScore();
		DeleteSession();
	}
	public void GameOver()
	{
		gameState = GameState.Lose;
		OnLose?.Invoke();
		Debug.Log("Game Over!");
		CheckAndSaveHighScore();
		DeleteSession();
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
		SaveSession(); // Save session when game actually starts
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
			DeleteSession(); // Clear any old session first
			remainedTiles = MAX_TILES;
			tilesMatchChecker.ResetHints();
			currentLevel = 0;
			remainedShuffles = 6;
			currentScore = 0;
			winScreenScoreText.text = currentScore.ToString("D6");
			board.ClearBoard();
			LoadLevel();
		}
	}

	public void AddScore()
	{
		currentScore += 10;
		winScreenScoreText.text = currentScore.ToString("D6");
		SaveSession();
	}

	// --- Saving/Loading ---
	public void SaveSession()
	{
		PlayerPrefs.SetInt(SaveKey + "_level", currentLevel);
		PlayerPrefs.SetInt(SaveKey + "_tiles", remainedTiles);
		PlayerPrefs.SetInt(SaveKey + "_shuffles", remainedShuffles);
		PlayerPrefs.SetInt(SaveKey + "_score", currentScore);
		PlayerPrefs.SetInt(SaveKey + "_timer", (int)timer.remainedTime);
		PlayerPrefs.Save();
	}

	public bool HasSavedSession()
	{
		return PlayerPrefs.HasKey(SaveKey + "_level");
	}

	public void LoadSessionIfExists()
	{
		if (HasSavedSession())
		{
			currentLevel = PlayerPrefs.GetInt(SaveKey + "_level");
			remainedTiles = PlayerPrefs.GetInt(SaveKey + "_tiles");
			remainedShuffles = PlayerPrefs.GetInt(SaveKey + "_shuffles");
			currentScore = PlayerPrefs.GetInt(SaveKey + "_score");
			timer.remainedTime = PlayerPrefs.GetInt(SaveKey + "_timer");
			winScreenScoreText.text = currentScore.ToString("D6");
			// You may want to call LoadLevel() or set up the board here
		}
	}

	public void DeleteSession()
	{
		PlayerPrefs.DeleteKey(SaveKey + "_level");
		PlayerPrefs.DeleteKey(SaveKey + "_tiles");
		PlayerPrefs.DeleteKey(SaveKey + "_shuffles");
		PlayerPrefs.DeleteKey(SaveKey + "_score");
		PlayerPrefs.DeleteKey(SaveKey + "_timer");
	}

	// --- High Score ---
	public void CheckAndSaveHighScore()
	{
		if (currentScore > highScore)
		{
			highScore = currentScore;
			PlayerPrefs.SetInt(HighScoreKey, highScore);
			PlayerPrefs.Save();
			// Optionally: Show new record UI
		}
	}

	public void LoadHighScore()
	{
		highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
	}

}
