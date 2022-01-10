using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMenu : MonoBehaviour
{
    public Pawn[] prefabs;

    private int p1Choice = 0;
    private int p2Choice = 1;

    public GameObject[] p1Preview;
    public GameObject[] p2Preview;

    public void ChangePlayerCharacter(int id)
    {
        if (id == 0)
        {
            p1Preview[p1Choice].SetActive(false);
            if (p1Choice == 0)
                p1Choice = 1;
            else
                p1Choice = 0;
            p1Preview[p1Choice].SetActive(true);
            GameManager.Instance.pawnPrefabs[id] = prefabs[p1Choice];
        }
        else if(id == 1)
        {
            p2Preview[p2Choice].SetActive(false);
            if (p2Choice == 0)
                p2Choice = 1;
            else
                p2Choice = 0;
            p2Preview[p2Choice].SetActive(true);
            GameManager.Instance.pawnPrefabs[id] = prefabs[p2Choice];
        }
    }
}
