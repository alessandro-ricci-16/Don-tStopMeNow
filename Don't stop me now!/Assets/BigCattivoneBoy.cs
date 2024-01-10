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

    private Rigidbody2D _rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
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
        }
        else if (isPlayer)
        {
            //in any other case the cube need to stand still
            _rigidbody2D.velocity = Vector2.zero;
        }
    }
}