using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaserShot : MonoBehaviour
{
    private GameObject player;
    public GameObject projectile;
    public float minDamage;
    public float maxDamage;
    public float projectileForce;
    public float minCooldown;
    public float maxCooldown;

    void Start()
    {
        StartCoroutine(ShootPlayer());
        player = FindObjectOfType<PlayerMovement>().gameObject;
    }

    IEnumerator ShootPlayer()
    {
        float cooldown = Random.Range(minCooldown, maxCooldown);
        yield return new WaitForSeconds(cooldown);
        if(player != null)
        {
            GameObject laserShot = Instantiate(projectile, transform.position, Quaternion.identity);
            Vector2 myPos = transform.position;
            Vector2 targetPos = player.transform.position;
            Vector2 direction = (targetPos - myPos).normalized;
            laserShot.GetComponent<Rigidbody2D>().velocity = direction * projectileForce;
            laserShot.GetComponent<EnemyProjectile>().damage = Random.Range(minDamage, maxDamage);
            StartCoroutine(ShootPlayer());
        }
    }
}
