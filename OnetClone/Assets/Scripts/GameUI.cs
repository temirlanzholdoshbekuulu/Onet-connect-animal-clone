using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
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
		scoreText.text = GameManager.Instance.currentScore.ToString("D6");
	}
	public void ShowWinScreen()
	{
		WinScreen.SetActive(true);
	}
}
