// Target path in your project: Assets/Buildings/Workshop/Machines/Goods station/Scripts/CraftingStationUI.cs
// (this REPLACES your existing file of the same name — changes are marked with "// UNLOCK:")

using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class CraftingStationUI : MonoBehaviour
{
    // Right panel fields (assign these in the inspector)
    public GameObject detailPanel;
    public TMP_Text detailName;
    public TMP_Text detailDesc;
    public TMP_Text detailRequirementsText; // requirements and have text
    public TMP_Text detailValueText;
    public Slider detailAmountSlider;
    public TMP_Text detailAmountText;
    public Button detailCraftButton;

    // Used to track/hold detail state
    private CraftingGood selectedRecipe;
    private int maxCraftable;

    [Header("Left panel/recipe list")]
    public Transform contentParent;
    public GameObject craftingRowPrefab;

    [Header("Player data")]
    public RawMaterialStorage playerStorage;
    public List<CraftingGood> goods;
    private CraftingGood selectedGood;

    [Header("Output")]
    public GoodStorage goodStorage;

    // UNLOCK: recipes are only shown here if RecipeUnlockManager says they're unlocked.
    [Header("Unlock")]
    public RecipeUnlockManager unlockManager;

    private List<GameObject> spawnedRows = new();

    void Awake()
    {
        if (playerStorage == null)
            playerStorage = FindAnyObjectByType<RawMaterialStorage>();

        if (goodStorage == null)
            goodStorage = FindAnyObjectByType<GoodStorage>();

        // UNLOCK:
        if (unlockManager == null)
            unlockManager = FindAnyObjectByType<RecipeUnlockManager>();
    }

    void OnEnable()
    {
        // UNLOCK: refresh live if a Data Stick unlocks something while this panel is open.
        if (unlockManager != null)
            unlockManager.OnRecipeUnlocked += HandleRecipeUnlocked;
    }

    void OnDisable()
    {
        if (unlockManager != null)
            unlockManager.OnRecipeUnlocked -= HandleRecipeUnlocked;
    }

    // UNLOCK:
    private void HandleRecipeUnlocked(IUnlockableRecipe recipe) => RefreshUI();

    void Start() => RefreshUI();

    public void RefreshUI()
    {
        foreach (var go in spawnedRows)
            Destroy(go);
        spawnedRows.Clear();

        // UNLOCK: Unknown/unrevealed recipes (Room_Workshop.md Section 10) are not shown at all.
        IEnumerable<CraftingGood> visibleGoods = unlockManager != null
            ? goods.Where(g => g != null && unlockManager.IsUnlocked(g))
            : goods;

        foreach (var good in visibleGoods)
        {
            var go = Instantiate(craftingRowPrefab, contentParent);
            // Set item name (left panel)
            go.transform.Find("ItemName").GetComponent<TMP_Text>().text = good.goodName;

            var image = go.transform.Find("ItemImage");
            if (image != null)
            {
                var icon = image.GetComponent<Image>();
                if (icon != null)
                    icon.sprite = good.icon;
            }

            var btn = go.GetComponent<Button>();
            var capturedGood = good;
            btn.onClick.AddListener(() => OnSelectRecipe(capturedGood));
            spawnedRows.Add(go);
        }
    }

    void OnSelectRecipe(CraftingGood recipe)
    {
        selectedRecipe = recipe;
        detailPanel.SetActive(true);

        // detailIcon.sprite = recipe.displayIcon; // <-- removed!
        detailName.text = recipe.goodName;
        detailDesc.text = recipe.description;
        detailValueText.text = $"Value: {recipe.valueGold}G";

        // Format requirements/have blocks
        var needSB = new System.Text.StringBuilder();
        foreach (var req in recipe.requiredMaterials)
        {
            int have = playerStorage.GetAll().TryGetValue(req.material, out int amt) ? amt : 0;
            string color = have >= req.amount ? "#FFFFFF" : "#FF5555";
            needSB.AppendLine($"<color={color}>{req.material.displayName} {req.amount}/{have}</color>");
        }

        detailRequirementsText.text = needSB.ToString();

        // Crafting amount logic
        maxCraftable = int.MaxValue;
        foreach (var req in recipe.requiredMaterials)
        {
            int have = playerStorage.GetAll().TryGetValue(req.material, out int amt) ? amt : 0;
            int max = req.amount > 0 ? have / req.amount : 0;
            if (max < maxCraftable) maxCraftable = max;
        }
        maxCraftable = Mathf.Max(0, maxCraftable);

        detailAmountSlider.minValue = 1;
        detailAmountSlider.maxValue = Mathf.Max(1, maxCraftable);
        detailAmountSlider.wholeNumbers = true;
        detailAmountSlider.value = maxCraftable > 0 ? 1 : 0;

        UpdateDetailAmountText((int)detailAmountSlider.value);
        detailAmountSlider.onValueChanged.RemoveAllListeners();
        detailAmountSlider.onValueChanged.AddListener((val) => UpdateDetailAmountText((int)val));

        detailCraftButton.interactable = maxCraftable > 0;
        detailCraftButton.onClick.RemoveAllListeners();
        detailCraftButton.onClick.AddListener(() => TryCraftSelected((int)detailAmountSlider.value));
    }

    void UpdateDetailAmountText(int val)
    {
        detailAmountText.text = $"x{val}";
    }

    void TryCraftSelected(int count)
    {
        if (selectedRecipe == null || goodStorage == null)
            return;

        for (int i = 0; i < count; i++)
        {
            bool canCraft = true;
            foreach (var req in selectedRecipe.requiredMaterials)
            {
                int have = playerStorage.GetAll().TryGetValue(req.material, out int amt) ? amt : 0;
                if (have < req.amount)
                {
                    canCraft = false;
                    break;
                }
            }

            if (!canCraft)
                break;

            foreach (var req in selectedRecipe.requiredMaterials)
                playerStorage.TrySpend(req.material, req.amount);

            goodStorage.Add(selectedRecipe, 1);
        }

        RefreshUI();

        if (selectedRecipe != null)
            OnSelectRecipe(selectedRecipe);
    }
}
