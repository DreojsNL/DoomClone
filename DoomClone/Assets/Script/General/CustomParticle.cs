using System.Collections;
using UnityEngine;

public class CustomParticle : MonoBehaviour
{
    public Material[] materials; // List of materials to cycle through
    public float cycleSpeed = 1.0f; // Speed at which to cycle through materials
    public GameObject parrent;
    private Renderer renderer;
    private int currentMaterialIndex = 0;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        if (materials.Length > 0)
        {
            // Apply the initial material
            renderer.material = materials[currentMaterialIndex];
        }

        // Start cycling through materials
        StartCoroutine(CycleMaterials());
    }

    IEnumerator CycleMaterials()
    {
        while (currentMaterialIndex < materials.Length)
        {
            // Wait for the specified time before changing materials
            yield return new WaitForSeconds(1.0f / cycleSpeed);

            // Increment the material index
            currentMaterialIndex++;

            // Apply the new material
            if (currentMaterialIndex < materials.Length)
            {
                renderer.material = materials[currentMaterialIndex];
            }
        }

        if(parrent != null)
        {
            Destroy(parrent);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
}
