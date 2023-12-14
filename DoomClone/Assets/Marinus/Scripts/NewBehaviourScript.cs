using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    private Transform player;

    private void Start()
    {
        // Find the player object using the FindObjectOfType method
        player = FindObjectOfType<PlayerController>().transform;
    }

    private void Update()
    {
        // Calculate the direction to the player
        Vector3 directionToPlayer = player.position - transform.position;

        // Rotate the object to face the player using Quaternion.LookRotation
        transform.rotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);
    }
}