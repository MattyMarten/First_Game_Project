using System.Collections.Generic;
using UnityEngine;

// Room_Shop.md Section 16 — physical floor plate / wall frame. Holds the set of decor
// options valid for this spot and the currently-selected one, and pushes changes to
// DecorManager so shop-wide totals stay correct.
//
// KNOWN TEMPORARY SHORTCUT (log to Known_Temporary_Systems.md): `availableOptions` is a
// hand-assigned list per spot in the Inspector. There is no "owned decor inventory" yet —
// that only makes sense once the Merchant (Stage 6) can sell decor. Once it exists, this
// should pull from the player's owned-decor set filtered by `slotType` instead of a fixed
// designer list.
public class DecorSpot : MonoBehaviour
{
    [Header("Identity")]
    [Tooltip("Must be unique across all decor spots in the Shop.")]
    [SerializeField] private string spotId;
    [SerializeField] private DecorSlotType slotType;

    [Header("Options (temporary — see class comment)")]
    [SerializeField] private List<DecorItemData> availableOptions = new();

    [Header("References")]
    [SerializeField] private DecorManager decorManager;

    // -1 = spot is empty / no decor selected. Index 0..N-1 map into availableOptions.
    [SerializeField] private int currentIndex = -1;

    public event System.Action OnSelectionChanged;

    public string SpotId => spotId;
    public DecorSlotType SlotType => slotType;
    public DecorItemData CurrentItem => IsValidIndex(currentIndex) ? availableOptions[currentIndex] : null;

    // -1 = empty. Exposed (not just CurrentItem) so the menu UI can show a precise
    // "Option 2 of 5" position instead of just the item name.
    public int CurrentIndex => currentIndex;
    public int OptionCount => availableOptions.Count;

    private void Awake()
    {
        if (decorManager == null)
            decorManager = FindAnyObjectByType<DecorManager>();

        // Strip any mis-assigned options that don't match this spot's slot type so a
        // designer mistake in the Inspector can't silently mix Wall/Floor decor.
        availableOptions.RemoveAll(item => item == null || item.slotType != slotType);
    }

    private void Start()
    {
        ApplyCurrentSelection();
    }

    public void CycleNext()
    {
        if (availableOptions.Count == 0)
            return;

        // Range is -1..Count-1 so "empty" is always reachable, not just skipped over.
        currentIndex = currentIndex + 1 >= availableOptions.Count ? -1 : currentIndex + 1;
        ApplyCurrentSelection();
    }

    public void CyclePrev()
    {
        if (availableOptions.Count == 0)
            return;

        currentIndex = currentIndex - 1 < -1 ? availableOptions.Count - 1 : currentIndex - 1;
        ApplyCurrentSelection();
    }

    public string GetReadoutText()
    {
        return CurrentItem != null ? CurrentItem.GetReadout() : "(empty)";
    }

    private void ApplyCurrentSelection()
    {
        if (decorManager != null)
            decorManager.SetDecorAtSpot(spotId, CurrentItem);

        OnSelectionChanged?.Invoke();
    }

    private bool IsValidIndex(int index)
    {
        return index >= 0 && index < availableOptions.Count;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(spotId))
            spotId = gameObject.name;
    }
#endif
}
