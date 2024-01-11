using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawn : MonoBehaviour
{
    public float despawnDelay = 3.0f; // Adjust this value as needed

    // Start is called before the first frame update
    void Start()
    {
        // Invoke the DespawnObject method after despawnDelay seconds
        Invoke("DespawnObject", despawnDelay);
    }

    // Method to despawn the object
    void DespawnObject()
    {
        // Destroy the GameObject this script is attached to
        Destroy(gameObject);
    }
}
