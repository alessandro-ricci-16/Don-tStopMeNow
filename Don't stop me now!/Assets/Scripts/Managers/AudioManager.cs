using System;
using System.Collections;
using System.Collections.Generic;
using Ice_Cube.States;
using UnityEngine;
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
    [Header("Volume")] [Range(0, 1)] 
    public float masterVolume = 0.8f;
    [Range(0, 1)] public float musicVolume = 1f;
    [Range(0, 1)] public float sfxVolume = 1f;

    [Header("Music")] 
    public SoundData[] songs;
    
    [Header("Game Sound Effects")] 
    public SoundData deathSound;
    public SoundData jumpSound;
    public SoundData groundPoundSound;
    public SoundData dashSound;
    
    [Header("UI Sound Effects")] 
    public SoundData buttonClickSound;
    
    // Music variables
    private int _currentSongIndex = 0;
    private bool _isLooping = false;
    private Coroutine _preloadCoroutine;
    private AudioSource _musicAudioSource;
    
    // Sound effects variables
    private AudioSource _sfxAudioSource;
    
    
    #region Inizialization
    
    private void OnEnable()
    {
        _musicAudioSource = gameObject.AddComponent<AudioSource>();
        _sfxAudioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
        PlaySong(_currentSongIndex);
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

    #region Songs

    public void PlaySong(int index)
    {
        _musicAudioSource.Stop();
        _musicAudioSource.clip = songs[index].sound;
        _musicAudioSource.volume = songs[index].volume * masterVolume * musicVolume;
        _musicAudioSource.Play();
        _currentSongIndex = index;

        // Check if the sound should loop automatically when played
        _isLooping = songs[index].loopOnPlay;
        _musicAudioSource.loop = _isLooping;

        // Start coroutine to automatically play next sound if it's not set to loop
        StartCoroutine(WaitForSongToEnd());
        // Check if there's a next song available
        int nextIndex = (index + 1) % songs.Length;

        // Start preloading the next song
        //PreloadSound(songs[nextIndex]);
    }

    private void PreloadSound(SoundData soundData)
    {
        if (soundData != null && soundData.sound is not null)
        {
            // Load the audio clip asynchronously
            ResourceRequest request = Resources.LoadAsync<AudioClip>(soundData.sound.name);

            // Wait until the audio clip is fully loaded
            StartCoroutine(WaitForSoundLoaded(request, soundData));
        }
    }

    private IEnumerator WaitForSoundLoaded(ResourceRequest request, SoundData soundData)
    {
        yield return request;
        soundData.sound = request.asset as AudioClip;
    }


    public void ToggleLoop(bool shouldLoop)
    {
        _musicAudioSource.loop = shouldLoop;
        _isLooping = shouldLoop;
    }

    private IEnumerator WaitForSongToEnd()
    {
        float songLength = _musicAudioSource.clip.length;
        yield return new WaitForSeconds(songLength);

        // If the sound is not set to loop, play the next sound
        if (!_isLooping)
        {
            PlayNextSong();
        }
    }

    public void PlayNextSong()
    {
        _currentSongIndex = (_currentSongIndex + 1) % songs.Length;
        PlaySong(_currentSongIndex);
    }

    public void PlayPreviousSong()
    {
        _currentSongIndex = (_currentSongIndex - 1 + songs.Length) % songs.Length;
        PlaySong(_currentSongIndex);
    }

    #endregion

    #region Sound effects and callbacks

    private void PlaySound(SoundData soundData)
    {
        if (soundData.sound != null)
        {
            // Debug.Log("Playing sound " + soundData.sound.name);
            _sfxAudioSource.PlayOneShot(soundData.sound, soundData.volume * masterVolume * sfxVolume);
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
}