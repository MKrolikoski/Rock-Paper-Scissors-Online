using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // Layer mask for hovering over coins
    public LayerMask interactablesMask;

    // Currently hovered over coin
    private Interactable hoveredObject;

    // Hover states
    private enum HoverState { HOVERING, NOT_HOVERING };

    // Current hover state
    private HoverState hoverState = HoverState.NOT_HOVERING;



    //------------------------------------//
    //--------------METHODS---------------//
    //------------------------------------//

    void Start()
    {

    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = GetComponent<Player>().playerCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactablesMask))
        {
            // Mouse starts hovering over an object
            if (hoverState == HoverState.NOT_HOVERING)
            {
                hoveredObject = hit.collider.GetComponent<Interactable>();
                if (hoveredObject.canBeHoveredOver)
                {
                    hoveredObject.OnMouseHover();
                    hoverState = HoverState.HOVERING;
                }
            }
            // Mouse is already hovering over an object
            else
            {
                if (hoveredObject.canBeClicked)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        hoveredObject.OnMouseClick(GetComponent<Player>());
                    }
                }
            }
        }
        else
        {
            // Mouse exits object's radius
            if (hoverState == HoverState.HOVERING)
            {
                hoveredObject.OnMouseEndHover();
                hoverState = HoverState.NOT_HOVERING;
                hoveredObject = null;
            }
        }
    }
}
