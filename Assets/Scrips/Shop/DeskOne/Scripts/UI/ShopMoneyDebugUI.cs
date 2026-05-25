using UnityEngine;
using TMPro;

public class ShopMoneyDebugUI : MonoBehaviour
{
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private string prefix = "Shop Money: ";

    private void Awake()
    {
        if (shopManager == null)
            shopManager = FindAnyObjectByType<ShopManager>();

        RefreshUI();
    }

private void OnEnable()
{
    if (shopManager == null)
        shopManager = FindAnyObjectByType<ShopManager>();

    if (shopManager != null)
        shopManager.OnMoneyChanged += RefreshUI;
}

    private void OnDisable()
    {
        if (shopManager != null)
            shopManager.OnMoneyChanged -= RefreshUI;
    }

    public void RefreshUI()
    {
        if (moneyText == null)
            return;

        if (shopManager == null)
        {
            moneyText.text = $"{prefix}N/A";
            return;
        }

        moneyText.text = $"{prefix}{shopManager.CurrentMoney}";
    }
}