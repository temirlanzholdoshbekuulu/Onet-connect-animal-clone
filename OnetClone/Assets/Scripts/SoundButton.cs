using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SoundButton : MonoBehaviour
{
	[SerializeField] private Sprite soundOnSprite;
	[SerializeField] private Sprite soundOffSprite;

	private Image imageComponent;
	void OnEnable()
	{
		AudioManager.OnSoundStateChanged += UpdateButtonSprite;
	}
	void Start()
	{
		imageComponent = GetComponent<Image>();
		bool isSoundOn = AudioManager.Instance.IsSoundOn();
		UpdateButtonSprite(isSoundOn);
	}

	public void ToggleSound()
	{
		bool isSoundOn = !AudioManager.Instance.IsSoundOn();
		AudioManager.Instance.SetSound(isSoundOn);
	}

	public void UpdateButtonSprite(bool isSoundOn)
	{
		imageComponent.sprite = isSoundOn ? soundOnSprite : soundOffSprite;
	}
}

