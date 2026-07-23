// Target path in your project: Assets/Buildings/Merchant/Scripts/UI/MerchantPedestalUI.cs

using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Single generic panel for one pedestal's current contents. Deliberately
/// type-agnostic (reads through MerchantPedestalSlot.GetDisplayName()/
/// GetIcon() instead of branching per category) since a pedestal's category
/// can change day-to-day (see MerchantRoomManager.TryOverlayDataStick).
/// </summary>
public class MerchantPedestalUI : MonoBehaviour
{
    [Header("Display")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private TMP_Text emptyStateText;

    [Header("Purchase")]
    [SerializeField] private Button buyOneButton;
    [SerializeField] private Button buyAllButton;
    [SerializeField] private GameObject buyButtonsRoot;  // hidden entirely when the pedestal is empty
    [SerializeField] private GameObject buyAllButtonRoot; // hidden for single-item pedestals (Decor/Data Stick) where Buy All would be redundant

    private MerchantRoomManager roomManager;
    private int pedestalIndex;

    public void OpenForPedestal(MerchantRoomManager manager, int index)
    {
        roomManager = manager;
        pedestalIndex = index;

        Refresh();
    }

    private void Refresh()
    {
        if (roomManager == null || pedestalIndex < 0 || pedestalIndex >= roomManager.Pedestals.Count)
        {
            ShowEmptyState();
            return;
        }

        MerchantPedestalSlot slot = roomManager.Pedestals[pedestalIndex];

        if (slot == null || slot.isEmpty || !slot.HasAnyItem)
        {
            ShowEmptyState();
            return;
        }

        if (emptyStateText != null)
            emptyStateText.gameObject.SetActive(false);

        if (itemIcon != null)
        {
            Sprite icon = slot.GetIcon();
            itemIcon.sprite = icon;
            itemIcon.enabled = icon != null;
        }

        if (itemNameText != null)
            itemNameText.text = slot.GetDisplayName();

        if (priceText != null)
            priceText.text = $"{slot.finalPrice} C";

        if (quantityText != null)
        {
            // Decor and Data Stick pedestals are single-item, not stacked.
            bool showsQuantity = slot.category == MerchantPedestalCategory.Utility
                || slot.category == MerchantPedestalCategory.Backpack
                || slot.category == MerchantPedestalCategory.Charm
                || slot.category == MerchantPedestalCategory.Material;

            quantityText.gameObject.SetActive(showsQuantity);
            if (showsQuantity)
                quantityText.text = $"x{slot.quantity}";
        }

        bool stackable = slot.utilityItem != null || slot.materialItem != null;

        if (buyButtonsRoot != null)
            buyButtonsRoot.SetActive(true);

        if (buyAllButtonRoot != null)
            buyAllButtonRoot.SetActive(stackable && slot.quantity > 1);

        if (buyOneButton != null)
        {
            buyOneButton.onClick.RemoveAllListeners();
            buyOneButton.onClick.AddListener(HandleBuyOneClicked);
        }

        if (buyAllButton != null)
        {
            buyAllButton.onClick.RemoveAllListeners();
            buyAllButton.onClick.AddListener(HandleBuyAllClicked);
        }
    }

    private void ShowEmptyState()
    {
        if (itemIcon != null)
            itemIcon.enabled = false;

        if (itemNameText != null)
            itemNameText.text = string.Empty;

        if (priceText != null)
            priceText.text = string.Empty;

        if (quantityText != null)
            quantityText.gameObject.SetActive(false);

        if (buyButtonsRoot != null)
            buyButtonsRoot.SetActive(false);

        if (emptyStateText != null)
        {
            emptyStateText.gameObject.SetActive(true);
            emptyStateText.text = "Nothing here today.";
        }
    }

    private void HandleBuyOneClicked()
    {
        if (roomManager == null)
            return;

        roomManager.TryPurchaseOne(pedestalIndex);
        Refresh();
    }

    private void HandleBuyAllClicked()
    {
        if (roomManager == null)
            return;

        roomManager.TryPurchaseAll(pedestalIndex);
        Refresh();
    }
}
