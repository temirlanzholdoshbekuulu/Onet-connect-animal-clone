using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance {get;private set;}
	public int remainedTiles =0;
	public int currentLevel = 0;
	public static event Action OnWin;
	[SerializeField] LevelSpawner levelSpawner;
	
	void Awake()
	{
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
		levelSpawner.CallSpawnGrid();
	}
	public void StartNewLevel()
	{
		remainedTiles = 128;
		currentLevel++;
	}
	void Update()
	{
		if(remainedTiles == 0)
		{
			OnWin();
		}
		if(Input.GetKey(KeyCode.Space))
		{
			remainedTiles = 128;
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}
}
