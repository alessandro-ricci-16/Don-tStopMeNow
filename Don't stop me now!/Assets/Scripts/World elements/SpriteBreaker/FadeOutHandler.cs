using UnityEngine;
using UnityEngine.Serialization;

public class FadeOutHandler : MonoBehaviour
{
    public float fadeSpeed = 1;
    public int destroyDistance = 50;
    public float startFadeTimerTarget = 1;

    private Rigidbody2D _rb;
    private bool _startFade = false;
    private float _forceFadeTimer;
    private Camera _camera;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = transform.childCount > 0 ? transform.GetChild(0).GetComponent<SpriteRenderer>() : transform.GetComponent<SpriteRenderer>();
        _camera = Camera.main;
        _rb = GetComponent<Rigidbody2D>();
    }

    private void LateUpdate()
    {
        var distanceToCamera = Vector2.Distance(transform.position, _camera.transform.position);
        if (distanceToCamera > destroyDistance) Destroy(gameObject);
        else if (_startFade)
        {
            Color color = _spriteRenderer.color;
            color.a -=  fadeSpeed * Time.deltaTime;
            if(color.a <= 0) Destroy(gameObject);
            _spriteRenderer.color = color;
        }
        else
        {
            if (_rb is not null && _rb.velocity.x == 0) _startFade = true;
            if (_forceFadeTimer < startFadeTimerTarget) _forceFadeTimer += Time.deltaTime;
            else _startFade = true;
        }
    }
}
