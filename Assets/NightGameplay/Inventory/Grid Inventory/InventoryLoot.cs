using UnityEngine;
using UnityEngine.UI;

public class InventoryLoot : MonoBehaviour
{
    [Header("Runtime Data")]
    public Item item;

    [Header("Size (tiles) - base (unrotated)")]
    public int sizeWidth = 1;
    public int sizeHeight = 1;

    [Header("Runtime Rotation")]
    public LootRotation rotation = LootRotation.R0;

    [Header("Optional UI")]
    [SerializeField] private Image iconImage;

    public void RotateClockwise()
    {
        rotation = rotation switch
        {
            LootRotation.R0 => LootRotation.R90,
            LootRotation.R90 => LootRotation.R180,
            LootRotation.R180 => LootRotation.R270,
            LootRotation.R270 => LootRotation.R0,
            _ => LootRotation.R0
        };

        if (item != null)
            Apply(item);
    }

    private Sprite GetSpriteForRotation(Item data, LootRotation rot)
    {
        if (data == null) return null;

        return rot switch
        {
            LootRotation.R90 => data.imageR90 != null ? data.imageR90 : data.image,
            LootRotation.R180 => data.imageR180 != null ? data.imageR180 : data.image,
            LootRotation.R270 => data.imageR270 != null ? data.imageR270 : data.image,
            _ => data.image
        };
    }

    public void Apply(Item data)
    {
        item = data;

        int baseW = Mathf.Max(1, data.sizeWidth);
        int baseH = Mathf.Max(1, data.sizeHeight);
        sizeWidth = baseW;
        sizeHeight = baseH;

        bool swap = rotation == LootRotation.R90 || rotation == LootRotation.R270;
        int uiW = swap ? baseH : baseW;
        int uiH = swap ? baseW : baseH;

        RectTransform root = GetComponent<RectTransform>();
        root.sizeDelta = new Vector2(uiW * 64f, uiH * 64f);
        root.localRotation = Quaternion.identity;

        if (iconImage == null)
            return;

        // No transform rotation
        RectTransform iconRt = iconImage.rectTransform;
        iconRt.localRotation = Quaternion.identity;

        iconImage.sprite = GetSpriteForRotation(data, rotation);

        // Fill footprint
        iconRt.anchorMin = Vector2.zero;
        iconRt.anchorMax = Vector2.one;
        iconRt.offsetMin = Vector2.zero;
        iconRt.offsetMax = Vector2.zero;

        gameObject.name = data.itemName;
    }
}