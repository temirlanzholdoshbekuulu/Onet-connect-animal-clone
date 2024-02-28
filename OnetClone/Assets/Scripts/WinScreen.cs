using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class WinScreen : MonoBehaviour
{
	[Inject] GameManager gameManager;
	[SerializeField] TileSpawner levelManager;
	[SerializeField] Timer timer;
	
	public void NextLevel()
	{
		Debug.Log("Next level");
		gameManager.gameState = GameManager.GameState.Shuffling;
		gameObject.SetActive(false);
	}
}
