using UnityEngine;

public class Gem : Collectible
{
    private void Start()
    {
        Debug.Log("Gem instance created: " + gameObject.name);
    }
}
