using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager Instance { get; private set; }

	[SerializeField] AudioClip buttonSound;
	[SerializeField] AudioClip matchSound;
	[SerializeField] AudioClip backgroundMusic;
	[SerializeField] AudioClip levelCompleteSound;
	[SerializeField] AudioClip gameOverSound;
	private AudioSource soundEffectsSource;
	private AudioSource musicSource;

	public delegate void SoundStateChanged(bool isSoundOn);
	public static event SoundStateChanged OnSoundStateChanged;

	public delegate void MusicStateChanged(bool isMusicOn);
	public static event MusicStateChanged OnMusicStateChanged;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
		AudioSource[] sources = GetComponents<AudioSource>();
		soundEffectsSource = sources[0];
		musicSource = sources[1];

		TileSelectionHandler.OnTilesMatch += PlayMatchSound;
		GameManager.OnWin+=PlayLevelCompleteSound;
		GameManager.OnLose+=PlayGameOverSound;
	}

	public void SetSound(bool isSoundOn)
	{
		soundEffectsSource.mute = !isSoundOn;
		OnSoundStateChanged?.Invoke(isSoundOn);
	}

	public bool IsSoundOn()
	{
		return !soundEffectsSource.mute;
	}

	public void SetMusic(bool isMusicOn)
	{
		musicSource.mute = !isMusicOn;
		OnMusicStateChanged?.Invoke(isMusicOn);
	}

	public bool IsMusicOn()
	{
		return !musicSource.mute;
	}

	public void PlayButtonSound()
	{
		soundEffectsSource.clip = buttonSound;
		soundEffectsSource.Play();
	}

	public void PlayMatchSound()
	{
		soundEffectsSource.clip = matchSound;
		soundEffectsSource.Play();
	}

	public void PlayLevelCompleteSound()
	{
		if (IsMusicOn())
		{
			musicSource.clip = levelCompleteSound;
			musicSource.Play();
		}
	}

	public void PlayGameOverSound()
	{
		if (IsMusicOn())
		{
			musicSource.clip = gameOverSound;
			musicSource.Play();
		}
	}

	public void PlayBackgroundMusic()
	{
		musicSource.clip = backgroundMusic;
		musicSource.Play();
	}
}


