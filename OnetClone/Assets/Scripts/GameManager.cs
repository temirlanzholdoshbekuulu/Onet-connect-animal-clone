using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance {get;private set;}
	public int remainedTiles;
	public int remainedShuffles =6;
	public int currentLevel = 0;
	public int currentScore =0;
	public static event Action OnWin;
	[SerializeField] LevelSpawner levelSpawner;
	[SerializeField] TextMeshProUGUI scoreText;
	
	void Awake()
	{
		SelectObjects.current.OnTilesMatch+=AddScore;
		StartNewLevel();
		if(Instance==null)
		{
			Instance=this;
			DontDestroyOnLoad(transform.gameObject);			
		}
		else
		{
			Destroy(gameObject);
		}
	}
	public void StartNewLevel()
	{
		Debug.Log("Spawning new level");
		remainedTiles = 128;
		currentLevel++;
	}
	void Update()
	{
		if(remainedTiles == 0)
		{
			OnWin();
		}
		if(remainedShuffles == 0)
		{
			Debug.Log("Game Over!");
		}
		if(remainedShuffles < 0)
		{
			remainedShuffles=0;
		}
		
	}
	public void AddScore()
	{
		currentScore+=10;
		scoreText.text = currentScore.ToString("D6");
	}
}
