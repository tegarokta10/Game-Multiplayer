using System.Collections;
using UnityEngine;

public class Spawnbintang : MonoBehaviour
{
    public GameObject objectToSpawn;   // Object to spawn
    public Vector2 xSpawnRange = new Vector2(-5f, 5f); // x-axis range for spawn position
    public float minSpawnTime = 2f;    // Minimum spawn time
    public float maxSpawnTime = 6f;    // Maximum spawn time
    public float spawnYOffset = -1f;   // Offset to spawn below collider
    public float upwardForce = 5f;     // Force applied upward to the spawned object

    private void Start()
    {
        StartCoroutine(SpawnObjectAtRandomIntervals());
    }

    private IEnumerator SpawnObjectAtRandomIntervals()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);

            // Random spawn position along the x-axis and below the collider
            Vector2 spawnPosition = new Vector2(
                Random.Range(xSpawnRange.x, xSpawnRange.y),
                transform.position.y + spawnYOffset
            );

            // Spawn the object
            GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);

            // Add Rigidbody2D if it doesn't already exist and apply an upward force
            if (!spawnedObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
            {
                rb = spawnedObject.AddComponent<Rigidbody2D>();
            }
            rb.gravityScale = 1f; // Set gravity to 0 so it doesn't fall
            rb.AddForce(Vector2.up * upwardForce, ForceMode2D.Impulse); // Apply upward force
        }
    }
}
