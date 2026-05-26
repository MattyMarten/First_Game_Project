using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ServiceDeskUI : MonoBehaviour
{
    [Header("Main Panel")]
    [SerializeField] private GameObject panel;

    [Header("References")]
    [SerializeField] private ServiceDeskManager serviceDeskManager;

    [Header("Sections")]
    [SerializeField] private GameObject requestSection;
    [SerializeField] private GameObject dialogueSection;
    [SerializeField] private GameObject merchantSection;

    [Header("Request UI")]
    [SerializeField] private TMP_Text requestNpcNameText;
    [SerializeField] private TMP_Text requestDialogueText;
    [SerializeField] private TMP_Text requestDescriptionText;
    [SerializeField] private TMP_Text requestTaskNameText;
    [SerializeField] private TMP_Text requestRewardText;
    [SerializeField] private TMP_Text requestDifficultyText;
    [SerializeField] private TMP_Text requestTimeLimitText;
    [SerializeField] private Button requestAcceptButton;
    [SerializeField] private Button requestDeclineButton;

    [Header("Dialogue UI")]
    [SerializeField] private TMP_Text dialogueNpcNameText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Button[] dialogueChoiceButtons;
    [SerializeField] private TMP_Text[] dialogueChoiceButtonTexts;
    [SerializeField] private Button dialogueContinueButton;

    [Header("Merchant UI - Intro")]
    [SerializeField] private GameObject merchantIntroGroup;
    [SerializeField] private TMP_Text merchantNameText;
    [SerializeField] private TMP_Text merchantDialogueText;
    [SerializeField] private Button merchantViewWaresButton;
    [SerializeField] private Button merchantIgnoreButton;

    [Header("Merchant UI - Browsing")]
    [SerializeField] private GameObject merchantBrowseGroup;
    [SerializeField] private Transform merchantContentRoot;
    [SerializeField] private MerchantItemRowUI merchantRowPrefab;

    [SerializeField] private Image merchantSelectedItemIcon;
    [SerializeField] private TMP_Text merchantSelectedItemNameText;
    [SerializeField] private TMP_Text merchantSelectedItemPriceText;
    [SerializeField] private TMP_Text merchantSelectedItemTypeText;
    [SerializeField] private Button merchantBuyButton;
    [SerializeField] private Button merchantGoodbyeButton;

    private readonly List<GameObject> spawnedMerchantRows = new();

    private void Awake()
    {
        if (serviceDeskManager == null)
            serviceDeskManager = FindAnyObjectByType<ServiceDeskManager>();

        if (panel != null)
            panel.SetActive(false);

        if (requestAcceptButton != null)
            requestAcceptButton.onClick.AddListener(AcceptRequest);

        if (requestDeclineButton != null)
            requestDeclineButton.onClick.AddListener(DeclineRequest);

        if (dialogueContinueButton != null)
            dialogueContinueButton.onClick.AddListener(FinishDialogue);

        if (merchantViewWaresButton != null)
            merchantViewWaresButton.onClick.AddListener(OpenMerchantWares);

        if (merchantIgnoreButton != null)
            merchantIgnoreButton.onClick.AddListener(IgnoreMerchant);

        if (merchantBuyButton != null)
            merchantBuyButton.onClick.AddListener(BuyMerchantItem);

        if (merchantGoodbyeButton != null)
            merchantGoodbyeButton.onClick.AddListener(GoodbyeMerchant);

        SetupDialogueButtons();
    }

    private void OnEnable()
    {
        if (serviceDeskManager == null)
            serviceDeskManager = FindAnyObjectByType<ServiceDeskManager>();

        if (serviceDeskManager != null)
            serviceDeskManager.OnPendingInteractionChanged += RefreshUI;
    }

    private void OnDisable()
    {
        if (serviceDeskManager != null)
            serviceDeskManager.OnPendingInteractionChanged -= RefreshUI;
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
        HideAllSections();

        if (serviceDeskManager == null)
            return;

        switch (serviceDeskManager.CurrentInteractionType)
        {
            case ServiceDeskManager.ServiceInteractionType.Request:
                RefreshRequestUI();
                break;

            case ServiceDeskManager.ServiceInteractionType.Dialogue:
                RefreshDialogueUI();
                break;

            case ServiceDeskManager.ServiceInteractionType.MerchantOffer:
                RefreshMerchantUI();
                break;
        }
    }

    private void RefreshRequestUI()
    {
        if (requestSection != null)
            requestSection.SetActive(true);

        ShopRequestData request = serviceDeskManager.PendingRequest;
        bool hasRequest = serviceDeskManager.HasPendingRequest && request != null;

        if (requestNpcNameText != null)
            requestNpcNameText.text = hasRequest ? request.npcName : string.Empty;

        if (requestDialogueText != null)
            requestDialogueText.text = hasRequest ? request.npcDialogue : "No request available.";

        if (requestDescriptionText != null)
            requestDescriptionText.text = hasRequest ? request.description : string.Empty;

        if (requestTaskNameText != null)
            requestTaskNameText.text = hasRequest ? request.requestTitle : "-";

        if (requestRewardText != null)
            requestRewardText.text = hasRequest ? $"Reward: {request.rewardMoney}" : "Reward: -";

        if (requestDifficultyText != null)
            requestDifficultyText.text = hasRequest ? $"Difficulty: {request.difficulty}" : "Difficulty: -";

        if (requestTimeLimitText != null)
        {
            if (hasRequest)
            {
                int safeDays = Mathf.Max(1, request.timeLimitDays);
                string dayLabel = safeDays == 1 ? "day" : "days";
                requestTimeLimitText.text = $"Time Limit: {safeDays} {dayLabel}";
            }
            else
            {
                requestTimeLimitText.text = "Time Limit: -";
            }
        }

        if (requestAcceptButton != null)
            requestAcceptButton.interactable = hasRequest;

        if (requestDeclineButton != null)
            requestDeclineButton.interactable = hasRequest;
    }

    private void RefreshDialogueUI()
    {
        if (dialogueSection != null)
            dialogueSection.SetActive(true);

        DialogueEncounterData dialogue = serviceDeskManager.PendingDialogue;
        bool hasDialogue = serviceDeskManager.HasPendingDialogue && dialogue != null;
        bool hasResult = serviceDeskManager.HasDialogueResult;

        if (dialogueNpcNameText != null)
            dialogueNpcNameText.text = hasDialogue ? dialogue.npcName : string.Empty;

        if (dialogueText != null)
        {
            if (!hasDialogue)
            {
                dialogueText.text = "No dialogue available.";
            }
            else if (hasResult)
            {
                dialogueText.text = serviceDeskManager.CurrentDialogueResultText;
            }
            else
            {
                dialogueText.text = dialogue.openingLine;
            }
        }

        RefreshDialogueChoiceButtons(dialogue, hasResult);

        if (dialogueContinueButton != null)
            dialogueContinueButton.gameObject.SetActive(hasResult);
    }

    private void RefreshMerchantUI()
    {
        if (merchantSection != null)
            merchantSection.SetActive(true);

        GeneratedMerchantVisit visit = serviceDeskManager.PendingMerchantVisit;
        bool hasVisit = serviceDeskManager.HasPendingMerchantVisit && visit != null;
        bool isBrowsing = serviceDeskManager.IsViewingMerchantWares;

        if (merchantNameText != null)
            merchantNameText.text = hasVisit ? visit.merchantName : "Merchant";

        if (merchantDialogueText != null)
            merchantDialogueText.text = hasVisit ? visit.openingLine : "Take a look at my wares.";

        if (merchantIntroGroup != null)
            merchantIntroGroup.SetActive(hasVisit && !isBrowsing);

        if (merchantBrowseGroup != null)
            merchantBrowseGroup.SetActive(hasVisit && isBrowsing);

        ClearMerchantRows();

        if (!hasVisit || !isBrowsing)
        {
            RefreshMerchantSelectedItem();
            return;
        }

        BuildMerchantRows(visit);
        RefreshMerchantSelectedItem();
    }

    private void BuildMerchantRows(GeneratedMerchantVisit visit)
    {
        if (visit == null || merchantContentRoot == null || merchantRowPrefab == null)
            return;

        BuildUtilityRows(visit.utilityItems, "Utility");
        BuildUtilityRows(visit.miscItems, "Misc");
        BuildMaterialRows(visit.materialItems, "Material");
    }

    private void BuildUtilityRows(List<GeneratedMerchantUtilityItem> items, string typeLabel)
    {
        if (items == null)
            return;

        for (int i = 0; i < items.Count; i++)
        {
            GeneratedMerchantUtilityItem itemData = items[i];

            if (itemData == null || itemData.item == null)
                continue;

            MerchantItemRowUI row = Instantiate(merchantRowPrefab, merchantContentRoot);
            bool isSelected = serviceDeskManager.SelectedMerchantUtilityItem == itemData;

            row.Setup(
            itemData.item.icon,
            itemData.item.itemName,
            itemData.finalPrice,
            typeLabel,
            itemData.quantity,
            () => SelectMerchantUtility(itemData),
            isSelected);

            spawnedMerchantRows.Add(row.gameObject);
        }
    }

    private void BuildMaterialRows(List<GeneratedMerchantMaterialItem> items, string typeLabel)
    {
        if (items == null)
            return;

        for (int i = 0; i < items.Count; i++)
        {
            GeneratedMerchantMaterialItem itemData = items[i];

            if (itemData == null || itemData.item == null)
                continue;

            MerchantItemRowUI row = Instantiate(merchantRowPrefab, merchantContentRoot);
            bool isSelected = serviceDeskManager.SelectedMerchantMaterialItem == itemData;

            row.Setup(
                itemData.item.icon,
                itemData.item.displayName,
                itemData.finalPrice,
                typeLabel,
                itemData.quantity,
                () => SelectMerchantMaterial(itemData),
                isSelected);

            spawnedMerchantRows.Add(row.gameObject);
        }
    }

    private void ClearMerchantRows()
    {
        for (int i = 0; i < spawnedMerchantRows.Count; i++)
        {
            if (spawnedMerchantRows[i] != null)
                Destroy(spawnedMerchantRows[i]);
        }

        spawnedMerchantRows.Clear();
    }

    private void RefreshMerchantSelectedItem()
    {
        GeneratedMerchantUtilityItem selectedUtility = serviceDeskManager.SelectedMerchantUtilityItem;
        GeneratedMerchantMaterialItem selectedMaterial = serviceDeskManager.SelectedMerchantMaterialItem;

        Sprite icon = null;
        string itemName = "Select an item";
        string priceText = "Price: -";
        string typeText = string.Empty;
        bool hasSelection = false;

        if (selectedUtility != null && selectedUtility.item != null)
        {
            icon = selectedUtility.item.icon;
            itemName = selectedUtility.item.itemName;
            priceText = $"Price: {selectedUtility.finalPrice} C";
            typeText = selectedUtility.item.category == UtilityCategory.Utility ? "Utility" : "Misc";
            hasSelection = true;
        }
        else if (selectedMaterial != null && selectedMaterial.item != null)
        {
            icon = selectedMaterial.item.icon;
            itemName = selectedMaterial.item.displayName;
            priceText = $"Price: {selectedMaterial.finalPrice} C";
            typeText = "Material";
            hasSelection = true;
        }

        if (merchantSelectedItemIcon != null)
        {
            merchantSelectedItemIcon.sprite = icon;
            merchantSelectedItemIcon.enabled = icon != null;
        }

        if (merchantSelectedItemNameText != null)
            merchantSelectedItemNameText.text = itemName;

        if (merchantSelectedItemPriceText != null)
            merchantSelectedItemPriceText.text = priceText;

        if (merchantSelectedItemTypeText != null)
            merchantSelectedItemTypeText.text = typeText;

        bool canAfford = false;

        if (hasSelection)
        {
            ShopManager shopManager = FindAnyObjectByType<ShopManager>();
            if (shopManager != null)
            {
                int selectedPrice = selectedUtility != null ? selectedUtility.finalPrice : selectedMaterial.finalPrice;
                int selectedQuantity = selectedUtility != null ? selectedUtility.quantity : selectedMaterial.quantity;

                canAfford = selectedQuantity > 0 && shopManager.CurrentMoney >= selectedPrice;
            }
        }

        if (merchantBuyButton != null)
            merchantBuyButton.interactable = hasSelection && canAfford;
    }

    private void HideAllSections()
    {
        if (requestSection != null)
            requestSection.SetActive(false);

        if (dialogueSection != null)
            dialogueSection.SetActive(false);

        if (merchantSection != null)
            merchantSection.SetActive(false);
    }

    public void AcceptRequest()
    {
        if (serviceDeskManager == null || !serviceDeskManager.HasPendingRequest)
            return;

        serviceDeskManager.AcceptPendingRequest();
        RefreshUI();
    }

    public void DeclineRequest()
    {
        if (serviceDeskManager == null || !serviceDeskManager.HasPendingRequest)
            return;

        serviceDeskManager.DeclinePendingRequest();
        RefreshUI();
    }

    private void FinishDialogue()
    {
        if (serviceDeskManager == null || !serviceDeskManager.HasPendingDialogue)
            return;

        serviceDeskManager.FinishPendingDialogue();
        RefreshUI();
    }

    private void SetupDialogueButtons()
    {
        if (dialogueChoiceButtons == null)
            return;

        for (int i = 0; i < dialogueChoiceButtons.Length; i++)
        {
            int choiceIndex = i;

            if (dialogueChoiceButtons[i] == null)
                continue;

            dialogueChoiceButtons[i].onClick.RemoveAllListeners();
            dialogueChoiceButtons[i].onClick.AddListener(() => ChooseDialogueOption(choiceIndex));
        }
    }

    private void RefreshDialogueChoiceButtons(DialogueEncounterData dialogue, bool hasResult)
    {
        HideAllDialogueChoiceButtons();

        if (hasResult)
            return;

        if (dialogue == null || dialogue.choices == null)
            return;

        int choiceCount = Mathf.Min(dialogue.choices.Count, dialogueChoiceButtons.Length);

        for (int i = 0; i < choiceCount; i++)
        {
            if (dialogueChoiceButtons[i] != null)
                dialogueChoiceButtons[i].gameObject.SetActive(true);

            if (dialogueChoiceButtonTexts != null &&
                i < dialogueChoiceButtonTexts.Length &&
                dialogueChoiceButtonTexts[i] != null)
            {
                dialogueChoiceButtonTexts[i].text = dialogue.choices[i].playerReply;
            }
        }
    }

    private void HideAllDialogueChoiceButtons()
    {
        if (dialogueChoiceButtons == null)
            return;

        for (int i = 0; i < dialogueChoiceButtons.Length; i++)
        {
            if (dialogueChoiceButtons[i] != null)
                dialogueChoiceButtons[i].gameObject.SetActive(false);
        }
    }

    private void ChooseDialogueOption(int choiceIndex)
    {
        if (serviceDeskManager == null || !serviceDeskManager.HasPendingDialogue)
            return;

        serviceDeskManager.ChooseDialogueOption(choiceIndex);
        RefreshUI();
    }

    private void OpenMerchantWares()
    {
        if (serviceDeskManager == null || !serviceDeskManager.HasPendingMerchantVisit)
            return;

        serviceDeskManager.AcceptPendingMerchantVisit();
        RefreshUI();
    }

    private void IgnoreMerchant()
    {
        if (serviceDeskManager == null || !serviceDeskManager.HasPendingMerchantVisit)
            return;

        serviceDeskManager.DeclinePendingMerchantVisit();
        RefreshUI();
    }

    private void BuyMerchantItem()
    {
        if (serviceDeskManager == null || !serviceDeskManager.HasPendingMerchantVisit)
            return;

        serviceDeskManager.BuySelectedMerchantItem();
        RefreshUI();
    }

    private void GoodbyeMerchant()
    {
        if (serviceDeskManager == null || !serviceDeskManager.HasPendingMerchantVisit)
            return;

        serviceDeskManager.FinishMerchantVisit();
        RefreshUI();
    }

    private void SelectMerchantUtility(GeneratedMerchantUtilityItem itemData)
    {
        if (serviceDeskManager == null || itemData == null)
            return;

        serviceDeskManager.SelectMerchantUtilityItem(itemData);
        RefreshUI();
    }

    private void SelectMerchantMaterial(GeneratedMerchantMaterialItem itemData)
    {
        if (serviceDeskManager == null || itemData == null)
            return;

        serviceDeskManager.SelectMerchantMaterialItem(itemData);
        RefreshUI();
    }
}