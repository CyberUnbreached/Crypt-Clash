using UnityEngine;

public class StatPickup : MonoBehaviour
{
    public enum StatType { MaxHealth, MaxMana, Attack }
    public enum PickupMode { Increase, Refill }

    public StatType statToAffect;
    public PickupMode pickupMode = PickupMode.Increase;
    public int amount = 5;

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            switch (statToAffect)
            {
                case StatType.MaxHealth:
                    if (pickupMode == PickupMode.Increase)
                        IncreaseMaxHealth(player);
                    else
                        RefillHealth(player);
                    break;
                case StatType.MaxMana:
                    if (pickupMode == PickupMode.Increase)
                        IncreaseMaxMana(player);
                    else
                        RefillMana(player);
                    break;
                case StatType.Attack:
                    if (pickupMode == PickupMode.Increase)
                        IncreaseAttack(player);
                    // No refill for attack stat
                    break;
            }

            // Optionally, play a sound or effect here

            Destroy(gameObject); // Remove pickup after use
        }
    }

    private void IncreaseMaxHealth(Player player)
    {
        var field = typeof(Player).GetField("maxHealth", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var healthField = typeof(Player).GetField("health", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (field != null)
        {
            int newMax = (int)field.GetValue(player) + amount;
            Debug.Log($"Increasing max health from {(int)field.GetValue(player)} to {newMax}");
            field.SetValue(player, newMax);
            // Also increase current health by the same amount
            if (healthField != null)
            {
                int newHealth = (int)healthField.GetValue(player) + amount;
                healthField.SetValue(player, newHealth);
                Debug.Log($"Increasing current health to {newHealth}");
            }
        }
    }

    private void RefillHealth(Player player)
    {
        var field = typeof(Player).GetField("health", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var maxField = typeof(Player).GetField("maxHealth", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (field != null && maxField != null)
        {
            int max = (int)maxField.GetValue(player);
            int newHealth = Mathf.Min((int)field.GetValue(player) + amount, max);
            Debug.Log($"Refilling health to {newHealth}");
            field.SetValue(player, newHealth);
        }
    }

    private void IncreaseMaxMana(Player player)
    {
        var field = typeof(Player).GetField("maxMana", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var manaField = typeof(Player).GetField("mana", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (field != null)
        {
            int newMax = (int)field.GetValue(player) + amount;
            Debug.Log($"Increasing max mana from {(int)field.GetValue(player)} to {newMax}");
            field.SetValue(player, newMax);
            // Also increase current mana by the same amount
            if (manaField != null)
            {
                int newMana = (int)manaField.GetValue(player) + amount;
                manaField.SetValue(player, newMana);
                Debug.Log($"Increasing current mana to {newMana}");
            }
        }
    }

    private void RefillMana(Player player)
    {
        var field = typeof(Player).GetField("mana", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var maxField = typeof(Player).GetField("maxMana", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (field != null && maxField != null)
        {
            int max = (int)maxField.GetValue(player);
            int newMana = Mathf.Min((int)field.GetValue(player) + amount, max);
            Debug.Log($"Refilling mana to {newMana}");
            field.SetValue(player, newMana);
        }
    }

    private void IncreaseAttack(Player player)
    {
        var field = typeof(Player).GetField("attack", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (field != null)
        {
            int newAttack = (int)field.GetValue(player) + amount;
            Debug.Log($"Increasing attack from {(int)field.GetValue(player)} to {newAttack}");
            field.SetValue(player, newAttack);
        }
    }
}