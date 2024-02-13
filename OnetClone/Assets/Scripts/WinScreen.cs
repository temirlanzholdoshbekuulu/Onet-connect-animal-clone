using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
	[SerializeField] LevelSpawner levelManager;
	[SerializeField] Timer timer;
	[SerializeField] GameManager gameManager;
	
	void Update()
	{
		
	}
	public void NextLevel()
	{
		// gameManager.remainedTiles =128;
		GameManager.Instance.currentLevel++;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		// gameManager.ResetLevel();
		// levelManager.CallSpawnGrid();
		// timer.ResetTimer();
		// gameObject.SetActive(false);
		// Debug.Log("Spawning next level");
	}
}
