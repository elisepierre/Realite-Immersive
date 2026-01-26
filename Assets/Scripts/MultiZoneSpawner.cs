using UnityEngine;
using System.Collections;

public class MultiZoneSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float distanceToPlayer = 5f;
    public float spawnInterval = 3f;
    public int maxEnemies = 5;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        for (int i = 0; i < maxEnemies; i++)
        {
            float angle = i * (360f / maxEnemies);
            Vector3 spawnOffset = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad)) * distanceToPlayer;

            Vector3 playerPos = Camera.main.transform.position;
            Vector3 spawnPosition = new Vector3(playerPos.x + spawnOffset.x, 1f, playerPos.z + spawnOffset.z);

            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}