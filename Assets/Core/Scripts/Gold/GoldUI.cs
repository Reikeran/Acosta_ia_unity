using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    [SerializeField] private TMP_Text goldText;

    private void OnEnable()
    {
        GoldManager.OnGoldChanged += UpdateGoldText;
    }

    private void OnDisable()
    {
        GoldManager.OnGoldChanged -= UpdateGoldText;
    }

    private void Start()
    {
        // Initialize display with current value
        UpdateGoldText(GoldManager.TotalGold);
    }

    private void UpdateGoldText(int totalGold)
    {
        goldText.text = $"Gold: {totalGold}";
    }
}
