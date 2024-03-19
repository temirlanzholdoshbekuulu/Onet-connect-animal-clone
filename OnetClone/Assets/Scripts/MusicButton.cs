using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MusicButton : MonoBehaviour
{
	[SerializeField] private Sprite musicOnSprite;
	[SerializeField] private Sprite musicOffSprite;

	private Image imageComponent;
	void OnEnable()
	{
		AudioManager.OnMusicStateChanged += UpdateButtonSprite;
	}
	void Start()
	{
		imageComponent = GetComponent<Image>();
		bool isMusicOn = AudioManager.Instance.IsMusicOn();
		UpdateButtonSprite(isMusicOn);
	}

	public void ToggleMusic()
	{
		bool isMusicOn = !AudioManager.Instance.IsMusicOn();
		AudioManager.Instance.SetMusic(isMusicOn);
	}

	public void UpdateButtonSprite(bool isMusicOn)
	{
		imageComponent.sprite = isMusicOn ? musicOnSprite : musicOffSprite;
	}
}
