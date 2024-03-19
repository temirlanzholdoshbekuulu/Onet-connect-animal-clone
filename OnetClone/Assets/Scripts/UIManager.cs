using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UIManager : MonoBehaviour
{
	[SerializeField] List<GameObject> uiElements;
	[SerializeField] Canvas canvas;
	[Inject] AudioManager soundManager;
	private Dictionary<string, GameObject> uiDictionary;

	void OnEnable()
	{
		GameManager.OnLose += ShowGameOverScreen;
	}
	void OnDisable()
	{
		GameManager.OnLose -= ShowGameOverScreen;
	}
	private void Start()
	{
		uiDictionary = new Dictionary<string, GameObject>();
		foreach (GameObject uiElement in uiElements)
		{
			uiDictionary.Add(uiElement.name, uiElement);
		}
	}
	
	public void ShowGameUI() 
	{
		ShowScreen("Game"); 
		canvas.renderMode = RenderMode.ScreenSpaceCamera;
	}
	public void ShowPauseScreen() 
	{
		ShowScreen("Pause");
		canvas.renderMode = RenderMode.ScreenSpaceOverlay;
	}
	public void ShowGameOverScreen() 
	{
		canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		ShowScreen("GameOver");
	}
	public void ShowMainMenu() { ShowScreen("MainMenu"); }
	public void ShowWinScreen() { ShowScreen("Win"); }

	public void ShowScreen(string screenName)
	{
		soundManager.PlayButtonSound();
		foreach (KeyValuePair<string, GameObject> uiElement in uiDictionary)
		{
			uiElement.Value.SetActive(uiElement.Key == screenName);
		}
	}
}
