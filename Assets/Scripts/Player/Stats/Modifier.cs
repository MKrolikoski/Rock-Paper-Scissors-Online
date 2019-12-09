public class Modifier
{
    //Value of modifier that affects stats
    public int modifierValue { get; private set; }
    //Time in rounds in which the modifier effect wears off
    protected int modifierTimeInRounds;
    //Left time in rounds of modifier's effect
    public int modifierLeftTimeInRounds { get; private set; }


    //------------CONSTRUCTORS------------//


    public Modifier(int modifierValue, int modifierTimeInRounds = 1)
    {
        this.modifierValue = modifierValue;
        this.modifierTimeInRounds = modifierTimeInRounds;
        modifierLeftTimeInRounds = modifierTimeInRounds;
    }




    //------------------------------------//
    //--------------METHODS---------------//
    //------------------------------------//

    public bool ReduceTimeLeft()
    {
        modifierLeftTimeInRounds--;
        if (modifierLeftTimeInRounds <= 0)
            return false;
        return true;
    }
}
