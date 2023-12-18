using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScanAI : MonoBehaviour
{
    public Transform player;
    public float shootingRange = 10f;
    public LayerMask playerLayer;

    void Update()
    {
        // Check if the player is within shooting range
        if (IsPlayerInShootingRange())
        {
            // Perform hitscan when player is in shooting range
            ShootHitscan();
        }
    }

    bool IsPlayerInShootingRange()
    {
        // Check if the player is within shooting range using Physics.Raycast
        RaycastHit hit;

        if (Physics.Raycast(transform.position, player.position - transform.position, out hit, shootingRange, playerLayer))
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }

    void ShootHitscan()
    {
        // Perform hitscan logic here
        // For simplicity, let's just print a message
        Debug.Log("Enemy hitscan attack!");

        // You can add code here to damage the player or perform other actions
    }

    void OnDrawGizmosSelected()
    {
        // Draw a visual representation of the shooting range in the Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }
}
