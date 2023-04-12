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
        portalInstance.SetActive(true);
    }

    void OnDestroy()
    {
        EnemyReceiveDamage.OnEnemyKilled -= HandleEnemyKilled;
    }
}
