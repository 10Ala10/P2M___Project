
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField]
     float minDestroyDelay = 2.0f; // The delay in seconds before the game object is destroyed
     float maxDestroyDelay = 8.0f; // The delay in seconds before the game object is destroyed

    void Start()
    {
        float destroyDelay = Random.Range(minDestroyDelay, maxDestroyDelay);
        Invoke("DestroyObject", destroyDelay);
    }

    void DestroyObject()
    {
        // Destroy the game object
        Destroy(gameObject);
    }
}