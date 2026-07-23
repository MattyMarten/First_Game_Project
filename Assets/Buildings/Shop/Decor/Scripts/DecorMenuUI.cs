using UnityEngine;
using TMPro;
using StarterAssets;

// Room_Shop.md Section 16 — the panel opened by DecorSpotInteractable. While open, the
// Inventory action map is active (set by PanelInteractable/InputModeManager), so Q/E
// arrive as ConsumeLeftPage()/ConsumeRightPage() — the same actions your inventory
// paging already uses. This class just reads those and calls DecorSpot.CyclePrev()/
// CycleNext(), same as DisplayMenuUI reads button clicks for DisplayStand.
public class DecorMenuUI : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject panel;

    [Header("Input")]
    [SerializeField] private StarterAssetsInputs input;

    [Header("Readout — structured, not one blended string")]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text categoryText;
    [SerializeField] private TMP_Text effectValueText;
    [SerializeField] private TMP_Text tierAndPriceText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text positionText;   // e.g. "Option 2 of 5"
    [SerializeField] private TMP_Text controlHintText; // e.g. "Q / E — cycle   Tab — close"

    private DecorSpot currentSpot;

    private void Awake()
    {
        if (input == null)
            input = FindAnyObjectByType<StarterAssetsInputs>();

        if (panel != null)
            panel.SetActive(false);

        if (controlHintText != null)
            controlHintText.text = "Q / E — cycle decor        Tab — close";
    }

    public void OpenForSpot(DecorSpot spot)
    {
        currentSpot = spot;

        if (panel != null)
            panel.SetActive(true);

        RefreshUI();
    }

    private void Update()
    {
        if (panel == null || !panel.activeSelf || currentSpot == null || input == null)
            return;

        // ConsumeRightPage/ConsumeLeftPage already exist on StarterAssetsInputs, bound
        // to E/Q in the Inventory map (StarterAssets.inputactions).
        if (input.ConsumeRightPage())
        {
            currentSpot.CycleNext();
            RefreshUI();
        }
        else if (input.ConsumeLeftPage())
        {
            currentSpot.CyclePrev();
            RefreshUI();
        }
    }

    private void RefreshUI()
    {
        if (currentSpot == null)
            return;

        DecorItemData item = currentSpot.CurrentItem;

        if (positionText != null)
        {
            positionText.text = currentSpot.OptionCount == 0
                ? "No options assigned"
                : $"Option {currentSpot.CurrentIndex + 2} of {currentSpot.OptionCount + 1}"; // +1 slot accounts for "(empty)"
        }

        if (item == null)
        {
            SetEmptyState();
            return;
        }

        if (nameText != null) nameText.text = item.decorName;
        if (categoryText != null) categoryText.text = $"Category: {item.GetCategoryLabel()}";
        if (effectValueText != null) effectValueText.text = $"Effect: {item.GetEffectValueLabel()}";
        if (tierAndPriceText != null)
        {
            string tierText = item.tierIndex > 0 ? $"Tier {item.tierIndex + 1}" : "Base tier";
            tierAndPriceText.text = $"{tierText} — {item.price} coins";
        }
        if (descriptionText != null) descriptionText.text = item.description;
    }

    private void SetEmptyState()
    {
        if (nameText != null) nameText.text = "(empty)";
        if (categoryText != null) categoryText.text = string.Empty;
        if (effectValueText != null) effectValueText.text = "No decor placed in this spot.";
        if (tierAndPriceText != null) tierAndPriceText.text = string.Empty;
        if (descriptionText != null) descriptionText.text = string.Empty;
    }
}
