using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
	[SerializeField] GameObject WinScreen;

	void OnEnable() 
	{
		GameManager.OnWin += ShowWinScreen;		
	}
	void OnDisable() 
	{
		GameManager.OnWin -=ShowWinScreen;	
	}

	void Update()
	{
		
	}
	public void ShowWinScreen()
	{
		WinScreen.SetActive(true);
	}
}
