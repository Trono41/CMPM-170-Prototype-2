using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public GameObject[] objectsToSpawn;
    public int numberOfObjects = 10;
    public float spawnAreaWidth = 20f;
    public float spawnAreaLength = 20f;
    public float spawnHeight = 10f;
    public float minDistance = 5f;

    private Vector3[] spawnedPositions;

    void Start()
    {
        SpawnObjects();
    }

    void SpawnObjects()
    {
        spawnedPositions = new Vector3[numberOfObjects];
        int spawned = 0;

        while (spawned < numberOfObjects)
        {
            Vector3 randomPos = new Vector3(
                Random.Range(-spawnAreaWidth / 2, spawnAreaWidth / 2),
                spawnHeight,
                Random.Range(-spawnAreaLength / 2, spawnAreaLength / 2)
            );

            if (IsPositionValid(randomPos, spawned))
            {
                GameObject randomObject = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];
                Quaternion randomRotation = Quaternion.Euler(0, Random.Range(-120f, -180f), 0);
                Instantiate(randomObject, randomPos, randomRotation);
                spawnedPositions[spawned] = randomPos;
                spawned++;
            }
        }
    }

    bool IsPositionValid(Vector3 position, int currentCount)
    {
        for (int i = 0; i < currentCount; i++)
        {
            if (Vector3.Distance(position, spawnedPositions[i]) < minDistance)
            {
                return false;
            }
        }
        return true;
    }
}

