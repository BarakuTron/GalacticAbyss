using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemPickup : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(playerTag))
        {
            PickUpItem();
        }
    }

    private void PickUpItem()
    {
        // Handle item pickup functionality here
        // For example, you could increase the player's score, health, etc.

        Destroy(gameObject); // Destroy the item after it's picked up
    }
}

