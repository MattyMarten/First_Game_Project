using UnityEngine;
using UnityEngine.UI;
using TMPro;
using StarterAssets;
using System.Linq;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private LayerMask interactLayer;

    [Header("References")]
    [SerializeField] private StarterAssetsInputs input;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private PlayerHoldItem playerHoldItem;
    [SerializeField] private InventoryControls inventoryControls;

    [Header("Pickup UI")]
    [SerializeField] private GameObject pickupInfoPanel;
    [SerializeField] private GameObject pickupHandIcon;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private Image itemSpriteImage;
    [SerializeField] private Image rarityBackground;
    [SerializeField] private TextMeshProUGUI materialValueText;

    private Interactable currentInteractable;

    private void Awake()
    {
        if (input == null)
            input = GetComponent<StarterAssetsInputs>();

        if (playerHoldItem == null)
            playerHoldItem = GetComponent<PlayerHoldItem>();

        if (playerCamera == null)
            playerCamera = Camera.main;

        if (inventoryControls == null)
            inventoryControls = GetComponent<InventoryControls>();

        SetPickupUIVisible(false);
    }

    [System.Obsolete]
    private void Update()
    {
        if (!AreCoreRefsValid())
        {
            ClearCurrentInteractable();
            SetPickupUIVisible(false);
            return;
        }

        if (inventoryControls != null && inventoryControls.IsOpen)
        {
            ClearCurrentInteractable();
            SetPickupUIVisible(false);
            return;
        }

        bool hasTarget = Physics.SphereCast(
            new Ray(playerCamera.transform.position, playerCamera.transform.forward),
            0.4f,
            out RaycastHit hit,
            interactDistance,
            interactLayer,
            QueryTriggerInteraction.Collide
        );

        Interactable newInteractable = hasTarget ? hit.collider.GetComponentInParent<Interactable>() : null;
        ItemPickup pickup = hasTarget ? hit.collider.GetComponentInParent<ItemPickup>() : null;

        UpdateFocusedInteractable(newInteractable);

        if (pickup != null && newInteractable == null)
        {
            HandlePickup(pickup);
            return;
        }

        SetPickupUIVisible(false);

        if (currentInteractable != null && input.ConsumeInteract())
            currentInteractable.Interact(this);
    }

    private bool AreCoreRefsValid()
    {
        return input != null && playerCamera != null;
    }

    private void UpdateFocusedInteractable(Interactable newInteractable)
    {
        if (currentInteractable == newInteractable)
            return;

        if (currentInteractable != null)
            currentInteractable.OnFocusExit();

        currentInteractable = newInteractable;

        if (currentInteractable != null)
            currentInteractable.OnFocusEnter();
    }

    private void ClearCurrentInteractable()
    {
        if (currentInteractable != null)
        {
            currentInteractable.OnFocusExit();
            currentInteractable = null;
        }
    }

    private void HandlePickup(ItemPickup pickup)
    {
        SetPickupUIVisible(true);

        if (pickup.Item != null)
        {
            string htmlColor = ColorUtility.ToHtmlStringRGB(pickup.Item.RarityColor);
            string infoText =
                $"<b>{pickup.Item.itemName}</b>\n" +
                $"<size=80%><color=#{htmlColor}>{pickup.Item.rarity}</color></size>\n" +
                $"{FormatMaterialValue(pickup.Item)}";

            itemNameText.text = infoText;
            itemSpriteImage.sprite = pickup.Item.image;
            rarityBackground.color = pickup.Item.RarityColor;
        }

        if (input.ConsumeInteract())
        {
            if (inventoryManager == null)
            {
                Debug.LogWarning($"{nameof(PlayerInteraction)}: inventoryManager is not assigned.", this);
                return;
            }

            pickup.PickUp(inventoryManager, playerHoldItem);
        }
    }

    private void SetPickupUIVisible(bool visible)
    {
        if (pickupInfoPanel != null)
            pickupInfoPanel.SetActive(visible);

        if (pickupHandIcon != null)
            pickupHandIcon.SetActive(visible);
    }

    private string FormatMaterialValue(Item item)
    {
        if (item.MaterialValue == null || item.MaterialValue.Count == 0)
            return "No materials";

        return string.Join(", ", item.MaterialValue.Select(kv => $"{kv.Value}x {kv.Key.displayName}"));
    }
}