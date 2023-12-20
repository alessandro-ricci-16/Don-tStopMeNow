using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class SongData
{
    public AudioClip song;
    public bool loopOnPlay;
}

[RequireComponent(typeof(AudioSource))]
public class AudioManager : Singleton<AudioManager>
{
    [Header("Music")]
    private AudioSource _audioSource;
    public SongData[] songs;
    private int _currentSongIndex = 0;
    private bool _isLooping = false;
    
    [Header("Sound Effects")]
    public AudioClip deathSound;

    #region Inizialization
    
    private void OnEnable()
    {
        _audioSource = GetComponent<AudioSource>();
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
        _audioSource.Stop();
        _audioSource.clip = songs[index].song;
        _audioSource.Play();
        _currentSongIndex = index;

        // Check if the song should loop automatically when played
        _isLooping = songs[index].loopOnPlay;
        _audioSource.loop = _isLooping;

        // Start coroutine to automatically play next song if it's not set to loop
        StartCoroutine(WaitForSongToEnd());
    }

    public void ToggleLoop(bool shouldLoop)
    {
        _audioSource.loop = shouldLoop;
        _isLooping = shouldLoop;
    }

    private IEnumerator WaitForSongToEnd()
    {
        float songLength = _audioSource.clip.length;
        yield return new WaitForSeconds(songLength);

        // If the song is not set to loop, play the next song
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
    
    #region Callbacks and Sounds
    
    public void OnDeath()
    {
        _audioSource.PlayOneShot(deathSound);
    }
    
    #endregion
}