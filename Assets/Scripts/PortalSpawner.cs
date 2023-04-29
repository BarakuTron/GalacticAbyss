using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalSpawner : MonoBehaviour
{
    [SerializeField] private GameObject portalPrefab;
    private GameObject portalInstance;
    private int enemyCount;
    public enum PortalColor { Red, Green, Blue };
    public PortalColor portalColor;

    void Start()
    {
        portalInstance = GameObject.FindWithTag("Portal");
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
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                if (portalColor == PortalColor.Red)
                {
                    // Do nothing
                }
                else if (portalColor == PortalColor.Blue)
                {
                    // Heal the player by 50%
                    playerStats.HealCharacter(playerStats.maxHealth * 0.5f);
                }
                else if (portalColor == PortalColor.Green)
                {
                    // Heal the player by 100%
                    playerStats.HealCharacter(playerStats.maxHealth);
                }
            }

            // Load the next scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
