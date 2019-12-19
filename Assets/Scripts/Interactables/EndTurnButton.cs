using cakeslice;
using UnityEngine;

public class EndTurnButton : Interactable
{
    public GameObject ButtonTop;

    public GameObject ButtonBody;

    private Outline[] outlinesToDisable;


    //------------------------------------//
    //--------------METHODS---------------//
    //------------------------------------//

    protected override void Awake()
    {
        base.Awake();
        canBeHoveredOver = false;
        canBeClicked = false;
        canBeHighlighted = false;
    }

    void Start()
    {
        GetComponentInParent<Player>().onCoinSelected += OnCoinSelected;
    }

    void Update()
    {

    }


    public override void OnMouseClick(Player player)
    {
        player.EndTurn();
        ResetToDefaultValues();
    }

    public override void OnMouseHover() { }

    public override void OnMouseEndHover() { }

    public override void Highlight()
    {
        ButtonTop.GetComponent<Outline>().eraseRenderer = false;
        ButtonBody.GetComponent<Outline>().eraseRenderer = false;
    }

    public override void Dehighlight()
    {
        ButtonTop.GetComponent<Outline>().eraseRenderer = true;
        ButtonBody.GetComponent<Outline>().eraseRenderer = true;
    }

    public override void ResetToDefaultValues()
    {
        base.ResetToDefaultValues();
        canBeHoveredOver = false;
        canBeClicked = false;
        canBeHighlighted = false;
        Dehighlight();
    }

    private void OnCoinSelected()
    {
        canBeHoveredOver = true;
        canBeClicked = true;
        Highlight();
    }

    public void OnNewRound()
    {
        ResetToDefaultValues();
    }
}
