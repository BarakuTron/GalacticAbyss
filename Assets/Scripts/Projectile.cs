using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage;
    private Vector2 initialMousePos;

    void Start()
    {
        // Rotate the projectile by 90 degrees around the Z axis
        transform.Rotate(0, 0, 90);
    }

    public void Fire(Vector2 _initialMousePos, float _projectileForce)
    {
        initialMousePos = _initialMousePos;
        Vector2 myPos = transform.position;
        Vector2 direction = (_initialMousePos - myPos).normalized;

        GetComponent<Rigidbody2D>().velocity = direction * _projectileForce;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name != "Player")
        {
            if(collision.GetComponent<EnemyReceiveDamage>() != null)
            {
                collision.GetComponent<EnemyReceiveDamage>().DealDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
