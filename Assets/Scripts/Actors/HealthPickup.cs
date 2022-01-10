using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Actor
{
    public int recovery = 20;
    public override bool Interact(Actor other)
    {
        if (other is Pawn)
        {
            Pawn p = (Pawn)other;
            p.currentHealth += recovery;
            if(p.currentHealth > p.maxHealth)
                p.currentHealth = p.maxHealth;
            GameManager.Instance.mainUI.UpdateHealth(GameManager.Instance.players);

        }
        PickupManager.Instance.PickupLost(this);
        Destroy(gameObject, 0.3f);
        return true;
    }
}
