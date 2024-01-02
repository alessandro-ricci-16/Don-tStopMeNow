using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCounter : MonoBehaviour
{
    private int _deathCounter = 0;
    private TMPro.TextMeshProUGUI _textComponent;

    // Start is called before the first frame update
    private void Start()
    {
        EventManager.StartListening(EventNames.Death, DeathCounterFunction);
        //get text component
        _textComponent = GetComponent<TMPro.TextMeshProUGUI>();
    }
    
    private void DeathCounterFunction(String levelName, Vector3 playerPosition)
    {
        _deathCounter++;
        //add a 0 before the number if it is less than 10
        if (_deathCounter < 10)
        {
            _textComponent.text = "X 0" + _deathCounter;
        }
        else
        {
            _textComponent.text = "X " + _deathCounter;
        }
    }
}