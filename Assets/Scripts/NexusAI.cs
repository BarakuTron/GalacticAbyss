using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NexusAI : MonoBehaviour
{
    public Animator animator;
    private enum State { Patrol, Attack }
    private State currentState;

    public float patrolSpeed = 1f, attackSpeed = 2f, detectionRange = 5f;
    public float patrolRadius = 3f;

    private Vector2 initialPosition, target;
    private Vector2 direction;


    public float timeSinceLastAttack = 0f;
    public float attackCooldown = 2f;
    public GameObject projectile;
    public float minDamage;
    public float maxDamage;
    public float projectileForce;

    private bool isWalking = false;

    


    private void Start()
    {
        currentState = State.Patrol;
        initialPosition = transform.position;
        GetNewPatrolTarget();
    }

    private void Update()
    {
        timeSinceLastAttack += Time.deltaTime;
        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                break;
            case State.Attack:
                Attack();
                break;
        }

        if(isWalking){
           animator.SetBool("Walking", true); 
        } else {
            animator.SetBool("Walking", false);
        }
        // setting the z-value to ensure enemy is rendered correctly
        transform.position = new Vector3(transform.position.x, transform.position.y, -1f);
    }

    private void GetNewPatrolTarget()
    {
        bool validTarget = false;

        while (!validTarget)
        {
            target = (Vector2)initialPosition + Random.insideUnitCircle * patrolRadius;
            RaycastHit2D hit = Physics2D.Linecast(transform.position, target);

            if (hit.collider == null || hit.collider.GetComponent<TilemapCollider2D>() == null)
            {
                validTarget = true;
            }
        }
    }

    private void Patrol()
    {
        if (Vector2.Distance(transform.position, target) < 0.1f)
        {
            GetNewPatrolTarget();
           // animator.SetBool("Walking", false);
           isWalking = false;
        }
        else
        {
           // animator.SetBool("Walking", true);   
              isWalking = true;
        }

        direction = (target - (Vector2)transform.position).normalized;
        transform.Translate(direction * patrolSpeed * Time.deltaTime);

        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, detectionRange);
        foreach (Collider2D collider in hit)
        {
            if (collider.CompareTag("Player"))
            {
                currentState = State.Attack;
                break;
            }
        }
    }

    private void Attack()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            currentState = State.Patrol;
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distanceToPlayer > detectionRange)
        {
            currentState = State.Patrol;
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, attackSpeed * Time.deltaTime);
        
        if (timeSinceLastAttack >= attackCooldown)
        {
            Vector2 myPos = transform.position;
            Vector2 targetPos = player.transform.position;
            Vector2 direction = (targetPos - myPos).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            GameObject laserShot = Instantiate(projectile, transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
            laserShot.GetComponent<Rigidbody2D>().velocity = direction * projectileForce;
            laserShot.GetComponent<EnemyProjectile>().damage = Random.Range(minDamage, maxDamage);
            timeSinceLastAttack = 0f;
        }
    }
}