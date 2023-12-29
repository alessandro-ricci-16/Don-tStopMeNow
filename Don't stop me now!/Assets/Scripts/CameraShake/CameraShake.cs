using Cinemachine;
using Ice_Cube.States;
using UnityEngine;


public class CameraShake : MonoBehaviour
{
    public float duration = 0.3f;
    public float magnitude = 0.1f;
    public float frequency = 2f;
    private CinemachineVirtualCamera _cinemachineVirtualCamera;
    private CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;
    private float _shakeTimer;
    private float _originalZ;

    public void OnEnable()
    {
        _originalZ = transform.position.z;
        _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        EventManager.StartListening(EventNames.StateChanged, OnStateChanged);
        _cinemachineBasicMultiChannelPerlin =
            _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void ShakeCamera()
    {
        _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = magnitude;
        _cinemachineBasicMultiChannelPerlin.m_FrequencyGain = frequency;
        _shakeTimer = duration;
    }

    private void Update()
    {
        if (_shakeTimer > 0)
        {
            _shakeTimer -= Time.deltaTime;
            if (_shakeTimer <= 0f)
            {
                _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
                _cinemachineBasicMultiChannelPerlin.m_FrequencyGain = 0f;
            }
        }

        //Avoid the camera to shake along the z axis
        var tra = transform;
        //leave this comparison as it is, it's not a mistake
        if (tra.position.z!=_originalZ)
            tra.position = new Vector3(tra.position.x, transform.position.y, _originalZ);
    }


    private void OnStateChanged(IceCubeStatesEnum previous, IceCubeStatesEnum current)
    {
        if (previous == IceCubeStatesEnum.IsGroundPounding && current == IceCubeStatesEnum.OnGround)
        {
            ShakeCamera();
        }
    }
}