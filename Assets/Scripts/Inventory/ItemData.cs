/*
 * ItemData class: Represents the data for an inventory item.
 * - Contains the item's name (itemName), icon (Sprite), and count (int).
 * - Used to define different types of collectible items in the game.
 * - Can be created as ScriptableObjects in Unity for easy data management and customization.
 */



using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public int count;
    public GameObject itemPrefab;

    public bool isConsumable;
    public int healthRestoreAmount;
}

