using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Item item = new Item("Item", 1);

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
//            Inventory.Instance.AddItem(item);
            Destroy(gameObject);
        }
    }
}
