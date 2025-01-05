using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct Collectible
    {
        public GameObject prefab;
        public int spawnCount;
    }

    public Collectible[] collectibles;
    public Vector2 mapSize = new Vector2(500, 500);
    public LayerMask terrainLayer; 

    void Start()
    {
        SpawnAllCollectibles();
    }

    void SpawnAllCollectibles()
    {
        foreach (var collectible in collectibles)
        {
            SpawnCollectibleType(collectible.prefab, collectible.spawnCount);
        }
    }

    void SpawnCollectibleType(GameObject prefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(-mapSize.x / 2, mapSize.x / 2),
                0, // Temporary y-value for raycast start
                Random.Range(-mapSize.y / 2, mapSize.y / 2)
            );

            // Perform raycast to determine terrain height
            if (Physics.Raycast(randomPosition + Vector3.up * 100, Vector3.down, out RaycastHit hit, 200f, terrainLayer))
            {
                randomPosition.y = hit.point.y; 
                Instantiate(prefab, randomPosition, Quaternion.identity);
            }
        }
    }
}
