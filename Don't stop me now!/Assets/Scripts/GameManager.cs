using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private Animator _canvasAnimator;
    private Vignette _vignetteEffect;
   

    protected override void Awake() 
    { 
        //invoke the awake method of the singleton class
        base.Awake();
        // get animator component
        _canvasAnimator = GetComponent<Animator>();
        // try to get the vignette component of the post processing volume
        Volume postProcessing = GetComponentInChildren<Volume>();
        if(postProcessing != null && postProcessing.profile.TryGet<Vignette>( out var tmp ) )
            _vignetteEffect = tmp;
        else
            Debug.Log("Cannot get vignette effect");
    }

    public void Die(Vector3 playerPosition)
    {
        // freeze time
        Time.timeScale = 0.0f;
        // center the vignette animation on the passed position
        if (Camera.main != null)
        {
            Vector2 vignetteCenter = Camera.main.WorldToViewportPoint(playerPosition);
            _vignetteEffect.center.value = vignetteCenter;
        }
        // play the reload scene state of the animator that will trigger the ReloadScene() method at the end
        _canvasAnimator.Play("ReloadScene");
    }
    
    public void ReloadScene()
    {
        ChangeScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MenuScene()
    {
        // menu scene will be scene 0
        ChangeScene(0);
    }
    
    public void NextScene()
    {
        // call me to load the next scene
        ChangeScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void ChangeScene(int scene)
    {
        StartCoroutine(LoadAsyncScene(scene));
    }
    
    private IEnumerator LoadAsyncScene(int scene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            // float progress = asyncLoad.progress;
            yield return null;
        }
        // play the fadeout animation once the scene is loaded
        _canvasAnimator.Play("FadeOut");
        // reset the time scale to normal
        Time.timeScale = 1.0f;
        // Debug.Log("Loaded scene " + scene);
    }
}
