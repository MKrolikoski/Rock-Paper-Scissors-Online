using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Items/Item")]
public class Item : ScriptableObject
{
    // Item name
    new public string name = "New Item";

    // Material of an item
    public Material itemMaterial;

    // Item that defeats this one
    public Item nemesis;



    ////------------------------------------//
    ////--------------METHODS---------------//
    ////------------------------------------//


    // Compares two items and returns one of 3 results
    // -1 -> this item lost
    // 0 -> draw
    // 1 -> this item won
    public virtual int CompareTo(Item item)
    {
        return this == item ? 0 : (nemesis == item ? -1 : 1);
    }

}
