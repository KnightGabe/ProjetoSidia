using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleUI : MonoBehaviour
{
    public Transform attackerDice;
    public Transform defenderDice;

    public GameObject dicePrefab;
    public GameObject buttonParent;

    public void QuitBattle()
    {
        GameManager.Instance.ExitBattle();
        gameObject.SetActive(false);
    }

    public void StartBattle()
    {
        GameManager.Instance.ConfirmBattle();
        ClearDice();
    }

    void ClearDice()
    {
        for (int i = attackerDice.childCount - 1; i >= 0; i--)
        {
            Destroy(attackerDice.GetChild(i).gameObject);
        }
        for (int i = defenderDice.childCount - 1; i >= 0; i--)
        {
            Destroy(defenderDice.GetChild(i).gameObject);
        }
    }

    public IEnumerator RollDice(int[] p1Rolls, int[] p2Rolls)
    {
        buttonParent.gameObject.SetActive(false);
        int mostDice = 0;
        mostDice = p1Rolls.Length;
        if (p2Rolls.Length > mostDice)
            mostDice = p2Rolls.Length;
        for (int i = 0; i < mostDice; i++)
        {
            if(p1Rolls.Length > i)
            {
                Image d1 = Instantiate(dicePrefab, attackerDice).GetComponent<Image>();
                d1.GetComponentInChildren<TextMeshProUGUI>().text = p1Rolls[i].ToString();
                d1.color = Color.green;
            }
            if(p2Rolls.Length > i)
            {
                Image d2 = Instantiate(dicePrefab, defenderDice).GetComponent<Image>();
                d2.GetComponentInChildren<TextMeshProUGUI>().text = p2Rolls[i].ToString();
                d2.color = Color.red;
            }
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(1f);
        ClearDice();
    }
}
