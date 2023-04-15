using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSpawner : MonoBehaviour
{
    [SerializeField] private GameObject portalPrefab;
    private GameObject portalInstance;
    private int enemyCount;

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
}
