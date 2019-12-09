using UnityEngine;

public class HealthPoints : Stat
{
    //--------------EVENTS----------------//

    //Event fired when hp is reduced to or below 0
    public event System.Action OnDeath;


    //------------CONSTRUCTORS------------//


    public HealthPoints(int baseValue) : base(baseValue) { }



    //------------------------------------//
    //--------------METHODS---------------//
    //------------------------------------//


    public int LoseHp(int damage, int defenceValue = 0)
    {
        damage -= defenceValue;
        damage = Mathf.Clamp(damage, 0, CurrentValue);
        CurrentValue -= damage;

        if (CurrentValue == 0)
        {
            OnDeath?.Invoke();
        }
        return damage;
    }

    public void GainHp(int hp)
    {
        hp = Mathf.Clamp(hp, 0, baseValue - CurrentValue);
        CurrentValue += hp;
    }

}
