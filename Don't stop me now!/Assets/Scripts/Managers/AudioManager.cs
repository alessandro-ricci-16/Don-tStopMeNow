using System;
using System.Collections;
using System.Collections.Generic;
using Ice_Cube.States;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[Serializable]
public class SoundData
{
    public AudioClip sound;
    public bool loopOnPlay;
    public float volume = 1f;
}

public class AudioManager : Singleton<AudioManager>
{
    [Header("Music")] 
    public SoundData intro;
    public SoundData loopWorld1;
    public SoundData loopWorld2;

    [Header("Game Sound Effects")] public SoundData deathSound;
    public SoundData jumpSound;
    public SoundData groundPoundSound;
    public SoundData dashSound;

    [Header("UI Sound Effects")] 
    public SoundData buttonClickSound;

    // volumes
    [Range(0, 1)] private float _masterVolume = 0.8f;
    [Range(0, 1)] private float _musicVolume = 0.8f;
    [Range(0, 1)] private float _sfxVolume = 0.8f;

    // Music variables
    // private int _currentSongIndex = 0;
    // private bool _isLooping = false;
    private SoundData _currentSong;
    private Coroutine _preloadCoroutine;
    private AudioSource _musicAudioSource;

    // Sound effects variables
    private AudioSource _sfxAudioSource;


    #region Inizialization

    private new void Awake()
    {
        base.Awake();
        _musicAudioSource = gameObject.AddComponent<AudioSource>();
        _sfxAudioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(PlayIntro());
        EventManager.StartListening(EventNames.Death, OnDeath);
        EventManager.StartListening(EventNames.StateChanged, OnStateChanged);
        //EventManager.StartListening(EventNames.CollisionWithGround, OnCollisionWithGround);
        //EventManager.StartListening(EventNames.ChangedDirection, OnCollisionWithGround);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(EventNames.Death, OnDeath);
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

        yield return new WaitForSeconds(intro.sound.length);
        
        // if the intro is still the current song (haven't gone into world 2), then play world 1 loop
        if (_currentSong == intro)
        {
            _musicAudioSource.Stop();
            _musicAudioSource.clip = loopWorld1.sound;
            _musicAudioSource.volume = loopWorld1.volume * _masterVolume * _musicVolume;
            _musicAudioSource.Play();
            _musicAudioSource.loop = true;
        }
    }
    
    private IEnumerator FadeOutAndPlayLoop(SoundData song, float fadeOutDuration = 1.0f, float delay = 1.0f,
        float fadeInDuration = 1.0f)
    {
        // FADE OUT
        float startVolume = _musicAudioSource.volume;
        while (_musicAudioSource.volume > 0)
        {
            _musicAudioSource.volume -= startVolume * Time.deltaTime / fadeOutDuration;
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
            _musicAudioSource.volume += startVolume * Time.deltaTime / fadeInDuration;
            yield return new WaitForEndOfFrame();
        }
    }

    public void UpdateMusic()
    {
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
            // Debug.Log("Playing sound " + soundData.sound.name);
            _sfxAudioSource.PlayOneShot(soundData.sound, soundData.volume * _masterVolume * _sfxVolume);
        }
    }

    private void OnDeath()
    {
        PlaySound(deathSound);
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