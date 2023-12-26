using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    public bool destroyOnPlayerHit = false;
    
    private bool _sentEvent = false;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckDeath(other);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckDeath(collision.collider);
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
            if(destroyOnPlayerHit)
                Destroy(gameObject);
        }
    }
}
