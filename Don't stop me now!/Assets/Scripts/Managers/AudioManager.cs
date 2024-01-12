using System;
using System.Collections;
using Ice_Cube.States;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class SoundData
{
    public AudioClip sound;
    public float volume = 1f;
}

public class AudioManager : Singleton<AudioManager>
{
    [Header("Music")] public SoundData intro;
    public SoundData loopWorld1;
    public SoundData loopWorld2;

    [Header("Game Sound Effects")] public SoundData deathSound;
    public SoundData jumpSound;
    public SoundData groundPoundSound;
    public SoundData dashSound;
    public SoundData breakingPlatformSound;
    public SoundData heatedPlatformSound;
    [Header("UI Sound Effects")] public SoundData buttonClickSound;

    // volumes
    [Range(0, 1)] private float _masterVolume = 0.8f;
    [Range(0, 1)] private float _musicVolume = 0.8f;
    [Range(0, 1)] private float _sfxVolume = 0.8f;

    // Music variables
    private SoundData _currentSong;
    private Coroutine _preloadCoroutine;
    private AudioSource _musicAudioSource;

    // Sound effects variables
    private AudioSource _sfxAudioSource;

    // to avoid unnecessary computation
    private int _prevSceneIndex = -1;


    #region Inizialization

    private void Start()
    {
        Debug.Log("Initializing audio manager");

        _musicAudioSource = gameObject.AddComponent<AudioSource>();
        _sfxAudioSource = gameObject.AddComponent<AudioSource>();
        _prevSceneIndex = SceneManager.GetActiveScene().buildIndex;

        StartCoroutine(PlayIntro());

        EventManager.StartListening(EventNames.Death, OnDeath);
        EventManager.StartListening(EventNames.StateChanged, OnStateChanged);
        EventManager.StartListening(EventNames.BreakingPlatform, OnPlatformBreaking);
        EventManager.StartListening(EventNames.NewSceneLoaded, UpdateMusic);
        EventManager.StartListening(EventNames.OnHeatedPlatform, OnHeatedPlatform);
        EventManager.StartListening(EventNames.OffHeatedPlatform, OffHeatedPlatform);
    }

    private void OnDestroy()
    {
        Debug.Log("Destroying audio manager");
        EventManager.StopListening(EventNames.Death, OnDeath);
        EventManager.StopListening(EventNames.StateChanged, OnStateChanged);
        EventManager.StopListening(EventNames.BreakingPlatform, OnPlatformBreaking);
        EventManager.StopListening(EventNames.NewSceneLoaded, UpdateMusic);
        EventManager.StopListening(EventNames.OnHeatedPlatform, OnHeatedPlatform);
        EventManager.StopListening(EventNames.OffHeatedPlatform, OffHeatedPlatform);
    }

    #endregion


    #region Music

    private IEnumerator PlayIntro()
    {
        // update current song
        _currentSong = intro;

        _musicAudioSource.Stop();
        _musicAudioSource.clip = intro.sound;
        _musicAudioSource.volume = intro.volume * _masterVolume * _musicVolume;
        _musicAudioSource.Play();

        _musicAudioSource.loop = false;

        yield return new WaitForSeconds(intro.sound.length - 1.0f);

        // if the intro is still the current song (haven't gone into world 2), then play world 1 loop
        if (_currentSong == intro)
        {
            StartCoroutine(FadeOutAndPlayLoop(loopWorld1, delay: 0f));
        }
    }

    private IEnumerator FadeOutAndPlayLoop(SoundData song, float fadeOutDuration = 1.0f, float delay = 1.0f,
        float fadeInDuration = 1.0f)
    {
        // FADE OUT
        float startVolume = _musicAudioSource.volume;
        while (_musicAudioSource.volume > 0)
        {
            _musicAudioSource.volume -= startVolume * Time.unscaledDeltaTime / fadeOutDuration;
            yield return new WaitForEndOfFrame();
        }

        _musicAudioSource.Stop();

        // DELAY
        yield return new WaitForSeconds(delay);

        // FADE IN
        _currentSong = song;
        _musicAudioSource.clip = song.sound;
        _musicAudioSource.Play();
        _musicAudioSource.loop = true;
        while (_musicAudioSource.volume < song.volume * _masterVolume * _musicVolume)
        {
            _musicAudioSource.volume += startVolume * Time.unscaledDeltaTime / fadeInDuration;
            yield return new WaitForEndOfFrame();
        }
    }

    private void UpdateMusic()
    {
        // avoid computing update unnecessarily
        int newSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (_prevSceneIndex == newSceneIndex)
        {
            return;
        }

        if (_currentSong == intro || _currentSong == loopWorld1)
        {
            if (GameManager.Instance.SceneIsWorld2Screen() || GameManager.Instance.SceneIsWorld2())
            {
                // stop other coroutines which might be executing
                StopAllCoroutines();
                StartCoroutine(FadeOutAndPlayLoop(loopWorld2));
            }
        }
        else if (_currentSong == loopWorld2)
        {
            if (GameManager.Instance.SceneIsWorld1())
            {
                // stop other coroutines which might be executing
                StopAllCoroutines();
                StartCoroutine(FadeOutAndPlayLoop(loopWorld1));
            }
        }
    }

    #endregion

    #region Sound effects and callbacks

    private void PlaySound(SoundData soundData)
    {
        if (soundData.sound != null)
        {
            _sfxAudioSource.PlayOneShot(soundData.sound, soundData.volume * _masterVolume * _sfxVolume);
        }
    }

    private void OnDeath()
    {
        PlaySound(deathSound);
    }

    private void OnHeatedPlatform()
    {
        if (heatedPlatformSound.sound != null)
        {
            if (_sfxAudioSource.clip != heatedPlatformSound.sound)
                _sfxAudioSource.clip = heatedPlatformSound.sound;
            _sfxAudioSource.Play();
        }
    }

    private void OffHeatedPlatform()
    {
        _sfxAudioSource.Stop();
    }

    private void OnStateChanged(IceCubeStatesEnum previous, IceCubeStatesEnum current)
    {
        if (current == IceCubeStatesEnum.IsGroundPounding)
        {
            PlaySound(groundPoundSound);
            return;
        }

        if (current is IceCubeStatesEnum.IsJumping or IceCubeStatesEnum.IsWallJumping)
        {
            PlaySound(jumpSound);
            return;
        }

        if (current == IceCubeStatesEnum.IsDashing)
        {
            PlaySound(dashSound);
        }
    }

    // does not have a callback because it is called by the button script
    public void PlayButtonClickSound()
    {
        PlaySound(buttonClickSound);
    }

    private void OnPlatformBreaking()
    {
        PlaySound(breakingPlatformSound);
    }

    #endregion

    #region Getters and setters

    public float GetMasterVolume()
    {
        return _masterVolume;
    }

    public float GetMusicVolume()
    {
        return _musicVolume;
    }

    public float GetSfxVolume()
    {
        return _sfxVolume;
    }

    public void SetMasterVolume(float value)
    {
        _masterVolume = value;
        _musicAudioSource.volume = _currentSong.volume * _masterVolume * _musicVolume;
    }

    public void SetMusicVolume(float value)
    {
        _musicVolume = value;
        _musicAudioSource.volume = _currentSong.volume * _masterVolume * _musicVolume;
    }

    public void SetSfxVolume(float value)
    {
        _sfxVolume = value;
    }

    #endregion
}