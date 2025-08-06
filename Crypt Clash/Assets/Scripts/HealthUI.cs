using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Player player;

    private void Update()
    {
        if (player != null && healthText != null)
        {
            healthText.text = $"Health: {player.GetHealth()} / {player.GetMaxHealth()}";
        }
    }
}
