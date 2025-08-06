using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int attack = 5;
    [SerializeField] private int maxHealth = 20;
    [SerializeField] private int maxMana = 15;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip attackClip;
    [SerializeField] private AudioClip takeDamageClip;
    [SerializeField] private AudioClip fireballClip;

    private int health;
    private int mana;
    private bool inCombat;
    private bool isDead = false;


    public GameObject freeLookCamera;

    private void Start()
    {
        inCombat = false;
        health = maxHealth;
        mana = maxMana;
    }

    private void Update()
    {
        if (!TurnManager.Instance.IsPlayerTurn() || inCombat)
            return;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !inCombat)
        {
            Debug.Log("Enemy encountered! Initiating combat...");
            inCombat = true;

            // Disable player movement
            InputManager.Instance.DisablePlayerInput();
            DisableFreeLookCamera();

            // Trigger combat start
            CombatManager.Instance.StartCombat(this, other.GetComponent<Enemy>());

            // Lock camera to face enemy
            GetComponent<PlayerController>().LookAtTarget(other.transform.position);
        }
        
        
        else if (other.CompareTag("Chest"))
        {
            Debug.Log("Chest reached! Level complete. Transitioning to Success scene...");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadSceneAsync("Success");
        }
    }



    public void UnlockMovement()
    {
        inCombat = false;
        InputManager.Instance.EnablePlayerInput();
        EnableFreeLookCamera();
        GetComponent<PlayerController>().SetCanMove(true);
        Debug.Log("Player movement unlocked.");
    }


    public void BeginTurn()
    {
        Debug.Log("Player's turn begins.");
        // Show attack/magic UI options here
    }

    public void EndTurn()
    {
        Debug.Log("Player's turn ends.");
        TurnManager.Instance.EndTurn();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (takeDamageClip != null && audioSource != null)
            audioSource.PlayOneShot(takeDamageClip);

        if (health < 0)
        {
            health = 0;
        }

        Debug.Log($"Player takes {damage} damage. Health: {health}/{maxHealth}");
        CheckIfAlive();
    }

    public void Attack(Enemy target)
    {
        if (!TurnManager.Instance.IsPlayerTurn())
            return;

        if (attackClip != null && audioSource != null)
            audioSource.PlayOneShot(attackClip);

        Debug.Log($"Player attacks {target.name} for {attack} damage.");
        target.TakeDamage(attack);
        EndTurn();
    }

    public void CastFireball(Enemy target)
    {
        int cost = 5;
        int damage = attack * 2;

        if (mana >= cost)
        {
            mana -= cost;
            if (fireballClip != null && audioSource != null)
                audioSource.PlayOneShot(fireballClip);
            target.TakeDamage(damage);
            Debug.Log($"Player casts Fireball on {target.name} for {damage} damage. Mana: {mana}/{maxMana}");
        }
        else
        {
            Debug.Log("Not enough mana!");
        }

        EndTurn();
    }

    private void CheckIfAlive()
    {
        if (health <= 0 && !isDead)
        {
            isDead = true;
            Debug.Log("Player has died.");

            // Stop combat and go to Game Over screen
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Start coroutine for delayed scene transition
            StartCoroutine(TransitionToGameOver());
        }
    }

    private System.Collections.IEnumerator TransitionToGameOver()
    {
        yield return new WaitForSeconds(2f); // 2 second pause
        SceneManager.LoadSceneAsync("Game Over");
    }
    
    public void DisableFreeLookCamera()
    {
        if (freeLookCamera != null)
        {
            freeLookCamera.SetActive(false);
            Debug.Log("FreeLook camera disabled.");
        }
        else
        {
            Debug.LogWarning("FreeLook camera reference is missing.");
        }
    }

    public void EnableFreeLookCamera()
    {
        if (freeLookCamera != null)
        {
            freeLookCamera.SetActive(true);
            Debug.Log("FreeLook camera enabled.");
        }
        else
        {
            Debug.LogWarning("FreeLook camera reference is missing.");
        }
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public int GetMana()
    {
        return mana;
    }

    public int GetMaxMana()
    {
        return maxMana;
    }

}
