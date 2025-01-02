using UnityEngine;

public class CampfireInteraction : MonoBehaviour
{
    public GameObject cookedMushroomPrefab; // Assign the cooked mushroom prefab in the inspector

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the uncooked mushroom
        if (other.CompareTag("UncookedMushroom"))
        {
            // Spawn the cooked mushroom at the same position
            Instantiate(cookedMushroomPrefab, other.transform.position, Quaternion.identity);

            // Destroy the uncooked mushroom
            Destroy(other.gameObject);
        }
    }
}
