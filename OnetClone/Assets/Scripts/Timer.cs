using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
	Image image;
	private int minutesInRound = 10;
	void Awake()
	{
		minutesInRound *= 60;
		image = GetComponent<Image>();
	}

	void Update()
	{
		image.fillAmount -=1f / minutesInRound * Time.deltaTime;
		
	}
	public void ResetTimer()
	{
		image.fillAmount = 1f;
	}
}
