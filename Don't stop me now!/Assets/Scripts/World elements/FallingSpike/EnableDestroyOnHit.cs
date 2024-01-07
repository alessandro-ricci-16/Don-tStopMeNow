using System;
using UnityEngine;

public class Enable : MonoBehaviour
{
    private DeathZone _dz;
    private void Start()
    {
        _dz = GetComponent<DeathZone>();
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        // used to avoid falling spikes exploding against the ceiling
        _dz.destroyOnHit = true;
        Destroy(this);
    }
}
