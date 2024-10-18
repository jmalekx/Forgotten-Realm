using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

//some comments on the benefit of system.serializable
/*
 * In Unity, marking a class as [System.Serializable] allows instances of that class 
 * to be displayed and edited in the Unity Inspector.
For example, if you have a List<Item> in another script, each Item in that list will 
appear in the Inspector, allowing you to easily modify their properties directly within the Unity Editor.
 */

public class Item
{
    public string name;
    public int count; //represents the number of items the player is picking up
    public GameObject itemPrefab; //// prefab to instantiate when dropping the item

    public Item(string itemName, int itemCount, GameObject prefab)
    {
        name = itemName;
        count = itemCount;
        itemPrefab = prefab;
    }
}
