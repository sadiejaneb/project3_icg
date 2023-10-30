using UnityEngine;
using UnityEngine.AI;

public class navigation_patrol : MonoBehaviour
{
    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;

    private Animator animator;
    public float patrolSpeed = 2.0f;
    public float chaseSpeed = 5.0f;
    [SerializeField]
    public float shootingRange = 20.0f;
    private Transform playerTransform;

    public static bool playerInZone = false;
    public float rotSpeed = 10f;
    public GameObject bulletPrefab; 
    public Transform shootPoint; 
    public Transform gunTransform;
    public float shootingInterval = 1f; // Time in seconds between shots

    private float lastShootTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;

        animator = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        GotoNextPoint();
    }

    void GotoNextPoint()
    {
        if (points.Length == 0)
            return;

        agent.destination = points[destPoint].position;

        int newDestPoint = 0;
        do
        {
            newDestPoint = Random.Range(0, points.Length);
        } while (destPoint == newDestPoint);

        destPoint = newDestPoint;
    }

    void Update()
    {
        if (playerInZone)
        {
            FaceTarget(playerTransform.position);  // Turn the entire NPC towards the player
            AimGunAtPlayer();  // Point the gun directly at the player
            HandlePlayerInZone();
            Shoot();
        }
        else
        {
            HandlePlayerOutOfZone();
        }
    }


    void HandlePlayerInZone()
    {
        FaceTarget(playerTransform.position); // Always face the player when in the zone

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        animator.SetBool("IsShooting", true);
        animator.SetLayerWeight(1, 1);  // Enables the shooting animation layer

        if (distanceToPlayer <= shootingRange)
        {
            StopAgent();
        }
        else
        {
            ChasePlayer();
        }
    }

    public void HandlePlayerOutOfZone()
    {
        animator.SetBool("IsShooting", false);
        animator.SetLayerWeight(1, 0);  // Disables the shooting animation layer
        animator.SetFloat("Speed", patrolSpeed);
        agent.speed = patrolSpeed; // Set NavMeshAgent speed to patrolSpeed

        InstantlyTurn(agent.destination);

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();
    }

    void StopAgent()
    {
        animator.SetFloat("Speed", 0);
        agent.isStopped = true; // Stop the agent from moving
    }

    void ChasePlayer()
    {
        animator.SetFloat("Speed", chaseSpeed);
        agent.speed = chaseSpeed; // Set NavMeshAgent speed to chaseSpeed
        agent.destination = playerTransform.position;
        agent.isStopped = false;
    }

    void FaceTarget(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotSpeed);
    }
    void AimGunAtPlayer()
    {
        Vector3 directionToPlayer = playerTransform.position - gunTransform.position;
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        gunTransform.rotation = Quaternion.Slerp(gunTransform.rotation, lookRotation, Time.deltaTime * rotSpeed);

        // Draw a debug line from the gun towards the player
        Debug.DrawLine(gunTransform.position, playerTransform.position, Color.red);
    }



    private void InstantlyTurn(Vector3 destination)
    {
        if ((destination - transform.position).magnitude < 0.1f) return;

        Vector3 direction = (destination - transform.position).normalized;
        Quaternion qDir = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, qDir, Time.deltaTime * rotSpeed);
    }
    void Shoot()
    {
        if (Time.time - lastShootTime >= shootingInterval)
        {
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            Destroy(bullet, 5f); // Optional: Destroy bullet after 5 seconds to save resources
            lastShootTime = Time.time;
        }
    }
}
