using System.Collections.Generic;
using UnityEngine;

public class SpriteSplitter : MonoBehaviour
{
    public Sprite tileSprite;
    public Color spriteColor = Color.white;

    public Vector2 forcePosition = Vector2.zero;
    public Vector3 scale = Vector3.one;
    public Vector3 initialVelocity = Vector3.zero;

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
        
        for (var i = 0; i < spriteMasks.Count; i++)
        {
            GameObject newGo = Instantiate(prefab, transform.position,
                Quaternion.identity);
            newObjects.Add(newGo);
        }
        
        UpdateSpriteObject(parent, newObjects);
        
        foreach (var obj in newObjects)
        {
            var rb = obj.GetComponent<Rigidbody2D>();
            
            rb.velocity = initialVelocity;
            if(forcePosition != Vector2.zero){
                var forceDirection = ((Vector2)transform.position - forcePosition).normalized;
                rb.velocity += forceDirection * forceSpeed;
            }
            rb.angularVelocity = Random.Range(-360, 360);
        }
        Destroy(gameObject);
    }

    private void UpdateSpriteObject(GameObject parent, IReadOnlyList<GameObject> newGo)
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
            var childSpriteRenderer = child.GetComponent<SpriteRenderer>();
            childSpriteRenderer.sprite = tileSprite;
            childSpriteRenderer.color = spriteColor;
            child.GetComponent<SpriteMask>().sprite = spriteMasks[i];
            currentGo.transform.parent = parent.transform;

            currentGo.transform.localScale = scale;
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
