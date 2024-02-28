using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Zenject;

public class GameUI : MonoBehaviour
{
	[Inject] GameManager gameManager;
	[SerializeField] GameObject WinScreen;
	[SerializeField] TextMeshProUGUI scoreText;
	
	void OnEnable() 
	{
		GameManager.OnWin += ShowWinScreen;		
	}
	void OnDisable() 
	{
		GameManager.OnWin -=ShowWinScreen;	
	}
	void Start() 
	{
		
	}
	void Update()
	{
		scoreText.text = gameManager.currentScore.ToString("D6");
	}
	public void ShowWinScreen()
	{
		WinScreen.SetActive(true);
	}
}
