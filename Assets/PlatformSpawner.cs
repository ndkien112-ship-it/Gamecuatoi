using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject[] platformPrefabs;
    public GameObject goalPrefab; 
    public Transform playerTransform;

    public int maxPlatforms = 15; // Số bệ thường trước khi ra Bệ Đích
    public float spawnDistanceAhead = 12f;
    public float destroyDistanceBehind = 10f;

    public float minStepY = 1.8f;
    public float maxStepY = 2.5f;
    public float spawnRadiusX = 4f;
    public float spawnRadiusZ = 4f;

    private Vector3 lastSpawnPosition;
    private List<GameObject> activePlatforms = new List<GameObject>();
    private int spawnedCount = 0;
    private bool goalSpawned = false;

    void Start()
    {
        if (playerTransform == null) return;

        lastSpawnPosition = playerTransform.position + new Vector3(0, 1.5f, 4f);
        SpawnPlatformAt(lastSpawnPosition);
    }

    void Update()
    {
        if (playerTransform == null) return;

        // Chỉ sinh bệ thường khi CHƯA ĐỦ số lượng
        if (spawnedCount < maxPlatforms)
        {
            while (playerTransform.position.y + spawnDistanceAhead > lastSpawnPosition.y && spawnedCount < maxPlatforms)
            {
                SpawnNextPlatform();
            }
        }
        // Khi đã đủ bệ thường và CHƯA ĐẺ bệ đích -> Chỉ đẻ đúng 1 lần
        else if (!goalSpawned && playerTransform.position.y + spawnDistanceAhead > lastSpawnPosition.y)
        {
            SpawnGoal();
        }

        CleanOldPlatforms();
    }

    void SpawnNextPlatform()
    {
        float newY = lastSpawnPosition.y + Random.Range(minStepY, maxStepY);
        float minDist = 2f;
        float maxX = Mathf.Max(minDist, spawnRadiusX);
        float maxZ = Mathf.Max(minDist, spawnRadiusZ);

        float offsetX = Random.Range(minDist, maxX) * (Random.value > 0.5f ? 1 : -1);
        float offsetZ = Random.Range(minDist, maxZ) * (Random.value > 0.5f ? 1 : -1);

        Vector3 spawnPos = new Vector3(
            lastSpawnPosition.x + offsetX,
            newY,
            lastSpawnPosition.z + offsetZ
        );

        SpawnPlatformAt(spawnPos);
    }

    void SpawnPlatformAt(Vector3 pos)
    {
        int randomIndex = Random.Range(0, platformPrefabs.Length);
        GameObject newPlatform = Instantiate(platformPrefabs[randomIndex], pos, Quaternion.identity);
        activePlatforms.Add(newPlatform);
        lastSpawnPosition = pos;
        spawnedCount++;
    }

    void SpawnGoal()
    {
        if (goalPrefab == null) return;

        goalSpawned = true; // Khóa ngay lập tức để không bị lặp đẻ tràn bệ

        float newY = lastSpawnPosition.y + Random.Range(minStepY, maxStepY);
        Vector3 spawnPos = new Vector3(lastSpawnPosition.x, newY, lastSpawnPosition.z + 2f);

        GameObject goal = Instantiate(goalPrefab, spawnPos, Quaternion.identity);
        activePlatforms.Add(goal);
    }

    void CleanOldPlatforms()
    {
        for (int i = activePlatforms.Count - 1; i >= 0; i--)
        {
            if (activePlatforms[i] != null && activePlatforms[i].transform.position.y < playerTransform.position.y - destroyDistanceBehind)
            {
                Destroy(activePlatforms[i]);
                activePlatforms.RemoveAt(i);
            }
        }
    }
}