using UnityEngine;

public class DeathZone : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if the player enters the death zone dies :)
        if(other.CompareTag("Player"))
            GameManager.Instance.Die(other.transform.position);
    }
}
