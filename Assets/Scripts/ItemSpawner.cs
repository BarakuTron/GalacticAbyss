using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;
    private GameObject itemInstance;
    private int enemyCount;

    void Start()
    {    
        itemInstance = GameObject.FindWithTag("Item");
        if (itemInstance != null)
        {
            itemInstance.SetActive(false);
        }
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        EnemyReceiveDamage.OnEnemyKilled += HandleEnemyKilled;
    }

    void HandleEnemyKilled()
    {
        enemyCount--;

        if (enemyCount <= 0)
        {
            SpawnItem();
        }
    }

    void SpawnItem()
    {
        if (itemInstance == null)
        {
            itemInstance = Instantiate(itemPrefab, transform.position, Quaternion.identity);
            itemInstance.SetActive(false);
        }

        if (enemyCount <= 0)
        {
            itemInstance.SetActive(true);
        }
    }

    void OnDestroy()
    {
        EnemyReceiveDamage.OnEnemyKilled -= HandleEnemyKilled;
    }
}
