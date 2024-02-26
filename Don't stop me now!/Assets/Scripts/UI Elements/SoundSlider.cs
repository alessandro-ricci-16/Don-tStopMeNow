using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TypeOfSounds
{
    Master,
    Music,
    Sfx
}

public class SoundSlider : MonoBehaviour
{
    public TypeOfSounds type;
    private Slider _slider;
    public float delayInSeconds = 0.5f;

    private float _lastValue;

    private Coroutine _soundCoroutine;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        UpdateSliderValues();
    }


    // Start is called before the first frame update
    void OnEnable()
    {
        UpdateSliderValues();

        _slider.onValueChanged.AddListener((v) =>
        {
            if (type == TypeOfSounds.Master)
            {
                AudioManager.Instance.SetMasterVolume(v);
            }
            else if (type == TypeOfSounds.Music)
            {
                AudioManager.Instance.SetMusicVolume(v);
            }
            else
            {
                AudioManager.Instance.SetSfxVolume(v);
                AudioManager.Instance.PlayButtonClickSound();
            }
        });

        /*
        _slider.onValueChanged.AddListener((v) =>
        {

            if (type == TypeOfSounds.Music)
            {
                AudioManager.Instance.OnVolumeMusicChanged(v);
            }
            else
            {
                AudioManager.Instance.OnVolumeSfxChanged(v);
                //for some reason this doesn't work
                /*if (Mathf.Approximately(v, _lastValue))
                {
                    return; // Do nothing if the value hasn't changed
                }

                _lastValue = v;

                // Stop the previous coroutine if it's running
                if (_soundCoroutine != null)
                {
                    StopCoroutine(_soundCoroutine);
                }

                // Start a new coroutine to play the sound after the delay
                _soundCoroutine = StartCoroutine(PlayDelayedSound());
                AudioManager.Instance.PlayButtonClickSound();
            }
        });
        */
    }

    private void UpdateSliderValues()
    {
        _slider.value = type switch
        {
            TypeOfSounds.Master => AudioManager.Instance.GetMasterVolume(),
            TypeOfSounds.Music => AudioManager.Instance.GetMusicVolume(),
            TypeOfSounds.Sfx => AudioManager.Instance.GetSfxVolume(),
            _ => _slider.value
        };
    }

    //coroutine for playing sfx sound
    private IEnumerator PlayDelayedSound()
    {
        Debug.Log("waiting for seconds");
        yield return new WaitForSeconds(delayInSeconds);
        Debug.Log("playing sound");
        AudioManager.Instance.PlayButtonClickSound();
    }
}