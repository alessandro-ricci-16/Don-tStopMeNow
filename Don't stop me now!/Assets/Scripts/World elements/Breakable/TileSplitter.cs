using System.Collections.Generic;
using UnityEngine;

public class TileSplitter : MonoBehaviour
{
    public Sprite tileSprite;

    public Vector2 forcePosition;

    public float forceSpeed;
    public PhysicsMaterial2D physicsMaterial;

    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject parentPrefab;
    [SerializeField] private List<Sprite> spriteMasks;

    private void Start()
    {
        BreakSpriteToMasks();
    }

    private void BreakSpriteToMasks()
    {
        List<GameObject> newObjects = new List<GameObject>(spriteMasks.Count);
        GameObject parent = Instantiate(parentPrefab, transform.position, Quaternion.identity);
        
        for (int i = 0; i < spriteMasks.Count; i++)
        {
            GameObject newGo = Instantiate(prefab, transform.position + new Vector3(0.5f, 0.5f, 0),
                Quaternion.identity);
            newObjects.Add(newGo);
        }
        
        UpdateSpriteObject(parent, newObjects, spriteMasks, tileSprite, physicsMaterial);
        
        foreach (var obj in newObjects)
        {
            var rb = obj.GetComponent<Rigidbody2D>();
            Vector3 forceDirection = transform.position - (Vector3)forcePosition;
            
            rb.velocity = forceDirection * forceSpeed;
            rb.angularVelocity = Random.Range(-360, 360);
        }
        Destroy(gameObject);
    }

    private void UpdateSpriteObject(GameObject parent, IReadOnlyList<GameObject> newGo, IReadOnlyList<Sprite> spriteMasks, Sprite tileSprite, PhysicsMaterial2D physicsMaterial)
    {
        for (var i = 0; i < newGo.Count; i++)
        {
            var currentGo = newGo[i];
            
            currentGo.GetComponent<Rigidbody2D>().sharedMaterial = physicsMaterial;
            currentGo.GetComponent<SpriteRenderer>().sprite = spriteMasks[i];
            PolygonCollider2D polygonCollider = currentGo.AddComponent<PolygonCollider2D>();
            polygonCollider.sharedMaterial = physicsMaterial;
            
            UpdateShapeToSprite(polygonCollider);

            var child = currentGo.transform.GetChild(0);
            child.GetComponent<SpriteRenderer>().sprite = tileSprite;
            child.GetComponent<SpriteMask>().sprite = spriteMasks[i];
            currentGo.transform.parent = parent.transform;

            currentGo.transform.localScale *= 0.95f;
        }        
    }

    private void UpdateShapeToSprite(PolygonCollider2D coll)
    {
        Sprite sprite = coll.GetComponent<SpriteRenderer>().sprite;

        if (coll != null && sprite != null)
        {
            coll.pathCount = sprite.GetPhysicsShapeCount();
            List<Vector2> path = new List<Vector2>();

            for (var i = 0; i < coll.pathCount; i++)
            {
                path.Clear();
                sprite.GetPhysicsShape(i, path);
                coll.SetPath(i, path.ToArray());
            }
        }
    }
}
