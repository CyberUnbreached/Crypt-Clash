using UnityEngine;
using System.Collections; // <-- Add this for coroutines

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    private Player player;
    private Enemy enemy; // The current combat enemy

    private bool isPlayerTurn = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        player = FindAnyObjectByType<Player>();
    }

    public void SetCurrentEnemy(Enemy newEnemy)
    {
        enemy = newEnemy;
    }

    public void StartPlayerTurn()
    {
        isPlayerTurn = true;
        Debug.Log("=== Player's Turn ===");
        CombatManager.Instance.SetPlayerActionButtonsInteractable(true); // Enable buttons
        player.BeginTurn();
    }

    public void StartEnemyTurn()
    {
        if (enemy == null)
        {
            Debug.LogWarning("No enemy set for TurnManager!");
            return;
        }

        isPlayerTurn = false;
        Debug.Log("=== Enemy's Turn ===");
        CombatManager.Instance.SetPlayerActionButtonsInteractable(false); // Disable buttons
        enemy.BeginTurn();
    }

    public void EndTurn()
    {
        if (player == null || enemy == null)
            return;

        if (isPlayerTurn)
        {
            // If the player is dead, do not continue combat
            if (player.GetHealth() <= 0)
            {
                Debug.Log("Combat halted: Player is dead.");
                return;
            }

            StartCoroutine(DelayedEnemyTurn());
        }
        else
        {
            // If the enemy is dead, you may want to return to exploration or reward the player
            if (enemy.GetHealth() <= 0)
            {
                Debug.Log("Combat halted: Enemy is dead.");
                return;
            }

            StartCoroutine(DelayedPlayerTurn());
        }
    }

    private IEnumerator DelayedEnemyTurn()
    {
        yield return new WaitForSeconds(1f); // 1 second delay
        StartEnemyTurn();
    }

    private IEnumerator DelayedPlayerTurn()
    {
        yield return new WaitForSeconds(1f); // 1 second delay
        StartPlayerTurn();
    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }
}