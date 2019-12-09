using UnityEngine;

public class ItemsDB : MonoBehaviour
{
    public static ItemsDB instance;


    public Item[] items;

    //------------------------------------//
    //--------------METHODS---------------//
    //------------------------------------//

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public int GetItemIndex(Item item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (item == items[i])
                return i;
        }
        return -1;
    }
}
