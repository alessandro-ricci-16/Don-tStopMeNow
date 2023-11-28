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
    public Material normalTrailMaterial;
    public Material glowTrailMaterial;
    
    private Animator _animator;
    private IceCubeStateManager _stateManager;
    private TrailRenderer _trailRenderer;
    private int _maxTime;
    public HeatableSettings heatableSettings;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _stateManager = GetComponent<IceCubeStateManager>();
        _trailRenderer = GetComponent<TrailRenderer>();
    }

    private void FixedUpdate()
    {
        IceCubeState currentState = _stateManager.GetCurrentState();

        if (currentState.GetEnumState() == IceCubeStatesEnum.IsDashing)
        {
            _animator.SetBool("isDashing", true);
            // _trailRenderer.SetMaterials(new List<Material> {glowTrailMaterial});
        }
        else
        {
            _animator.SetBool("isDashing", false);
            // _trailRenderer.SetMaterials(new List<Material> {normalTrailMaterial});
        }
    }

    private void OnEnable()
    {
        //Call the event manager to start listening to the event "OnHeatedPlatform"
        EventManager.StartListening(EventNames.OnHeatedPlatform, (UnityAction<float>) OnHeatedPlatform);
        //Call the event manager to start listening to the event "OffHeatedPlatform"
        EventManager.StartListening(EventNames.OffHeatedPlatform, (UnityAction<float>)OffHeatedPlatform);
    }

    private void OnHeatedPlatform(float currentTimer)
    {
        // Set the playback speed (m_t) for the animation to match settings.maxTime
        float playbackSpeed = 1/heatableSettings.maxTime;
        //we use Animator.StringToHash because otherwise intellij starts crying
        _animator.SetFloat(Animator.StringToHash("heatScale"), playbackSpeed);
        // Calculate the normalized time to start the animation from a specific percentage which is given by the normalization of our currentTimer
        _animator.Play("OnHeatedPlatform", 0 , currentTimer/heatableSettings.maxTime);
    }

    private void OffHeatedPlatform(float currentTimer)
    {
        //similar to OnHeatedPlatform but we use recovery scale and play the animation in reverse
        //recoveryScale is a percentage on how fast the ice cube will recover
        float playbackSpeed = -heatableSettings.recoveryScale;
        _animator.SetFloat(Animator.StringToHash("heatScale"), playbackSpeed);
    }
}