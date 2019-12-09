using System.Collections.Generic;

public class Stat
{
    // Base value of the stat
    protected int baseValue;
    // Changed value e.g. with abilities
    protected int currentValue;
    // List of modifiers changing value of the stat
    protected List<Modifier> modifiers; 

    #region currentValue getters\setters
    public int CurrentValue
    {
        get
        {
            int totalValue = currentValue;
            modifiers.ForEach(e => totalValue += e.modifierValue);
            return totalValue;
        }
        set
        {
            currentValue = value;
        }
    }
    #endregion


    //------------CONSTRUCTORS------------//


    public Stat(int baseValue)
    {
        modifiers = new List<Modifier>();
        this.baseValue = baseValue;
        CurrentValue = this.baseValue;
    }

    //------------------------------------//
    //--------------METHODS---------------//
    //------------------------------------//

    public void AddModifier(Modifier modifier)
    {
        if (modifier != null)
        {
            modifiers.Add(modifier);
        }
    }

    public void RemoveModifier(Modifier modifier)
    {
        modifiers.Remove(modifier);
    }

    public int getValue()
    {
        int totalValue = currentValue;
        modifiers.ForEach(e => totalValue += e.modifierValue);
        if (totalValue < 0)
            return 0;
        return totalValue;
    }

    public virtual void OnNewRound()
    {
        List<Modifier> modifiersToRemove = new List<Modifier>();
        modifiers.ForEach(m =>
        {
            if(!m.ReduceTimeLeft())
            {
                modifiersToRemove.Add(m);
            }
        });
        modifiersToRemove.ForEach(m => RemoveModifier(m));
    }
}
