using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{

    public HealthPickup health;
    public float healthRarity = 0.7f;

    public TurnBoost boost;
    public float boostRarity = 0.3f;

    public BonusDice dice;
    public float diceRarity = 0.5f;

    private static PickupManager instance;
    public static PickupManager Instance { get { return instance; } }

    private List<Actor> activePickups = new List<Actor>();

    private void Awake()
    {
        instance = this;
    }


    public void PopulateGrid()
    {
        foreach (var tile in GridManager.Instance.myGrid.grid)
        {
            if(tile.occupant == null)
            {
                Actor a = GetRandomPickup();
                a.transform.position = tile.transform.position;
                tile.occupant = a;
                activePickups.Add(a);
            }
        }
    }

    public void PickupLost(Actor pickup)
    {
        if (activePickups.Contains(pickup))
        {
            activePickups.Remove(pickup);
            CheckForRepleshing();
        }
    }

    public void CheckForRepleshing()
    {
        while (activePickups.Count < GridManager.Instance.gridSize.magnitude * 0.1f)
        {
            Tile rand = GridManager.Instance.myGrid.GetRandomTile();
            if(rand.occupant == null)
            {
                Actor a = GetRandomPickup();
                a.transform.position = rand.transform.position;
                rand.occupant = a;
                activePickups.Add(a);
            }
        }
    }

    public Actor GetRandomPickup()
    {
        float healthChance = Random.value * healthRarity;
        float boostChance = Random.value * boostRarity;
        float diceChance = Random.value * diceRarity;
        if (healthChance >= boostChance && healthChance >= diceChance)
            return Instantiate(health);
        else if (diceChance >= boostChance)
            return Instantiate(dice);
        else
            return Instantiate(boost);
    }
}
