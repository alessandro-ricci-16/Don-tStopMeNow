using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSlider : MonoBehaviour
{
    public enum TypeOfSounds
    {
        Music,
        Sfx
    }

    public TypeOfSounds type;
    private Slider _slider;
    public float delayInSeconds = 0.5f;

    private float _lastValue;

    private Coroutine _soundCoroutine;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    // Start is called before the first frame update
    void OnEnable()
    {
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
                _soundCoroutine = StartCoroutine(PlayDelayedSound());*/
                AudioManager.Instance.PlayButtonClickSound();
            }
        });
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