using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] AudioClip buttonSound;
    [SerializeField] AudioClip matchSound;
    [SerializeField] AudioClip selectTileSound;
    [SerializeField] AudioClip backgroundMusic;
    [SerializeField] AudioClip levelCompleteSound;
    [SerializeField] AudioClip gameOverSound;

    private AudioSource soundEffectsSource;
    private AudioSource musicSource;

    private bool suppressSelectSound = false;

    public delegate void SoundStateChanged(bool isSoundOn);
    public static event SoundStateChanged OnSoundStateChanged;

    public delegate void MusicStateChanged(bool isMusicOn);
    public static event MusicStateChanged OnMusicStateChanged;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        AudioSource[] sources = GetComponents<AudioSource>();
        soundEffectsSource = sources[0];
        musicSource = sources[1];

        TileSelectionHandler.OnTilesMatch += PlayMatchSound;
        GameManager.OnWin += PlayLevelCompleteSound;
        GameManager.OnLose += PlayGameOverSound;
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
        soundEffectsSource.PlayOneShot(buttonSound);
    }

    public void PlaySelectTileSound()
    {
        if (selectTileSound != null && IsSoundOn() && !suppressSelectSound)
            soundEffectsSource.PlayOneShot(selectTileSound);
    }

    public void PlayMatchSound()
    {
        suppressSelectSound = true;
        soundEffectsSource.PlayOneShot(matchSound);
        StartCoroutine(ResetSuppressFlag());
    }

    private IEnumerator ResetSuppressFlag()
    {
        yield return null; // Wait one frame
        suppressSelectSound = false;
    }

    public void PlayLevelCompleteSound()
    {
        if (IsSoundOn())
        {
            soundEffectsSource.PlayOneShot(levelCompleteSound);
        }
    }

    public void PlayGameOverSound()
    {
        if (IsSoundOn())
        {
            soundEffectsSource.PlayOneShot(gameOverSound);
        }
    }

    public void PlayBackgroundMusic()
    {
        if (IsMusicOn())
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }
}
