using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance { get; private set; }

    [Header("UI")]
    public GameObject combatUIPanel;
    public Button attackButton;
    public Button fireballButton;

    private Player player;
    private Enemy currentEnemy;
    public GameObject combatUI;
    public TMP_Text enemyNameText;
    public TMP_Text enemyHealthText;
    


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void StartCombat(Player player, Enemy enemy)
    {
        Debug.Log("Combat Started!");
        Debug.Log($"Combat Started with {enemy.EnemyName}, ATK: {enemy.AttackPower}");
        currentEnemy = enemy;
        this.player = player;

        enemy.LookAtPlayer(player);

        TurnManager.Instance.SetCurrentEnemy(enemy);
        TurnManager.Instance.StartPlayerTurn();

        // Show combat UI
        combatUI.SetActive(true);

        if (enemyNameText != null)
            enemyNameText.text = $"ENCOUNTERED {enemy.EnemyName.ToUpper()}";

        UpdateEnemyHealthUI();

        ShowCombatUI();
    }

    public void EndCombat(Player player)
    {
        Debug.Log("Combat Ended!");

        // Hide combat UI and re-enable movement
        HideCombatUI();
        player.UnlockMovement();

        // Clear stored enemy/player to avoid future references
        currentEnemy = null;
        this.player = null;

        // Remove button listeners
        attackButton.onClick.RemoveAllListeners();
        fireballButton.onClick.RemoveAllListeners();
    }

    private void ShowCombatUI()
    {
        combatUIPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        attackButton.onClick.RemoveAllListeners();
        fireballButton.onClick.RemoveAllListeners();

        attackButton.onClick.AddListener(() => {
            SetPlayerActionButtonsInteractable(false); // Disable buttons immediately
            player.Attack(currentEnemy);
        });
        fireballButton.onClick.AddListener(() => {
            SetPlayerActionButtonsInteractable(false); // Disable buttons immediately
            player.CastFireball(currentEnemy);
        });
    }

    private void HideCombatUI()
    {
        combatUIPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UpdateEnemyHealthUI()
    {
        if (enemyHealthText != null && currentEnemy != null)
            enemyHealthText.text = $"Enemy Health: {currentEnemy.CurrentHealth}/{currentEnemy.MaxHealth}";
    }

    public void SetPlayerActionButtonsInteractable(bool interactable)
    {
        attackButton.interactable = interactable;
        fireballButton.interactable = interactable;
    }
}


