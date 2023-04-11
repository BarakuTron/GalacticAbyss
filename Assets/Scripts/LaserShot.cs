using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShot : MonoBehaviour
{
    public GameObject projectile;
    public float minDamage;
    public float maxDamage;
    public float projectileForce;

    public Vector3 spawnOffset = new Vector3(0, 0.5f, 0);

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 myPos = transform.position;
            Vector2 direction = (mousePos - myPos).normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            GameObject laserShot = Instantiate(projectile, transform.position + spawnOffset, Quaternion.AngleAxis(angle, Vector3.forward));
            laserShot.GetComponent<Rigidbody2D>().velocity = direction * projectileForce;
            laserShot.GetComponent<Projectile>().damage = Random.Range(minDamage, maxDamage);
        }
    }
}
