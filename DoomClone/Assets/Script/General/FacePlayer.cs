using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    private Transform player;

    private void Start()
    {
        // Find the player object by tag or by name
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // If you prefer using the player's name, you can use:
        // player = GameObject.Find("PlayerName").transform;
    }

    private void Update()
    {
        // Calculate the direction to the player
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0f; // Lock rotation on the y-axis

        // Rotate the object to face the player while locking the up and down rotation
        transform.rotation = Quaternion.LookRotation(directionToPlayer);
    }
}