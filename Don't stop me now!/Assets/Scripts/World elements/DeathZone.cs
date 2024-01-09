using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class DeathZone : MonoBehaviour
{
    private bool _sentEvent = false;
    public bool destroyOnHit = false;
    [SerializeField] private GameObject spriteSplitterPrefab;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckDeath(other);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckDeath(collision.collider);
        
        // removes this object once collided
        if (destroyOnHit)
        {
            // if set, breaks the sprite of the object with the deathzone once collided
            if (spriteSplitterPrefab is not null)
            {
                GameObject newTile = Instantiate(spriteSplitterPrefab, transform.position, Quaternion.identity);
                var tileSplitter = newTile.GetComponent<SpriteSplitter>();

                var spriteRenderer = GetComponent<SpriteRenderer>();
                tileSplitter.tileSprite = spriteRenderer.sprite;
                tileSplitter.spriteColor = spriteRenderer.color;
                tileSplitter.scale = transform.lossyScale;
                tileSplitter.initialVelocity = GetComponent<Rigidbody2D>().velocity;
            }
            gameObject.SetActive(false);
            //Destroy(gameObject); // destroys all if contained in a prefab :/
        }
    }

    private void CheckDeath(Collider2D other)
    {
        // if the player enters the death zone dies :)
        if (other.CompareTag("Player") && !_sentEvent)
        {
            _sentEvent = true;
            other.GetComponent<Explodable>().Explode();
            var position = other.transform.position;
            EventManager.TriggerEvent(EventNames.Death, SceneManager.GetActiveScene().name, position);
        }
    }
}
