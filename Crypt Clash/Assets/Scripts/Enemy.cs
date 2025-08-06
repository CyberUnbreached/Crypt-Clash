using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField] private string enemyName = "Goblin";
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private int attackPower = 3;

    private int currentHealth;
    public string EnemyName => enemyName;
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    public int AttackPower => attackPower;

    private void Start()
    {
        currentHealth = maxHealth;
        Debug.Log($"{enemyName} initialized with {attackPower} attack power.");
    }

    public void LookAtPlayer(Player player)
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0; // Keep only horizontal rotation
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);
    }

    public void BeginTurn()
    {
        Debug.Log($"{enemyName} with {attackPower} ATK is taking their turn.");
        if (currentHealth <= 0)
        {
            Debug.Log($"{enemyName} is already dead. Skipping turn.");
            return;
        }

        Debug.Log($"{enemyName}'s turn begins!");

        Player player = FindAnyObjectByType<Player>();
        Attack(player);

        EndTurn();
    }


    public void EndTurn()
    {

        Debug.Log($"{enemyName}'s turn ends.");
        TurnManager.Instance.EndTurn();
    }


    public void Attack(Player player)
    {
        Debug.Log($"{enemyName} attacks Player for {attackPower} damage!");
        StartCoroutine(AttackLunge());
        player.TakeDamage(attackPower);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{enemyName} takes {damage} damage. HP: {currentHealth}/{maxHealth}");

        CombatManager.Instance.UpdateEnemyHealthUI(); // <--- Add this line

        if (currentHealth <= 0)
        {
            Die();
        }
    }


    private void Die()
    {
        Debug.Log($"{enemyName} has been defeated!");
        CombatManager.Instance.EndCombat(FindAnyObjectByType<Player>());
        Destroy(gameObject);
    }

    public int GetHealth()
    {
        return CurrentHealth;
    }

    private IEnumerator AttackLunge()
    {
        Vector3 originalPosition = transform.position;
        Vector3 forwardPosition = originalPosition + transform.forward * 1.0f; // Adjust distance as needed

        float lungeTime = 0.15f;
        float elapsed = 0f;

        // Move forward
        while (elapsed < lungeTime)
        {
            transform.position = Vector3.Lerp(originalPosition, forwardPosition, elapsed / lungeTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = forwardPosition;

        // Brief pause at the front
        yield return new WaitForSeconds(0.05f);

        // Move back
        elapsed = 0f;
        while (elapsed < lungeTime)
        {
            transform.position = Vector3.Lerp(forwardPosition, originalPosition, elapsed / lungeTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition;
    }
}
