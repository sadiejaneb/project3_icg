using System.Collections;
using StarterAssets;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    [System.Serializable]
    public class TreeType
    {
        public GameObject prefab;
        public int quantity;
        public float checkRadius = 1.0f; // Set to the tree's approximate radius
    }

    public TreeType[] treeTypes; // Assign in the inspector with your tree prefabs and quantities
    public Collider[] obstacleColliders; // Assign all colliders that trees should not spawn on
    public Collider playAreaCollider;
    public Vector3 spawnAreaMin;
    public Vector3 spawnAreaMax;
    public float spawnDelay = 0.1f; // Delay between spawns to prevent freezing

    private int totalTreeCount;
    private int treesSpawned;

    private int spawnCoroutineCount = 0; // To track how many coroutines have been started
    private FirstPersonController playerController;


private void Start()
{
    FirstPersonController playerController = FindObjectOfType<FirstPersonController>();
        if (playerController != null)
        {
            playerController.canMove = false; // Disable movement at start
        }

        treesSpawned = 0;
    spawnCoroutineCount = treeTypes.Length; // Set to the number of tree types we'll be spawning

    foreach (var treeType in treeTypes)
    {
        totalTreeCount += treeType.quantity;
        StartCoroutine(SpawnTrees(treeType));
    }
}

private IEnumerator SpawnTrees(TreeType treeType)
{
    int spawnedTrees = 0;

    while (spawnedTrees < treeType.quantity)
    {
        Vector3 randomPosition = GetRandomPosition(spawnAreaMin, spawnAreaMax);

        if (!playAreaCollider.bounds.Contains(randomPosition) && !IsCollidingWithObstacles(randomPosition, treeType.checkRadius))
        {
            Instantiate(treeType.prefab, randomPosition, Quaternion.identity);
            spawnedTrees++;
            treesSpawned++;
        }

        yield return new WaitForSeconds(spawnDelay);
    }

    // Decrement the counter and check if all coroutines have finished
    spawnCoroutineCount--;
    if (spawnCoroutineCount <= 0)
    {
        FinishSpawning();
    }
}

    private Vector3 GetRandomPosition(Vector3 min, Vector3 max)
    {
        return new Vector3(
            Random.Range(min.x, max.x),
            Random.Range(min.y, max.y),
            Random.Range(min.z, max.z)
        );
    }

    private bool IsCollidingWithObstacles(Vector3 position, float radius)
    {
        foreach (var collider in obstacleColliders)
        {
            // Check if the collider is null or if the GameObject is not active
            if (collider != null && collider.gameObject.activeInHierarchy)
            {
                if (collider.bounds.Intersects(new Bounds(position, Vector3.one * radius * 2)))
                {
                    return true; // The position intersects with one of the obstacle colliders
                }
            }
        }
        return false; // No intersections found, it's safe to spawn a tree here
    }
    private void FinishSpawning()
    {
        // Re-enable player movement once the trees are loaded
        FirstPersonController playerController = FindObjectOfType<FirstPersonController>(); 
        if (playerController != null)
        {
            playerController.canMove = true; // Enable movement
        }
       
    }
}
