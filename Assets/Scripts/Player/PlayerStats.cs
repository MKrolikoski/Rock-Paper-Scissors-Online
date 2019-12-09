using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Player health points
    public HealthPoints hp;

    // Player power value
    public Stat power;

    // Player defence value
    public Stat defence;

    public delegate void OnDamageTaken(int damage);
    public OnDamageTaken onDamageTaken;


    //------------------------------------//
    //--------------METHODS---------------//
    //------------------------------------//

    private void Awake()
    {
        hp = new HealthPoints(3);
        power = new Stat(1);
        defence = new Stat(0);
    }

    void Start()
    {
        AddEventsToStats();
    }

    void Update()
    {
    }


    private void AddEventsToStats()
    {
        Player player = GetComponent<Player>();

        player.onNewRound += hp.OnNewRound;
        player.onNewRound += power.OnNewRound;
        player.onNewRound += defence.OnNewRound;
    }

    public int TakeDamage(int damage)
    {

        int hpLost = hp.LoseHp(damage, defence.getValue());
        onDamageTaken?.Invoke(hpLost);
        return hpLost;
    }
}
