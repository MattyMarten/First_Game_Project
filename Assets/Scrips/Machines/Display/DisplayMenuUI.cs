using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DisplayMenuUI : MonoBehaviour
{
    [Header("Main Panel")]
    [SerializeField] private GameObject panel;

    [Header("Slot List")]
    [SerializeField] private Transform slotContentRoot;
    [SerializeField] private GameObject slotRowPrefab;

    [Header("Goods List")]
    [SerializeField] private Transform goodsContentRoot;
    [SerializeField] private GameObject goodRowPrefab;

    [Header("Controls")]
    [SerializeField] private Button removeButton;
    [SerializeField] private TMP_Text selectedSlotText;

    [Header("Storage")]
    [SerializeField] private GoodStorage goodStorage;

    private DisplayStand currentDisplay;
    private int selectedSlotIndex = -1;

    private readonly List<GameObject> spawnedSlotRows = new();
    private readonly List<GameObject> spawnedGoodRows = new();

    private void Awake()
    {
        if (goodStorage == null)
            goodStorage = FindAnyObjectByType<GoodStorage>();

        if (panel != null)
            panel.SetActive(false);

        if (removeButton != null)
            removeButton.onClick.AddListener(RemoveFromSelectedSlot);
    }

    public void OpenForDisplay(DisplayStand displayStand)
    {
        currentDisplay = displayStand;
        selectedSlotIndex = -1;

        if (panel != null)
            panel.SetActive(true);

        RefreshUI();
    }

    public void Close()
    {
        currentDisplay = null;
        selectedSlotIndex = -1;

        if (panel != null)
            panel.SetActive(false);
    }

    public void RefreshUI()
    {
        ClearSpawnedRows();

        RefreshSelectedSlotLabel();

        if (currentDisplay == null)
            return;

        BuildSlotList();
        BuildGoodsList();
        UpdateRemoveButtonState();
    }

    private void BuildSlotList()
    {
        for (int i = 0; i < currentDisplay.SlotCount; i++)
        {
            int slotIndex = i;
            GameObject row = Instantiate(slotRowPrefab, slotContentRoot);

            TMP_Text slotNumberText = row.transform.Find("SlotNumberText").GetComponent<TMP_Text>();
            Image itemIcon = row.transform.Find("ItemIcon").GetComponent<Image>();
            Transform highlightTransform = row.transform.Find("SelectedHighlight");
            Button button = row.GetComponent<Button>();

            CraftingGood slottedGood = currentDisplay.GetGoodInSlot(slotIndex);

            if (slottedGood == null)
            {
                slotNumberText.text = (slotIndex + 1).ToString();
                slotNumberText.gameObject.SetActive(true);

                itemIcon.sprite = null;
                itemIcon.gameObject.SetActive(false);
            }
            else
            {
                slotNumberText.gameObject.SetActive(false);

                itemIcon.sprite = slottedGood.icon;
                itemIcon.gameObject.SetActive(true);
            }

            if (highlightTransform != null)
                highlightTransform.gameObject.SetActive(slotIndex == selectedSlotIndex);

            button.onClick.AddListener(() => SelectSlot(slotIndex));

            spawnedSlotRows.Add(row);
        }
    }
    private void BuildGoodsList()
    {
        if (goodStorage == null)
            return;

        Dictionary<CraftingGood, int> allGoods = goodStorage.GetAll();

        foreach (var kv in allGoods)
        {
            CraftingGood good = kv.Key;
            int amount = kv.Value;

            if (good == null || amount <= 0)
                continue;

            GameObject row = Instantiate(goodRowPrefab, goodsContentRoot);

            TMP_Text goodNameText = row.transform.Find("GoodNameText").GetComponent<TMP_Text>();
            TMP_Text goodAmountText = row.transform.Find("GoodAmountText").GetComponent<TMP_Text>();
            Image goodIconImage = row.transform.Find("GoodIcon")?.GetComponent<Image>();
            Button button = row.GetComponent<Button>();

            goodNameText.text = good.goodName;
            goodAmountText.text = $"x{amount}";

            if (goodIconImage != null)
                goodIconImage.sprite = good.icon;

            button.onClick.AddListener(() => PlaceGoodInSelectedSlot(good));

            spawnedGoodRows.Add(row);
        }
    }

    private void SelectSlot(int slotIndex)
    {
        selectedSlotIndex = slotIndex;
        RefreshUI();
    }

    private void PlaceGoodInSelectedSlot(CraftingGood good)
    {
        if (currentDisplay == null || goodStorage == null || good == null)
            return;

        if (selectedSlotIndex < 0 || selectedSlotIndex >= currentDisplay.SlotCount)
            return;

        CraftingGood existingGood = currentDisplay.GetGoodInSlot(selectedSlotIndex);

        if (existingGood == good)
            return;

        if (!goodStorage.TrySpend(good, 1))
            return;

        if (existingGood != null)
            goodStorage.Add(existingGood, 1);

        currentDisplay.PlaceGoodInSlot(selectedSlotIndex, good);
        RefreshUI();
    }

    private void RemoveFromSelectedSlot()
    {
        if (currentDisplay == null || goodStorage == null)
            return;

        if (selectedSlotIndex < 0 || selectedSlotIndex >= currentDisplay.SlotCount)
            return;

        CraftingGood removedGood = currentDisplay.RemoveGoodFromSlot(selectedSlotIndex);
        if (removedGood != null)
            goodStorage.Add(removedGood, 1);

        RefreshUI();
    }

    private void RefreshSelectedSlotLabel()
    {
        if (selectedSlotText == null)
            return;

        if (selectedSlotIndex < 0)
            selectedSlotText.text = "Selected Slot: None";
        else
            selectedSlotText.text = $"Selected Slot: {selectedSlotIndex + 1}";
    }

    private void UpdateRemoveButtonState()
    {
        if (removeButton == null || currentDisplay == null)
            return;

        bool hasValidSelection = selectedSlotIndex >= 0 && selectedSlotIndex < currentDisplay.SlotCount;
        bool hasItemInSlot = hasValidSelection && currentDisplay.GetGoodInSlot(selectedSlotIndex) != null;

        removeButton.interactable = hasItemInSlot;
    }

    private void ClearSpawnedRows()
    {
        foreach (GameObject row in spawnedSlotRows)
            Destroy(row);
        spawnedSlotRows.Clear();

        foreach (GameObject row in spawnedGoodRows)
            Destroy(row);
        spawnedGoodRows.Clear();
    }
}