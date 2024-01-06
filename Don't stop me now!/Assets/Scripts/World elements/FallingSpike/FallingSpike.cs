using System;
using ScriptableObjects;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class FallingSpike : MonoBehaviour
{
    public float cubeScale = 1.5f;
    
    // 0: start, 0.5 middle, 1 end of cube width
    public float hPosCubeHit = 0;
    // 0: bottom, 0.5 middle, 1 top of cube height
    public float yPosCubeHit = 0.5f;
    public float triggerHeight = 5;
    public float gravityScale;
    public IceCubeParameters parameters;
    private Rigidbody2D _spikeRb;

    private PolygonCollider2D _pc;
    private float _defaultCubeSpeed;
    
    void Start()
    {
        _spikeRb = GetComponentInChildren<Rigidbody2D>();
        // don't let the spike fall for the moment
        _spikeRb.gravityScale = 0;
        _pc = GetComponent<PolygonCollider2D>();
        //_doh.enabled = false;
        
        _defaultCubeSpeed = parameters.defaultSpeed;

        Vector2[] triangleColliderPoints = new Vector2[3];
        // upper point of the triangle trigger
        float upperY = -0.5f; // - cubeScale;
        // lower points of the triangle trigger
        float lowerY = -0.5f - triggerHeight;
        // physics shit to compute the base width in order to hit the cube with its default speed based on the height
        float halfBaseWidth = _defaultCubeSpeed * MathF.Sqrt((2 * (triggerHeight - cubeScale * yPosCubeHit)) / (gravityScale * MathF.Abs(Physics2D.gravity.y))) - cubeScale * hPosCubeHit;
        triangleColliderPoints[0] = new Vector2(0, upperY);
        triangleColliderPoints[1] = new Vector2(-halfBaseWidth, lowerY);
        triangleColliderPoints[2] = new Vector2(halfBaseWidth, lowerY);

        _pc.points = triangleColliderPoints;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // let the spike fall
            _spikeRb.gravityScale = gravityScale;
            transform.GetChild(0).GetComponent<DeathZone>().destroyOnHit = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // destroy the spike when exiting its own trigger
        if (other.attachedRigidbody == _spikeRb)
        {
            var fadeOut = other.AddComponent<FadeOutHandler>();
            fadeOut.startFadeTimerTarget = 0;
            fadeOut.fadeSpeed = 5;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        Debug.Log("On collision exit");
    }
}
