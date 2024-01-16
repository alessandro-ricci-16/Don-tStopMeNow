using System;
using System.Collections;
using System.Collections.Generic;
using Ice_Cube.States;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BigCattivoneBoy : MonoBehaviour
{
    public float intensity;
    private IceCubeStateManager _iceCubeStateManager;
    private bool _alreadyBeenPushed;
    private Rigidbody2D _rigidbody2D;
    public bool needToSwitchTransformAtCheckpoint = false;
    public Vector3 positionAfterCheckpoint;
    public int id;

    private void Awake()
    {
        if (needToSwitchTransformAtCheckpoint)
            EventManager.StartListening(EventNames.CheckpointPassed, OnCheckPointPassed);
    }

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        if (GameManager.Instance.StartAtCheckPoint && needToSwitchTransformAtCheckpoint)
        {
            _alreadyBeenPushed = GameManager.Instance.GetCheckpointsVariable(id);
        }

        if (_alreadyBeenPushed && needToSwitchTransformAtCheckpoint)
        {
            transform.position = positionAfterCheckpoint;
        }
    }

    private void OnCheckPointPassed()
    {
        if (needToSwitchTransformAtCheckpoint)
        {
            GameManager.Instance.SetCheckpointsVariable(id, _alreadyBeenPushed);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        bool isPlayer = other.gameObject.CompareTag("Player");
        if (isPlayer && _iceCubeStateManager is null)
        {
            _iceCubeStateManager = other.gameObject.GetComponent<IceCubeStateManager>();
            if (_iceCubeStateManager is null)
            {
                Debug.LogError("Cannot get IceCubeStateManager from player");
                return;
            }
        }

        if (isPlayer &&
            _iceCubeStateManager.GetCurrentState().GetEnumState() == IceCubeStatesEnum.IsDashing)
        {
            //add the force in the same direction as the velocity of the other

            _rigidbody2D.AddForce(other.relativeVelocity.normalized * intensity, ForceMode2D.Impulse);
            _alreadyBeenPushed = true;
        }
        else if (isPlayer)
        {
            //in any other case the cube need to stand still
            _rigidbody2D.velocity = Vector2.zero;
        }
    }
}