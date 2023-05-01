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

    private Vector2 initialMousePos;
    public LaserSoundScript laserSound;
    public float damageMultiplier = 1;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            // Capture initial mouse position
            initialMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 myPos = transform.position;
            Vector2 direction = (initialMousePos - myPos).normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            GameObject laserShot = Instantiate(projectile, transform.position + spawnOffset, Quaternion.AngleAxis(angle, Vector3.forward));
            laserShot.GetComponent<Rigidbody2D>().velocity = direction * projectileForce;
            laserShot.GetComponent<Projectile>().damage = Random.Range(minDamage, maxDamage) * damageMultiplier;
            laserSound.PlaySound();
        }
    }
}
