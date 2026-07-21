// Target path in your project: Assets/Buildings/Workshop/Machines/Utility Station/Scripts/UtilityCraftingUI.cs
// (this REPLACES your existing file of the same name — changes are marked with "// UNLOCK:")

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using StarterAssets;

public class UtilityCraftingUI : MonoBehaviour
{
    [Header("Category Buttons")]
    [SerializeField] private Button utilityButton;
    [SerializeField] private Button backpackButton;
    [SerializeField] private Button charmButton;

    [Header("Input")]
    [SerializeField] private StarterAssetsInputs input;
    [SerializeField] private GameObject panelRoot;

    [Header("Recipe List")]
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject craftingRowPrefab;

    [Header("Detail Panel")]
    [SerializeField] private GameObject detailPanel;
    [SerializeField] private TMP_Text detailName;
    [SerializeField] private TMP_Text detailDesc;
    [SerializeField] private TMP_Text detailRequirementsText;
    [SerializeField] private TMP_Text detailOwnedText;
    [SerializeField] private Button detailCraftButton;

    [Header("Data")]
    [SerializeField] private RawMaterialStorage playerStorage;
    [SerializeField] private CraftedUtilityStorage craftedUtilityStorage;
    [SerializeField] private List<UtilityCraftable> craftables = new();

    // UNLOCK: craftables are only shown here if RecipeUnlockManager says they're unlocked.
    [Header("Unlock")]
    [SerializeField] private RecipeUnlockManager unlockManager;

    [Header("Category Highlight")]
    [SerializeField] private Color selectedCategoryColor = Color.red;
    [SerializeField] private Color normalCategoryColor = Color.white;

    private readonly UtilityCategory[] categoryOrder =
    {
    UtilityCategory.Utility,
    UtilityCategory.Backpack,
    UtilityCategory.Charm
    };
    private UtilityCategory selectedCategory = UtilityCategory.Utility;
    private UtilityCraftable selectedCraftable;

    private List<GameObject> spawnedRows = new();

    private void Awake()
    {
        if (playerStorage == null)
            playerStorage = FindAnyObjectByType<RawMaterialStorage>();

        if (craftedUtilityStorage == null)
            craftedUtilityStorage = FindAnyObjectByType<CraftedUtilityStorage>();

        if (input == null)
            input = FindAnyObjectByType<StarterAssetsInputs>();

        // UNLOCK:
        if (unlockManager == null)
            unlockManager = FindAnyObjectByType<RecipeUnlockManager>();

        if (panelRoot == null)
            panelRoot = gameObject;

        SetupCategoryButtons();
    }

    private void OnEnable()
    {
        // UNLOCK: refresh live if a Data Stick unlocks something while this panel is open.
        if (unlockManager != null)
            unlockManager.OnRecipeUnlocked += HandleRecipeUnlocked;
    }

    private void OnDisable()
    {
        if (unlockManager != null)
            unlockManager.OnRecipeUnlocked -= HandleRecipeUnlocked;
    }

    // UNLOCK:
    private void HandleRecipeUnlocked(IUnlockableRecipe recipe) => BuildRecipeList();

    private void Start()
    {
        ShowCategory(UtilityCategory.Utility);
    }

    private void Update()
    {
        if (panelRoot == null || !panelRoot.activeSelf || input == null)
            return;

        if (input.ConsumeRightPage())
            CycleCategory(1);

        if (input.ConsumeLeftPage())
            CycleCategory(-1);
    }

    private void SetupCategoryButtons()
    {
        if (utilityButton != null)
            utilityButton.onClick.AddListener(() => ShowCategory(UtilityCategory.Utility));

        if (backpackButton != null)
            backpackButton.onClick.AddListener(() => ShowCategory(UtilityCategory.Backpack));

        if (charmButton != null)
            charmButton.onClick.AddListener(() => ShowCategory(UtilityCategory.Charm));
    }

    public void ShowCategory(UtilityCategory category)
    {
        selectedCategory = category;
        selectedCraftable = null;

        RefreshCategoryButtonVisuals();
        BuildRecipeList();
        RefreshDetailPanel();
    }

