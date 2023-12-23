using System;
using System.Collections;
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
    [Header("Volume")]
    [Range(0, 1)] public float masterVolume = 0.8f;
    [Range(0, 1)] public float musicVolume = 1f;
    [Range(0, 1)] public float sfxVolume = 1f;
    
    [Header("Music")]
    private AudioSource _musicAudioSource;
    public SoundData[] songs;
    private int _currentSongIndex = 0;
    private bool _isLooping = false;
    
    [Header("Sound Effects")]
    private AudioSource _sfxAudioSource;
    public SoundData deathSound;

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
        _sfxAudioSource.PlayOneShot(soundData.sound, soundData.volume * masterVolume * sfxVolume);
    }
    
    public void OnDeath()
    {
        PlaySound(deathSound);
    }
    
    #endregion
}