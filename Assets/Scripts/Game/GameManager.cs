using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Pawn[] pawnPrefabs;

    public List<Pawn> players = new List<Pawn>();

    public int currentTurn = 0;
    public static GameManager Instance { get { return instance; } }
    private static GameManager instance;

    private bool waitingForPlayer = false;
    private bool battleConfirmation = false;

    private bool gameOver = false;

    public BattleUI battleUI;
    public MainUI mainUI;

    public GameObject gameOverScreen;
    public TextMeshProUGUI gameOverText;

    private void Awake()
    {
        instance = this;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Application.Quit();
    }


    public void InitializeGame()
    {
        //GridManager.Instance.CreateGrid();
        SpawnPawns();
        PickupManager.Instance.PopulateGrid();
        mainUI.gameObject.SetActive(true);
    }

    public void SpawnPawns()
    {
        for (int i = 0; i < 2; i++)
        {
            Tile startingTile = GridManager.Instance.GetStartingTile(i);
            players.Add(Instantiate(pawnPrefabs[i], startingTile.transform.position, pawnPrefabs[i].transform.rotation));
            startingTile.occupant = players[i];
            players[i].currentTile = startingTile;
        }
        mainUI.UpdateHealth(players);
        StartCoroutine(TurnBehaviour());
    }

    public void ValidTileSelected(Tile tile)
    {
        players[currentTurn].onFinishedMove += EndPlayerMove;
        players[currentTurn].Move(tile);
    }

    void EndPlayerMove()
    {
        players[currentTurn].onFinishedMove -= EndPlayerMove;
        waitingForPlayer = false;
    }

    public IEnumerator TurnBehaviour()
    {
        while (!gameOver)
        {
            for (int i = 0; i < players.Count; i++)
            {
                currentTurn = i;
                players[currentTurn].playerCamera.ToggleCamera(1);
                for (int j = 0; j < players[currentTurn].movesPerTurn; j++)
                {
                    mainUI.SetTurnValues(currentTurn, players[currentTurn].bonusDice, players[currentTurn].movesPerTurn - j);
                    GridManager.Instance.HighlightNeighbours(players[currentTurn].currentTile);
                    waitingForPlayer = true;
                    yield return new WaitUntil(() => !waitingForPlayer);
                    yield return new WaitForSeconds(0.4f);
                }
                players[currentTurn].movesPerTurn = 3;
                players[currentTurn].bonusDice = 0;
            }
        }
    }

    public void CheckForVictor()
    {
        for (int i = players.Count - 1; i >= 0; i--)
        {
            if(players[i].currentHealth <= 0)
            {
                if (i == 0)
                    EndGame(1);
                else 
                    EndGame(0);
                return;
            }
        }                           
    }

    public void EndGame(int winningPlayer)
    {
        gameOverScreen.SetActive(true);
        gameOverText.text = "Player " + (winningPlayer + 1) + " Won!";
    }

    public void ConfirmBattle()
    {
        battleConfirmation = true;
    }

    public void ExitBattle()
    {
        battleUI.gameObject.SetActive(false);
        players[currentTurn].playerCamera.ToggleCamera(1);
        StopCoroutine("StartBattle");
        GridManager.Instance.HighlightNeighbours(players[currentTurn].currentTile);
    }

    public IEnumerator StartBattle(Pawn attacker, Pawn defender)
    {
        battleUI.gameObject.SetActive(true);
        battleUI.buttonParent.gameObject.SetActive(true);
        defender.StartCoroutine(defender.RotateAnimation(attacker.transform.position));
        yield return new WaitForSeconds(0.3f);
        attacker.battleCamera.ToggleCamera(1);
        yield return new WaitUntil(() => battleConfirmation);
        int wins = 0;
        List<int> attackerRolls = RollDice(3 + attacker.bonusDice);
        List<int> defenderRolls = RollDice(3);
        for (int i = 0; i < 3; i++)
        {
            Debug.Log(attacker.name + " rolled " + attackerRolls[i] + " to attack!");
            Debug.Log(defender.name + " rolled " + defenderRolls[i] + " to defend!");
            if (attackerRolls[i] >= defenderRolls[i])
                wins++;
        }
        if (currentTurn == 0)
        {
            yield return StartCoroutine(battleUI.RollDice(attackerRolls.ToArray(), defenderRolls.ToArray()));
        }
        else
        {
            yield return StartCoroutine(battleUI.RollDice(defenderRolls.ToArray(), attackerRolls.ToArray()));
        }
        if (wins > 1)
        {
            Debug.Log("Attacker wins!");
            attacker.anim.SetTrigger("Attack");
            yield return new WaitForSeconds(1);
            defender.TakeDamage(attacker.damage);
        }
        else
        {
            Debug.Log("Defender wins!");
            defender.anim.SetTrigger("Attack");
            yield return new WaitForSeconds(1);
            attacker.TakeDamage(defender.damage);
        }
        mainUI.UpdateHealth(players);
        yield return new WaitForSeconds(1);
        attacker.playerCamera.ToggleCamera(1);
        battleConfirmation = false;
        battleUI.gameObject.SetActive(false);
        EndPlayerMove();
        CheckForVictor();
    }

    public List<int> RollDice(int die)
    {
        List<int> rolls = new List<int>();
        for (int i = 0; i < die; i++)
        {
            rolls.Add(Random.Range(1, 7));
            Debug.Log(rolls[i]);
        }
        rolls.Sort();
        List<int> inv = new List<int>();
        for (int i = rolls.Count - 1; i >= 0; i--)
        {
            inv.Add(rolls[i]);
        }
        return inv;
    }
}
