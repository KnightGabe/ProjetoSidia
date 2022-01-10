using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainUI : MonoBehaviour
{
    public RectTransform p1HealthBar;
    public RectTransform p2HealthBar;

    public TextMeshProUGUI p1HealthCount;
    public TextMeshProUGUI p2HealthCount;

    public TextMeshProUGUI turnDisplay;
    public TextMeshProUGUI movesLeft;
    public TextMeshProUGUI diceAmount;    

    public void UpdateHealth(List<Pawn> pawns)
    {
        p1HealthBar.localScale = new Vector2((float)pawns[0].currentHealth / pawns[0].maxHealth, 1);
        p2HealthBar.localScale = new Vector2((float)pawns[1].currentHealth / pawns[1].maxHealth, 1);

        p1HealthCount.text = "Health: " + pawns[0].currentHealth + "/" + pawns[0].maxHealth;
        p2HealthCount.text = "Health: " + pawns[1].currentHealth + "/" + pawns[1].maxHealth;
    }

    public void SetTurnValues(int turn, int dice, int moves)
    {
        turnDisplay.text = "Player " + (turn + 1) + " turn";
        diceAmount.text = "Extra Dice: " + dice;
        movesLeft.text = "Moves Left: " + moves;
    }
}
