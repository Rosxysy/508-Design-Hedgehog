using UnityEngine;

public class ScranPickup : MonoBehaviour
{



    void Start()
    {
        
    }


    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player") & CompareTag("Scran"))
        {
            Debug.Log("Scran picked up by player!");
            Destroy(gameObject); // Remove scran from the ground
        }
}
}