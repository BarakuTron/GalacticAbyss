using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Linq;
using static Organism;

public class Organism : MonoBehaviour
{

    public enum Gender
    {
        Male,
        Female
    }

    public enum Team
    {
        Red,
        Green,
        Blue
    }
    public enum State
    {
        Idle,
        Social,
        Aggressive
    }



    public Gender organismGender;
    public Team organismTeam;
    public State currentState;
    private NavMeshAgent navMeshAgent;
    private float timeToChangeDestination = 3f;
    private float timeSinceLastChange = 3f;
    public float movementSpeed = 3.5f;
    public float viewDistance = 15f;
    public float fieldOfView = 120f;
    public float hearingRange = 10f;
    public float attackRange = 2f;
    public float attackDamage = 10f;
    public float health = 100f;
    public float attackCooldown = 1f;
    private float timeSinceLastAttack = 0f;
    public float desireToReproduce = 0f;
    public float reproductionThreshold = 5f;
    public float reproductionIncreaseRate = 1f;
    public float reproductionDistance = 2f;
    private bool reproducing = false;
    private Organism reproductionPartner;
    public List<Organism> sameTeamOrganisms = new List<Organism>();
    public List<Organism> otherTeamOrganisms = new List<Organism>();
    public bool showSenses = true;

    void Awake()
    {
        if (GetComponent<NavMeshAgent>() == null)
        {
            gameObject.AddComponent<NavMeshAgent>();
        }

        if (GetComponent<Rigidbody>() == null)
        {
            gameObject.AddComponent<Rigidbody>();
            GetComponent<Rigidbody>().useGravity = false;
            //GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentState = State.Idle;
        organismTeam = Team.Red;
    }

    void Update()
    {

        timeSinceLastChange += Time.deltaTime;
        switch (currentState)
        {
            case State.Idle:
                IdleState();
                break;
            case State.Social:
                SocialState();
                break;
            case State.Aggressive:
                AggressiveState();
                break;
        }
    }

    void IdleState()
    {
        Wander();
        UpdateMovementSpeed();
        Detect();
    }

    void SocialState()
    {
        desireToReproduce += reproductionIncreaseRate * Time.deltaTime;
        Detect();
        if (desireToReproduce >= reproductionThreshold && !reproducing)
        {
            //Reproduce();
        }
        Wander();
        //Flocking();
    }

    void AggressiveState()
    {
        Detect();
        FightOrFlight();
        Wander();
    }

    private void Flocking()
    {
        Vector3 separation = Vector3.zero;
        Vector3 alignment = Vector3.zero;
        Vector3 cohesion = Vector3.zero;
        int neighborsCount = 0;

        foreach (Organism organism in sameTeamOrganisms)
        {
            float distance = Vector3.Distance(transform.position, organism.transform.position);
            if (distance <= viewDistance && distance > 0)
            {
                // Separation
                Vector3 direction = (transform.position - organism.transform.position).normalized / distance;
                separation += direction;

                // Alignment
                alignment += organism.GetComponent<Rigidbody>().velocity;

                // Cohesion
                cohesion += organism.transform.position;

                neighborsCount++;
            }
        }

        if (neighborsCount > 0)
        {
            // Average and adjust forces
            separation /= neighborsCount;
            alignment /= neighborsCount;
            cohesion /= neighborsCount;

            cohesion = (cohesion - transform.position).normalized * movementSpeed;

            // Normalize and scale by movementSpeed
            separation = Vector3.ClampMagnitude(separation, movementSpeed);
            alignment = Vector3.ClampMagnitude(alignment, movementSpeed);

            // Combine forces
            Vector3 flockingDirection = separation + alignment + cohesion;

            // Update velocity
            if (!float.IsNaN(flockingDirection.x) && !float.IsNaN(flockingDirection.y) && !float.IsNaN(flockingDirection.z))
            {
                Rigidbody rb = GetComponent<Rigidbody>();
                rb.velocity += flockingDirection;
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, movementSpeed);
            }
            else
            {
                Debug.LogWarning("Invalid flocking direction calculated. Skipping velocity update.");
            }
        }
    }

