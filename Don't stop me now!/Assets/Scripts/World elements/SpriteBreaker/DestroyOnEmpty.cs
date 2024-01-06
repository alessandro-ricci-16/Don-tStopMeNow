using UnityEngine;

public class DestroyOnEmpty : MonoBehaviour
{
    private void Update()
    {
        if(transform.childCount <= 0)
            Destroy(gameObject);
    }
}
 