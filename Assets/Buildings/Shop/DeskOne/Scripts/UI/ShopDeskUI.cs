using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopDeskUI : MonoBehaviour
{
    [Header("Main Panel")]
    [SerializeField] private GameObject panel;

    [Header("References")]
    [SerializeField] private ShopManager shopManager;

    [Header("UI")]
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private TMP_Text buyerNameText;
    [SerializeField] private TMP_Text buyerDialogueText;
    [SerializeField] private Image itemIconImage;
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button declineButton;

    private void Awake()
    {
        if (shopManager == null)
            shopManager = FindAnyObjectByType<ShopManager>();

        if (panel != null)
            panel.SetActive(false);

        if (acceptButton != null)
            acceptButton.onClick.AddListener(AcceptSale);

        if (declineButton != null)
            declineButton.onClick.AddListener(DeclineSale);
    }

    private void OnEnable()
    {
        if (shopManager == null)
            shopManager = FindAnyObjectByType<ShopManager>();

        if (shopManager != null)
            shopManager.OnPendingSaleChanged += RefreshUI;
    }

    private void OnDisable()
    {
        if (shopManager != null)
            shopManager.OnPendingSaleChanged -= RefreshUI;
    }

    public void OpenDesk()
    {
        if (panel != null)
            panel.SetActive(true);

        RefreshUI();
    }

    public void CloseDesk()
    {
        if (panel != null)
            panel.SetActive(false);
    }

    public void RefreshUI()
    {
        if (shopManager == null)
            return;

        if (!shopManager.HasPendingSale)
        {
            ShowEmptyState();
            return;
        }

        ShowPendingSale();
    }

    private void ShowPendingSale()
    {
        CraftingGood pendingGood = shopManager.PendingGood;

        if (itemNameText != null)
            itemNameText.text = pendingGood != null ? pendingGood.goodName : "-";

        if (priceText != null)
            priceText.text = $"Price: {shopManager.PendingPrice}";

        if (buyerNameText != null)
            buyerNameText.text = shopManager.PendingBuyerName;

        if (buyerDialogueText != null)
            buyerDialogueText.text = shopManager.PendingBuyerDialogue;

        SetItemIcon(pendingGood != null ? pendingGood.icon : null);
        SetButtonState(true, true);
    }

    private void ShowEmptyState()
    {
        if (itemNameText != null)
            itemNameText.text = "-";

        if (priceText != null)
            priceText.text = "Price: -";

        if (buyerNameText != null)
            buyerNameText.text = string.Empty;

        if (buyerDialogueText != null)
            buyerDialogueText.text = "No customer waiting.";

        SetItemIcon(null);
        SetButtonState(false, false);
    }

    private void SetItemIcon(Sprite icon)
    {
        if (itemIconImage == null)
            return;

        itemIconImage.sprite = icon;
        itemIconImage.enabled = icon != null;
    }

    private void SetButtonState(bool acceptInteractable, bool declineInteractable)
    {
        if (acceptButton != null)
            acceptButton.interactable = acceptInteractable;

        if (declineButton != null)
            declineButton.interactable = declineInteractable;
    }

    public void AcceptSale()
    {
        if (shopManager == null || !shopManager.HasPendingSale)
            return;

        shopManager.AcceptPendingSale();
        RefreshUI();
    }

    public void DeclineSale()
    {
        if (shopManager == null || !shopManager.HasPendingSale)
            return;

        shopManager.DeclinePendingSale();
        RefreshUI();
    }
}