    private void FightOrFlight()
    {
        int teammatesCount = sameTeamOrganisms.Count;
        int enemiesCount = otherTeamOrganisms.Count;

        if (teammatesCount +1 >= enemiesCount)
        {
            Fight();
        }
        else
        {
            Flight();
        }
    }
    private void Fight()
    {
        Detect();
        if (otherTeamOrganisms.Count == 0)
        {
            return;
        }

        // Find the closest enemy
        Organism closestEnemy = otherTeamOrganisms[0];
        float minDistance = Vector3.Distance(transform.position, closestEnemy.transform.position);

        foreach (Organism enemy in otherTeamOrganisms)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemy;
            }
        }

        // Move towards the closest enemy
        navMeshAgent.SetDestination(closestEnemy.transform.position);

        // Update the time since the last attack
        timeSinceLastAttack += Time.deltaTime;


        // Attack if within attack range and cooldown has passed
        if (minDistance <= attackRange && timeSinceLastAttack >= attackCooldown)
        {
            Organism enemyOrganism = closestEnemy.GetComponent<Organism>();
            enemyOrganism.TakeDamage(attackDamage);
            timeSinceLastAttack = 0f; // Reset the time since the last attack
            // Draw a red line between the attacker and the target
            Debug.DrawLine(transform.position, closestEnemy.transform.position, teamColor, 0.5f);
        }
        Detect();
        Wander();
    }

    private void Flight()
    {
        if (otherTeamOrganisms.Count == 0)
        {
            return;
        }

        // Calculate the average direction towards all enemies
        Vector3 directionToEnemies = Vector3.zero;
        foreach (Organism enemy in otherTeamOrganisms)
        {
            directionToEnemies += enemy.transform.position - transform.position;
        }
        directionToEnemies /= otherTeamOrganisms.Count;

        // Calculate the retreat direction (opposite of the average direction towards enemies)
        Vector3 retreatDirection = -directionToEnemies;

        // Normalize the retreat direction and set the destination for the NavMeshAgent
        retreatDirection.Normalize();
        float retreatDistance = 5f;
        Vector3 retreatDestination = transform.position + retreatDirection * retreatDistance;
        navMeshAgent.SetDestination(retreatDestination);

        // Check if the number of teammates is greater than the number of enemies
        if (sameTeamOrganisms.Count > otherTeamOrganisms.Count)
        {
            currentState = State.Idle;
        }
        Detect();
        Wander();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void Wander()
    {
        if (timeSinceLastChange >= timeToChangeDestination)
        {
            float wanderRadius = 10f;
            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
            randomDirection += transform.position;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1))
            {
                navMeshAgent.SetDestination(hit.position);
            }
            timeSinceLastChange = 0f;
            timeToChangeDestination = UnityEngine.Random.Range(2f, 5f);

        }
    }

    void UpdateMovementSpeed()
    {
        movementSpeed = navMeshAgent.speed;
    }

    private void Reproduce()
    {
        Debug.Log("Reproduce called");
        // Find suitable mates
        var potentialMates = sameTeamOrganisms
            .Where(organism => organism.desireToReproduce >= reproductionThreshold &&
                           organism.organismGender != organismGender &&
                           !organism.reproducing)
            .ToList();

        if (potentialMates.Count > 0)
        {
            // Choose the closest mate
            Organism closestMate = potentialMates.OrderBy(mate => Vector3.Distance(transform.position, mate.transform.position)).First();
            reproductionPartner = closestMate;
            reproducing = true;
            reproductionPartner.reproducing = true;

            // Move to the middle point between the organisms
            Vector3 meetingPoint = (transform.position + reproductionPartner.transform.position) / 2;
            navMeshAgent.SetDestination(meetingPoint);
            reproductionPartner.navMeshAgent.SetDestination(meetingPoint);

            // Check if the organisms are close enough to reproduce
            if (Vector3.Distance(transform.position, reproductionPartner.transform.position) <= reproductionDistance)
            {
                // Create offspring
                GameObject offspring = Instantiate(gameObject, meetingPoint, Quaternion.identity);
                offspring.GetComponent<Organism>().organismGender = (Gender)Random.Range(0, 2);
                offspring.GetComponent<Organism>().organismTeam = organismTeam;
                offspring.name = offspring.GetComponent<Organism>().organismTeam.ToString() + " " + offspring.GetComponent<Organism>().organismGender.ToString();

                // Reset desire to reproduce
                desireToReproduce = 0f;
                reproductionPartner.desireToReproduce = 0f;
                reproducing = false;
                reproductionPartner.reproducing = false;
            }
        }
    }


    void Detect()
    {
        sameTeamOrganisms.Clear();
        otherTeamOrganisms.Clear();
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, viewDistance);

        foreach (Collider obj in objectsInRange)
        {
            Organism detectedOrganism = obj.GetComponent<Organism>();
            if (detectedOrganism != null && detectedOrganism != this)
            {
                Vector3 directionToObject = (obj.transform.position - transform.position).normalized;
                float angleToObject = Vector3.Angle(transform.forward, directionToObject);
                float distanceToObject = Vector3.Distance(transform.position, obj.transform.position);

                if ((angleToObject <= fieldOfView * 0.5f && distanceToObject <= viewDistance) || distanceToObject <= hearingRange)
                {
                    //Debug.Log("Detected organism: " + obj.name);
                    if (detectedOrganism.organismTeam == this.organismTeam)
                    {
                        sameTeamOrganisms.Add(detectedOrganism);
                        currentState = State.Social;
                    }
                    else
                    {
                        otherTeamOrganisms.Add(detectedOrganism);
                        currentState = State.Aggressive;
                    }
                }
            }
        }
    }

    public Color teamColor
    {
        get
        {
            switch (organismTeam)
            {
                case Team.Red:
                    return Color.red;
                case Team.Green:
                    return Color.green;
                case Team.Blue:
                    return Color.blue;
                default:
                    return Color.white;
            }
        }
    }


    void OnDrawGizmos()
    {
        if (showSenses)
        {
            // Draw combined area of field of view and view distance
            Gizmos.color = Color.yellow;

            // Draw field of view
            Vector3 leftFOV = Quaternion.Euler(0, -fieldOfView * 0.5f, 0) * transform.forward;
            Vector3 rightFOV = Quaternion.Euler(0, fieldOfView * 0.5f, 0) * transform.forward;

            int segments = 30;
            for (int i = 0; i <= segments; i++)
            {
                float angle = Mathf.Lerp(-fieldOfView * 0.5f, fieldOfView * 0.5f, i / (float)segments);
                Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;
                Gizmos.DrawLine(transform.position, transform.position + direction * viewDistance);
            }

            // Draw hearing range
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, hearingRange);
        }
    }
}