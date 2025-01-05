using UnityEngine;

public class MapSizeChecker : MonoBehaviour
{
    public GameObject mapBoundary;

    void Start()
    {
        if (mapBoundary.TryGetComponent<Collider>(out Collider collider))
        {
            Vector3 size = collider.bounds.size;
            Debug.Log("Map size: " + size);
        }
        else
        {
            Debug.LogError("MapBoundary does not have a Collider attached.");
        }
    }
}
