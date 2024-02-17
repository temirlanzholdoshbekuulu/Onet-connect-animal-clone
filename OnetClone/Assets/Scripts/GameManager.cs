using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance {get;private set;}
	public enum GameState {Shuffling,Playing,Win,Lose};
	public GameState gameState{get;set;}
	public static event Action OnWin;
	
	public int remainedTiles;
	public int remainedShuffles =6;
	public int currentLevel = 0;
	public int currentScore =0;
	private const int MAX_TILES = 128;
	[SerializeField] LevelSpawner levelSpawner;
	[SerializeField] TextMeshProUGUI winScreenScoreText;
	[SerializeField] Timer timer;
	public bool levelIsLoaded = false;
	
	void OnEnable() 
	{
		Timer.OnTimerFinished+=GameOver;
		SelectObjects.Instance.OnTilesMatch+=AddScore;
	}
	void OnDisable() 
	{
		Timer.OnTimerFinished-=GameOver;	
	}
	void Awake()
	{
		if(Instance==null)
		{
			Instance=this;			
		}
		else
		{
			Destroy(gameObject);
		}
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
		if(remainedShuffles <= 0 || timer.remainedTime <= 0)
		{
			Debug.Log("Game Over!");
		}
		switch (gameState)
		{
			case GameState.Shuffling:
				StartNewLevel(currentLevel);
				break;
			case GameState.Playing:
				levelIsLoaded=false;
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
	public void StartNewLevel(int level)
	{
		if (levelIsLoaded == false)
		{
			levelSpawner.CallSpawnGrid();
			Debug.Log("Spawning new level : " + currentLevel.ToString());
			remainedTiles = MAX_TILES;
			currentLevel++;
			levelIsLoaded = true;
			gameState = GameState.Playing;
			timer.ResetTimer();
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
