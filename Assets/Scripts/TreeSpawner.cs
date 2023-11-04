using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    [System.Serializable]
    public class TreeType
    {
        public GameObject prefab;
        public int quantity;
    }

    public TreeType[] treeTypes; // Assign in the inspector with your tree prefabs and quantities
    public Collider playAreaCollider;
    public Vector3 spawnAreaMin;
    public Vector3 spawnAreaMax;

    private void Start()
    {
        foreach (var treeType in treeTypes)
        {
            SpawnTrees(treeType.prefab, treeType.quantity);
        }
    }

    void SpawnTrees(GameObject treePrefab, int quantity)
    {
        int spawnedTrees = 0;
        float checkRadius = 1.0f; // Change this to match the approximate radius of your trees

        while (spawnedTrees < quantity)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y),
                Random.Range(spawnAreaMin.z, spawnAreaMax.z)
            );

            // Check if the position is inside the play area or intersecting other colliders
            if (!playAreaCollider.bounds.Contains(randomPosition) && !IsCollidingWithObjects(randomPosition, checkRadius))
            {
                Instantiate(treePrefab, randomPosition, Quaternion.identity);
                spawnedTrees++;
            }
        }
    }

    bool IsCollidingWithObjects(Vector3 position, float radius)
    {
        // Perform a sphere check at the position for any colliders.
        // Note: You might want to exclude the tree layer or any other specific layers from this check.
        Collider[] hitColliders = Physics.OverlapSphere(position, radius);

        if (hitColliders.Length > 0)
        {
            // Optionally, you can further inspect the hitColliders array to take specific actions
            // depending on what objects are colliding.
            return true; // We've hit something, so this position is not good for spawning a tree
        }
        return false; // No collisions, it's safe to spawn a tree here
    }
}
