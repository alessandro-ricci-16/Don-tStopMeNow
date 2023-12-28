using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonWithSound : Button
{
    protected override void Awake()
    {
        base.Awake();
        onClick.AddListener(PlaySound);
    }

    private void PlaySound()
    {
        AudioManager.Instance.PlayButtonClickSound();
    }
}
