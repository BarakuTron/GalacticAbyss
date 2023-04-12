using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReceiveDamage : MonoBehaviour
{
    public float health;
    public float maxHealth;

    public static event Action OnEnemyKilled;

    void Start()
    {
        health = maxHealth;
    }

    public void DealDamage(float damage)
    {
        health -= damage;
        CheckDeath();
    }

    private void CheckOverheal()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    private void CheckDeath()
    {
        if (health <= 0)
        {
            OnEnemyKilled?.Invoke();
            Destroy(gameObject);
        }
    }
}
