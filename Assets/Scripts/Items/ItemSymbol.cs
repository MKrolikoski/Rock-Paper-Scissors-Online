using UnityEngine;

public class ItemSymbol : MonoBehaviour
{


    //------------------------------------//
    //--------------METHODS---------------//
    //------------------------------------//


    public void ChangeMaterial(Material material)
    {
        GetComponent<Renderer>().material = material;
    }
}
