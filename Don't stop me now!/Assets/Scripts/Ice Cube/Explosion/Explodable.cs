using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class Explodable : MonoBehaviour
{
    public float forceMagnitude;
    public float upwardsModifier = 0.1f;
    public enum ShatterType
    {
        Triangle,
        Voronoi
    };

    public ShatterType shatterType;
    private List<GameObject> _fragments = new List<GameObject>();

    /// <summary>
    /// Creates fragments if necessary and destroys original gameobject
    /// </summary>
    public void Explode()
    {
        generateFragments();
        foreach (GameObject frag in _fragments)
        {
            frag.transform.parent = null;
            frag.SetActive(true);
            //add explosion force to each piece
            Rigidbody2D rb = frag.GetComponent<Rigidbody2D>();
            rb.velocity = 0.1f *rb.velocity;
            if (rb != null)
            {
                Vector2 explosionDir = rb.position - (Vector2) transform.position;
                var explosionDistance = explosionDir.magnitude;

                // Normalize without computing magnitude again
                if (upwardsModifier == 0)
                    explosionDir /= explosionDistance;
                else {
                    // From Rigidbody.AddExplosionForce doc:
                    // If you pass a non-zero value for the upwardsModifier parameter, the direction
                    // will be modified by subtracting that value from the Y component of the centre point.
                    explosionDir.y += upwardsModifier;
                    explosionDir.Normalize();
                }
                rb.gravityScale = 0;
                rb.AddForce(Mathf.Lerp(0, forceMagnitude, (1 - explosionDistance)) * Random.Range(0f,1f)* explosionDir, ForceMode2D.Impulse);
            }
        }


        //if fragments exist destroy the original
        if (_fragments.Count > 0)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Turns Gameobject into multiple fragments
    /// </summary>
    private void generateFragments()
    {
        _fragments = new List<GameObject>();
        switch (shatterType)
        {
            case ShatterType.Triangle:
                _fragments = SpriteExploder.GenerateTriangularPieces(gameObject, 1, 0);
                break;
            case ShatterType.Voronoi:
                _fragments = SpriteExploder.GenerateVoronoiPieces(gameObject, 3, 0);
                break;
        }
    }
}