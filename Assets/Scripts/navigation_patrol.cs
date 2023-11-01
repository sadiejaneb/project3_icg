using UnityEngine;
using UnityEngine.AI;

public class navigation_patrol : MonoBehaviour
{
    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;

    public float patrolSpeed = 2.0f;
    public float chaseSpeed = 5.0f;
    [SerializeField]
    public float shootingRange = 20.0f;
    private Transform playerTransform;

    public static bool playerInZone = false;
    public float rotSpeed = 10f;

    [SerializeField] private float timer =5;
    private float bulletTime;
    public GameObject enemyBullet;
    public Transform spawnPoint;
    public float enemySpeed ;
    public AudioClip gunFireSound;
    private AudioSource audioSource;
    public GameObject bulletImpactEffect;  // Drag your Particle System prefab here in the Unity editor



    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        agent.autoBraking = false;

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
            //AimGunAtPlayer();  // Point the gun directly at the player
            HandlePlayerInZone();
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

        if (distanceToPlayer <= shootingRange)
        {
            StopAgent();
            Shoot();
        }
        else
        {
            ChasePlayer();
        }
    }

    public void HandlePlayerOutOfZone()
    {
        agent.speed = patrolSpeed; // Set NavMeshAgent speed to patrolSpeed
        InstantlyTurn(agent.destination);

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();
    }

    void StopAgent()
    {
        agent.isStopped = true; // Stop the agent from moving
    }

    void ChasePlayer()
    {
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


    private void InstantlyTurn(Vector3 destination)
    {
        if ((destination - transform.position).magnitude < 0.1f) return;

        Vector3 direction = (destination - transform.position).normalized;
        Quaternion qDir = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, qDir, Time.deltaTime * rotSpeed);
    }

    void Shoot()
    {
        bulletTime -= Time.deltaTime;

        if (bulletTime > 0) return;

        bulletTime = timer;

        Vector3 start = spawnPoint.position;
        Vector3 direction = (playerTransform.position - start).normalized;
        // Play the gun firing sound
        if (gunFireSound && audioSource)
        {
            audioSource.PlayOneShot(gunFireSound);
        }

        Ray shootingRay = new Ray(start, direction);
        RaycastHit hitInfo;

       
        // Instantiate the visual bullet
        GameObject bulletObj = Instantiate(enemyBullet, spawnPoint.position, Quaternion.LookRotation(direction));
        Rigidbody bulletRig = bulletObj.GetComponent<Rigidbody>();
        bulletRig.AddForce(direction * enemySpeed);
        Destroy(bulletObj, 5f);

        // Use raycasting to check if the shot will hit the player
        if (Physics.Raycast(shootingRay, out hitInfo, shootingRange))
        {
            if (hitInfo.collider.CompareTag("Player"))
            {
                // This means the ray has hit the player, apply damage or any other effect here.
                hitInfo.collider.GetComponent<PlayerHealth>().TakeDamage(1);
            }
         
            if (bulletImpactEffect != null)
            {
                GameObject impact = Instantiate(bulletImpactEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(impact, 5f);  
            }
        }
    }
    private void OnEnable()
    {
        PlayerHealth.OnPlayerDied += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDied -= HandlePlayerDeath;
    }

    void HandlePlayerDeath()
    {
        // Do something when the player dies, like showing a game over screen
    }
}
