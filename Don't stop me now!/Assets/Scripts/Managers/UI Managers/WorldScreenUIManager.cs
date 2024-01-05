using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldScreenUIManager : MonoBehaviour
{
    public float displayTime = 2f;
    
    private void Start()
    {
        StartCoroutine(LoadAfterDelay());
    }
    
    private IEnumerator LoadAfterDelay()
    {
        yield return new WaitForSeconds(displayTime);
        GameManager.Instance.LoadNextScene();
    }
}
