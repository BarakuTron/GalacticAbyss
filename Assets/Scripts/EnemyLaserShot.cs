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
            Vector2 myPos = transform.position;
            Vector2 targetPos = player.transform.position;
            Vector2 direction = (targetPos - myPos).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            GameObject laserShot = Instantiate(projectile, transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
            laserShot.GetComponent<Rigidbody2D>().velocity = direction * projectileForce;
            laserShot.GetComponent<EnemyProjectile>().damage = Random.Range(minDamage, maxDamage);
            StartCoroutine(ShootPlayer());
        }
    }
}
