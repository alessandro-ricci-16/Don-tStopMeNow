using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public bool destroyOnPlayerHit = false;
    
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
        if (other.CompareTag("Player"))
        {
            EventManager.TriggerEvent(EventNames.Death, other.transform.position);
            if(destroyOnPlayerHit)
                Destroy(gameObject);
        }
    }
}
