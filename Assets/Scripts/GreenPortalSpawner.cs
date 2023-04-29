using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GreenPortalSpawner : MonoBehaviour
{
    [SerializeField] private GameObject portalPrefab;
    private GameObject portalInstance;
    private int enemyCount;

    void Start()
    {
        portalInstance = GameObject.FindWithTag("GreenPortal");
        if (portalInstance != null)
        {
            portalInstance.SetActive(false);
        }
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        EnemyReceiveDamage.OnEnemyKilled += HandleEnemyKilled;
    }

    void HandleEnemyKilled()
    {
        enemyCount--;

        if (enemyCount <= 0)
        {
            SpawnPortal();
        }
    }

    void SpawnPortal()
    {
        if (portalInstance == null)
        {
            portalInstance = Instantiate(portalPrefab, transform.position, Quaternion.identity);
            portalInstance.SetActive(false);
        }
        
        if (enemyCount <= 0)
        {
            portalInstance.SetActive(true);
        }
    }

    void OnDestroy()
    {
        EnemyReceiveDamage.OnEnemyKilled -= HandleEnemyKilled;
    }

    // This function will be called when the player touches the portal
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats playerStats = PlayerStats.playerStats;
            
            if(playerStats != null)
            {
                playerStats.AddScore(10);
                // Heal the player by 100%
                playerStats.HealCharacter(playerStats.maxHealth);
            }

            // Load the next scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
