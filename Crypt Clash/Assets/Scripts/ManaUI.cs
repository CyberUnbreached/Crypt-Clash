using TMPro;
using UnityEngine;

public class ManaUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI manaText;
    [SerializeField] private Player player;

    private void Update()
    {
        if (player != null && manaText != null)
        {
            manaText.text = $"Mana: {player.GetMana()} / {player.GetMaxMana()}";
        }
    }
}
