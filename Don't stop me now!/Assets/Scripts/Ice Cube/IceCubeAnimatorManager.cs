using System;
using System.Collections;
using System.Collections.Generic;
using Ice_Cube.States;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(TrailRenderer))]
[RequireComponent(typeof(IceCubeStateManager))]
public class IceCubeAnimatorManager : MonoBehaviour
{
    public Material cubeGlowMaterial;
    public Material glowTrailMaterial;
    public Sprite dashingSprite;
    public GameObject jumpAnimation;
    private GameObject _instanceJump;
    private GameObject _instanceWallJump;
    private Animator _animator;
    private IceCubeStateManager _stateManager;
    private TrailRenderer _trailRenderer;
    private int _maxTime;
    public HeatableSettings heatableSettings;
    public IceCubeParameters parameters;
    private SpriteRenderer _spriteRenderer;
    private Material _normalMaterial;
    private Material _normalTrailMaterial;
    private Sprite _normalSprite;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _stateManager = GetComponent<IceCubeStateManager>();
        _trailRenderer = GetComponent<TrailRenderer>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _normalMaterial = _spriteRenderer.material;
        _normalTrailMaterial = _trailRenderer.material;
        _instanceJump = Instantiate(jumpAnimation); //Spawn a copy of 'prefab' and store a reference to it.
        _instanceWallJump = Instantiate(jumpAnimation);
        //rotate the instance wall jump z of 90 degrees
        _instanceWallJump.transform.Rotate(0, 0, 90);
        //flip the sprite render along y of the instance wall jump
        _instanceWallJump.GetComponent<SpriteRenderer>().flipY = true;
        _instanceJump.SetActive(false); //turn off the instance
        EventManager.StartListening(EventNames.StateChanged, OnStateChanged);
    }

    private void OnStateChanged(IceCubeStatesEnum previousState, IceCubeStatesEnum currentState)
    {
        switch (currentState)
        {
            case IceCubeStatesEnum.IsJumping:
                _instanceJump.transform.position = transform.position;
                _instanceJump.SetActive(true);
                break;
            case IceCubeStatesEnum.IsGroundPounding:
                _animator.SetFloat(Animator.StringToHash("groundPoundScale"),
                    1 / (parameters.groundPoundTimeSlowDown * parameters.groundPoundTimeScale));
                _animator.SetBool(Animator.StringToHash("isGroundPounding"), true);
                break;
            case IceCubeStatesEnum.IsDashing:
                _animator.SetFloat(Animator.StringToHash("dashScale"), 1 / parameters.dashDuration);
                _animator.SetBool(Animator.StringToHash("isDashing"), true);
                _spriteRenderer.material = cubeGlowMaterial;
                _trailRenderer.material = glowTrailMaterial;
                _spriteRenderer.sprite = dashingSprite;
                break;
            case IceCubeStatesEnum.IsWallJumping:
                _instanceWallJump.transform.position = transform.position;
                _instanceWallJump.SetActive(true);
                break;
        }

        switch (previousState)
        {
            case IceCubeStatesEnum.IsGroundPounding:
                _animator.SetBool(Animator.StringToHash("isGroundPounding"), false);
                break;
            case IceCubeStatesEnum.IsDashing:
                _animator.SetBool(Animator.StringToHash("isDashing"), false);
                //change the material of the trail to the normal one
                _trailRenderer.material = _normalTrailMaterial;
                //change the sprite material of the cube
                _spriteRenderer.material = _normalMaterial;
                break;
        }
    }


    private void OnDestroy()
    {
        EventManager.StopListening(EventNames.StateChanged, OnStateChanged);
    }


    private void OnEnable()
    {
        //Call the event manager to start listening to the event "OnHeatedPlatform"
        EventManager.StartListening(EventNames.OnHeatedPlatform, (UnityAction<float>)OnHeatedPlatform);
        //Call the event manager to start listening to the event "OffHeatedPlatform"
        EventManager.StartListening(EventNames.OffHeatedPlatform, (UnityAction<float>)OffHeatedPlatform);
    }

    private void OnHeatedPlatform(float currentTimer)
    {
        // Set the playback speed (m_t) for the animation to match settings.maxTime
        float playbackSpeed = 1 / heatableSettings.maxTime;
        //we use Animator.StringToHash because otherwise intellij starts crying
        _animator.SetFloat(Animator.StringToHash("heatScale"), playbackSpeed);
        // Calculate the normalized time to start the animation from a specific percentage which is given by the normalization of our currentTimer
        _animator.Play("OnHeatedPlatform", 0, currentTimer / heatableSettings.maxTime);
    }

    private void OffHeatedPlatform(float currentTimer)
    {
        //similar to OnHeatedPlatform but we use recovery scale and play the animation in reverse
        //recoveryScale is a percentage on how fast the ice cube will recover
        float playbackSpeed = -heatableSettings.recoveryScale;
        _animator.SetFloat(Animator.StringToHash("heatScale"), playbackSpeed);
    }
}