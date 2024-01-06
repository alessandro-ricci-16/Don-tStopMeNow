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
        _dz.destroyOnHit = true;
        Destroy(this);
    }
}
