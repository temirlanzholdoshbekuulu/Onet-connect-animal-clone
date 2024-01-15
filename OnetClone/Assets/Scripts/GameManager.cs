using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public int remainedTiles = 128;
	public static Action OnWin;
	
	void Start()
	{
		
	}

	void Update()
	{
		if(remainedTiles == 0)
		{
			OnWin();
			Debug.Log("Win!");
		}
		if(Input.GetKey(KeyCode.Space))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}
}
