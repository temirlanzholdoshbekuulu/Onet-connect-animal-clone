﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Timer : MonoBehaviour
{
	[Inject] GameManager gameManager;
	private const int SECONDS_IN_MINUTE = 60;
	private int minutesInRound = 10;
	public float remainedTime;
	public static event Action OnTimerFinished;
	private Image image;

	void OnEnable()=>GameManager.OnLevelStart+=ResetTimer;
	void OnDisable()=>GameManager.OnLevelStart-=ResetTimer;
	
	void Awake()
	{
		minutesInRound *= SECONDS_IN_MINUTE;
		image = GetComponent<Image>();
	}

	void Update()
	{
		if (gameManager.gameState == GameManager.GameState.Playing)
		{
			image.fillAmount -= 1f / minutesInRound * Time.deltaTime;
			remainedTime = image.fillAmount;
	
			if (image.fillAmount <= 0)
			{
				OnTimerFinished?.Invoke();
			}
		}
	}
	public void ResetTimer()
	{
		image.fillAmount = 1f;
	}

}
