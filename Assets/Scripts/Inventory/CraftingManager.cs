using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public ItemData wood;
    public ItemData stone;
    public ItemData dagger;

    private bool hasCrafted = false;

    private void OnTriggerEnter(Collider other)
    {
        // Ensure we only craft once
        if (hasCrafted) return;

        // Check if the collider is wood or stone
        if (other.CompareTag("Wood") || other.CompareTag("Stone"))
        {
            // Check if we have both items present
            if (HasBothItemsInTrigger())
            {
                CraftDagger();
            }
        }
    }

    private void CraftDagger()
    {
        hasCrafted = true;

        // Destroy all wood and stone objects in the vicinity
        DestroyGameObjectsWithTag("Wood");
        DestroyGameObjectsWithTag("Stone");

        // Instantiatse a dagger at the position of the crafting manager
        Vector3 spawnPosition = transform.position;
        Instantiate(dagger.itemPrefab, spawnPosition, Quaternion.identity);

        Debug.Log("Crafted a dagger!");
    }

    private bool HasBothItemsInTrigger()
    {
        // Use Physics.OverlapSphere to check for both items in the trigger area
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f); // Adjust radius as needed
        bool hasWood = false;
        bool hasStone = false;

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Wood"))
            {
                hasWood = true;
                // If we found both, we can exit early
                if (hasStone) break;
            }
            else if (collider.CompareTag("Stone"))
            {
                hasStone = true;
                // If we found both, we can exit early
                if (hasWood) break;
            }
        }

        return hasWood && hasStone;
    }

    private void DestroyGameObjectsWithTag(string tag)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f); // Adjust radius as needed
        foreach (var collider in colliders)
        {
            if (collider.CompareTag(tag))
            {
                Destroy(collider.gameObject);
            }
        }
    }
}
