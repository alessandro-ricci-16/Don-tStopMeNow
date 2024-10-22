using Ice_Cube.States;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeatedPlatform : MonoBehaviour
{
    // we have to create a function that when the player enters the heated platform, it will start a timer.
    // As long as it stays in contact with the platform the timer will increase,
    // when it reaches a max value the player will die

    public HeatableSettings heatableSettings;
    private float _timer;
    private bool _isPlayerOn;
    private IceCubeStateManager _iceCubeStateManager;
    private bool _sentDeathEvent;

    private void Update()
    {
        if (_isPlayerOn)
        {
            _timer += Time.deltaTime;
        }
        else if (_timer > 0)
        {
            if (_iceCubeStateManager is not null &&
                _iceCubeStateManager.GetCurrentState().GetEnumState() != IceCubeStatesEnum.IsDashing)
            {
                _timer -= heatableSettings.recoveryScale * Time.deltaTime;
                _timer = Mathf.Clamp(_timer, 0, heatableSettings.maxTime);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (_iceCubeStateManager is null)
                _iceCubeStateManager = other.gameObject.GetComponent<IceCubeStateManager>();
            EventManager.TriggerEvent(EventNames.OnHeatedPlatform, _timer);
            _isPlayerOn = true;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (_timer >= heatableSettings.maxTime && !_sentDeathEvent && other.gameObject.CompareTag("Player"))
        {
            var position = other.gameObject.transform.position;
            other.gameObject.GetComponent<Explodable>().Explode();
            EventManager.TriggerEvent(EventNames.Death, SceneManager.GetActiveScene().name, position);
            _sentDeathEvent = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _isPlayerOn = false;
            EventManager.TriggerEvent(EventNames.OffHeatedPlatform, _timer);
        }
    }
}