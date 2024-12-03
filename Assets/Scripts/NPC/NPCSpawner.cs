using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab;
    public Vector3 spawnCenter; 
    public float spawnRadius; 
    public int spawnCount = 10;
    public float minY = -5f;  //min Y value for spawn height
    public float maxY = 5f;

    private void Start()
    {
        SpawnPrefabs();
    }

    private void SpawnPrefabs()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            //random position within the spawn radius (including Y range)
            Vector3 randomPosition = spawnCenter + (Random.insideUnitSphere * spawnRadius);
            randomPosition.y = Random.Range(minY, maxY);  //randomize Y within the specified range

            Instantiate(npcPrefab, randomPosition, Quaternion.identity);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green; 
        Gizmos.DrawWireSphere(spawnCenter, spawnRadius); 
    }
}
