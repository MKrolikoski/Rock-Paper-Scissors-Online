using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(CoinMovement))]
public class Coin : Interactable
{
    // Item assigned to this coin
    private Item _item;

    // Material used when coin is hidden
    public Material hiddenMaterial;

    public GameObject selectedPositionPoint;

    // Point where this coin should be when not selected
    private Vector3 defaultPosition;

    // Point where this coin should be when selected
    private Vector3 selectedPosition;


    // Coin movement script
    private CoinMovement coinMovement;

    // Moves coin to selected position if changed to true, if to false moves to default pos
    private bool _selected = false;

    // Indicates if item's symbol is hidden
    private bool _hidden;

    public Item Item
    {
        get
        {
            return _item;
        }
        set
        {
            _item = value;
            if (_item != null)
            {
                if(!Hidden)
                {
                    ChangeMaterial(Item.itemMaterial);
                }
            }
        }
    }

    public bool Selected
    {
        get
        {
            return _selected;
        }
        set
        {
            _selected = value;
            if (!_selected)
            {
                if (defaultPosition != null)
                {
                    MoveToDefaultPosition();
                }
            }
            else
            {
                MoveToSelectedPosition();
            }
        }
    }

    public bool Hidden
    {
        get
        {
            return _hidden;
        }
        set
        {
            _hidden = value;
            if(_hidden)
            {
                HideSymbol();
            }
            else
            {
                ShowSymbol();
            }
        }
    }



    //------------------------------------//
    //--------------METHODS---------------//
    //------------------------------------//


    protected override void Awake()
    {
        base.Awake();
        canBeClicked = true;
        defaultPosition = transform.position;
        selectedPosition = selectedPositionPoint.transform.position;

        coinMovement = GetComponent<CoinMovement>();
    }

    void Start()
    {
        coinMovement.completePathLength = Vector3.Distance(defaultPosition.normalized, selectedPosition.normalized);
        DisableInteractability();
    }

    void Update()
    {

    }


    public override void OnMouseClick(Player player)
    {
        player.SelectCoin(this);
        DisableInteractability();
    }


    private void HideSymbol()
    {
        ChangeMaterial(hiddenMaterial);
    }

    private void ShowSymbol()
    {
        ChangeMaterial(Item.itemMaterial);
    }

    // Defaults coin's values
    public override void ResetToDefaultValues()
    {
        base.ResetToDefaultValues();
        Dehighlight();
        canBeClicked = true;
        canBeHighlighted = true;
        canBeHoveredOver = true;
        outline.color = 0;
        _selected = false;
    }


    // Changes coin material to corresponding item's material
    public void ChangeMaterial(Material material)
    {
        GetComponentInChildren<ItemSymbol>().ChangeMaterial(material);
    }

    public void ChangeOutlineColor(int color)
    {
        outline.color = color;
    }

    public override void EnableInteractability()
    {
        base.EnableInteractability();
        canBeClicked = true;
    }

    // Initiates movement to position when coin is selected
    private void MoveToSelectedPosition()
    {
        coinMovement.StartMoving(transform.position, selectedPosition);
    }

    // Initiates movement to coin's default position

    private void MoveToDefaultPosition()
    {
        coinMovement.StartMoving(transform.position, defaultPosition);
    }

    public void OnNewRound()
    {
        if(transform.position != defaultPosition)
        {
            MoveToDefaultPosition();
        }
        ResetToDefaultValues();
    }
}
