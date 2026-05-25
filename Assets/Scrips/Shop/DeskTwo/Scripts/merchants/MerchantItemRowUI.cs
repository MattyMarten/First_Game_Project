using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MerchantItemRowUI : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private TMP_Text typeText;
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private GameObject selectedHighlight;

    public void Setup(Sprite icon, string itemName, int price, string itemType, int quantity, Action onClick, bool isSelected)
    {
        if (itemIcon != null)
        {
            itemIcon.sprite = icon;
            itemIcon.enabled = icon != null;
        }

        if (itemNameText != null)
            itemNameText.text = itemName;

        if (priceText != null)
            priceText.text = $"{price} C";

        if (typeText != null)
            typeText.text = itemType;

        if (quantityText != null)
            quantityText.text = $"x{quantity}";

        if (selectedHighlight != null)
            selectedHighlight.SetActive(isSelected);

        if (button != null)
        {
            button.onClick.RemoveAllListeners();

            if (onClick != null)
                button.onClick.AddListener(() => onClick.Invoke());
        }
    }
}