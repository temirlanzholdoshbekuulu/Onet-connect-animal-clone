using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
	[SerializeField] private Button continueButton;
	[SerializeField] private GameManager gameManager;
	[SerializeField] private TextMeshProUGUI highScoreText;

	void Start()
	{
		// Enable/disable Continue button based on session
		if (gameManager != null && continueButton != null)
		{
			bool hasSession = gameManager.HasSavedSession();
			continueButton.interactable = hasSession;
		}
		// Show high score
		if (gameManager != null && highScoreText != null)
		{
			highScoreText.text = $"High Score: {gameManager.highScore:D6}";
		}
	}

	public void PlayGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void ContinueGame()
	{
		if (gameManager != null && gameManager.HasSavedSession())
		{
			// Optionally: set a flag to load session after scene load
			PlayerPrefs.SetInt("OnetContinue", 1);
			PlayerPrefs.Save();
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}
	}
}
