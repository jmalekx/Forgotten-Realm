using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public ItemData wood;
    public ItemData stone;
    public ItemData dagger;

    private bool hasCrafted = false;

    private void OnTriggerEnter(Collider other)
    {

        if (hasCrafted) return;

        if (other.CompareTag("Wood") && CompareTag("Stone") ||
            other.CompareTag("Stone") && CompareTag("Wood"))
        {

            hasCrafted = true;

            // Destroy the colliding objects
            Destroy(other.gameObject);
            Destroy(gameObject);

            // Instantiate a dagger
            Vector3 spawnPosition = transform.position;
            Instantiate(dagger.itemPrefab, spawnPosition, Quaternion.identity);

            // Add the crafted dagger to the inventory
            Inventory.Instance.AddItem(dagger);


            Debug.Log("Crafted a dagger!");
        }
    }
}