    private void BuildRecipeList()
    {
        foreach (var row in spawnedRows)
        {
            if (row != null)
                Destroy(row);
        }

        spawnedRows.Clear();

        // UNLOCK: Unknown/unrevealed craftables (Room_Workshop.md Section 10) are not shown at all.
        IEnumerable<UtilityCraftable> filteredCraftables = craftables.Where(c =>
            c != null &&
            c.category == selectedCategory &&
            (unlockManager == null || unlockManager.IsUnlocked(c)));

        foreach (UtilityCraftable craftable in filteredCraftables)
        {
            GameObject row = Instantiate(craftingRowPrefab, contentParent);

            Transform nameTransform = row.transform.Find("ItemName");
            Transform imageTransform = row.transform.Find("ItemImage");
            Transform ownedTransform = row.transform.Find("OwnedText");
            Transform highlightTransform = row.transform.Find("SelectedHighlight");

            if (nameTransform != null)
            {
                TMP_Text nameText = nameTransform.GetComponent<TMP_Text>();
                if (nameText != null)
                    nameText.text = craftable.itemName;
            }

            if (imageTransform != null)
            {
                Image iconImage = imageTransform.GetComponent<Image>();
                if (iconImage != null)
                    iconImage.sprite = craftable.icon;
            }

            if (ownedTransform != null)
            {
                TMP_Text ownedText = ownedTransform.GetComponent<TMP_Text>();
                if (ownedText != null)
                {
                    int ownedCount = craftedUtilityStorage != null ? craftedUtilityStorage.GetCount(craftable) : 0;
                    ownedText.text = $"x{ownedCount}";
                }
            }

            if (highlightTransform != null)
                highlightTransform.gameObject.SetActive(craftable == selectedCraftable);

            Button button = row.GetComponent<Button>();
            if (button != null)
            {
                UtilityCraftable capturedCraftable = craftable;
                button.onClick.AddListener(() => SelectCraftable(capturedCraftable));
            }

            spawnedRows.Add(row);
        }
    }

    private void SelectCraftable(UtilityCraftable craftable)
    {
        selectedCraftable = craftable;
        BuildRecipeList();
        RefreshDetailPanel();
    }

    private void RefreshDetailPanel()
    {
        if (detailPanel == null)
            return;

        if (selectedCraftable == null)
        {
            detailPanel.SetActive(false);
            return;
        }

        detailPanel.SetActive(true);

        if (detailName != null)
            detailName.text = selectedCraftable.itemName;

        if (detailDesc != null)
            detailDesc.text = selectedCraftable.description;

        if (detailRequirementsText != null)
            detailRequirementsText.text = BuildRequirementsText(selectedCraftable);

        if (detailOwnedText != null && craftedUtilityStorage != null)
            detailOwnedText.text = $"Owned: {craftedUtilityStorage.GetCount(selectedCraftable)}";

        if (detailCraftButton != null)
        {
            detailCraftButton.interactable = CanCraft(selectedCraftable);
            detailCraftButton.onClick.RemoveAllListeners();
            detailCraftButton.onClick.AddListener(TryCraftSelected);
        }
    }

    private string BuildRequirementsText(UtilityCraftable craftable)
    {
        if (craftable == null || playerStorage == null)
            return "";

        Dictionary<RawMaterial, int> ownedMaterials = playerStorage.GetAll();
        System.Text.StringBuilder sb = new();

        foreach (var req in craftable.requiredMaterials)
        {
            int have = ownedMaterials.TryGetValue(req.material, out int amount) ? amount : 0;
            string color = have >= req.amount ? "#FFFFFF" : "#FF5555";
            string materialName = req.material != null ? req.material.displayName : "Missing Material";

            sb.AppendLine($"<color={color}>{materialName} {req.amount}/{have}</color>");
        }

        return sb.ToString();
    }

    private bool CanCraft(UtilityCraftable craftable)
    {
        if (craftable == null || playerStorage == null)
            return false;

        Dictionary<RawMaterial, int> ownedMaterials = playerStorage.GetAll();

        foreach (var req in craftable.requiredMaterials)
        {
            int have = ownedMaterials.TryGetValue(req.material, out int amount) ? amount : 0;
            if (have < req.amount)
                return false;
        }

        return true;
    }

    private void TryCraftSelected()
    {
        if (selectedCraftable == null || playerStorage == null || craftedUtilityStorage == null)
            return;

        if (!CanCraft(selectedCraftable))
            return;

        foreach (var req in selectedCraftable.requiredMaterials)
            playerStorage.TrySpend(req.material, req.amount);

        craftedUtilityStorage.Add(selectedCraftable, 1);

        BuildRecipeList();
        RefreshDetailPanel();
    }

    private void CycleCategory(int direction)
    {
        int currentIndex = System.Array.IndexOf(categoryOrder, selectedCategory);
        if (currentIndex < 0)
            currentIndex = 0;

        currentIndex += direction;

        if (currentIndex < 0)
            currentIndex = categoryOrder.Length - 1;
        else if (currentIndex >= categoryOrder.Length)
            currentIndex = 0;

        ShowCategory(categoryOrder[currentIndex]);
    }

    private void RefreshCategoryButtonVisuals()
    {
        SetCategoryButtonColor(utilityButton, selectedCategory == UtilityCategory.Utility);
        SetCategoryButtonColor(backpackButton, selectedCategory == UtilityCategory.Backpack);
        SetCategoryButtonColor(charmButton, selectedCategory == UtilityCategory.Charm);
    }


    private void SetCategoryButtonColor(Button button, bool selected)
    {
        if (button == null)
            return;

        Image image = button.GetComponent<Image>();
        if (image != null)
            image.color = selected ? selectedCategoryColor : normalCategoryColor;
    }
}
