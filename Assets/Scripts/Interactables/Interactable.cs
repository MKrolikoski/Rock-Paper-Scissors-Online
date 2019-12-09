using cakeslice;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public abstract class Interactable : MonoBehaviour
{
    // Indicates if object can be hovered over
    public bool canBeHoveredOver;

    // Indicated if object can be interacted clicked on
    public bool canBeClicked;
    // Highlight outline
    protected Outline outline;

    // Indicates if object can be highlighted
    protected bool canBeHighlighted;


    //------------------------------------//
    //--------------METHODS---------------//
    //------------------------------------//


    protected virtual void Awake()
    {
        outline = GetComponent<Outline>();
        canBeHoveredOver = true;
        canBeClicked = false;
        canBeHighlighted = true;
    }


    // Highlight object when mouse hovers over it
    public virtual void OnMouseHover()
    {
        if (canBeHighlighted)
        {
            Highlight();
        }
    }

    // Dehighlight object when mouse exits its radius
    public virtual void OnMouseEndHover()
    {
        Dehighlight();
    }

    public abstract void OnMouseClick(Player player);


    public virtual void Highlight()
    {
        outline.eraseRenderer = false;
    }

    public virtual void Dehighlight()
    {
        outline.eraseRenderer = true;
    }

    public virtual void ResetToDefaultValues()
    {
        canBeHoveredOver = true;
        canBeClicked = false;
        canBeHighlighted = true;
    }

    public virtual void DisableInteractability()
    {
        Dehighlight();
        canBeHoveredOver = false;
        canBeClicked = false;
        canBeHighlighted = false;
    }
    public virtual void EnableInteractability()
    {
        Dehighlight();
        canBeHoveredOver = true;
        canBeClicked = false;
        canBeHighlighted = true;
    }
}
