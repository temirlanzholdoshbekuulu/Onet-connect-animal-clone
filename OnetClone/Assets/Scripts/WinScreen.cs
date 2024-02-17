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
	
	public void NextLevel()
	{
		Debug.Log("Next level");
		GameManager.Instance.gameState = GameManager.GameState.Shuffling;
		gameObject.SetActive(false);
	}
}
