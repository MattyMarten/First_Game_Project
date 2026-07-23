using System.Collections.Generic;
using UnityEngine;

// Room_Shop.md Section 17 — Dirt Rules. Shop-wide system, same architectural role
// DecorManager plays for decor: owns no physical spawn points itself, reads
// DirtSpawnPoint markers assigned in the Inspector, and exposes the price penalty
// through a getter that ShopManager.CalculateBuyerSalePrice() will read in Step 4.
public class DirtManager : MonoBehaviour
{
    [Header("Spawn Points")]
    [Tooltip("All valid locations a dirt spot could appear in the Shop.")]
    [SerializeField] private List<DirtSpawnPoint> spawnPoints = new();

    [Header("Dirt Prefab")]
    [Tooltip("Must have a DirtSpotInteractable component.")]
    [SerializeField] private GameObject dirtVisualPrefab;

    [Header("Spawn Rule (Section 17)")]
    [Tooltip("15% base chance per purchase, per doc.")]
    [SerializeField, Range(0f, 1f)] private float baseDirtSpawnChance = 0.15f;

    [Header("Price Penalty (Section 17 / 19)")]
    [Tooltip("Each active dirt spot lowers item prices by this much (5% per doc).")]
    [SerializeField, Range(0f, 1f)] private float pricePenaltyPerDirtSpot = 0.05f;

    [Header("Decor Integration (Step 1)")]
    [SerializeField] private DecorManager decorManager;

    private readonly List<(DirtSpawnPoint spot, GameObject instance)> activeDirt = new();

    public event System.Action OnDirtChanged;

    public int ActiveDirtCount => activeDirt.Count;

    private void Awake()
    {
        if (decorManager == null)
            decorManager = FindAnyObjectByType<DecorManager>();
    }

    // Call this from ShopManager.AcceptPendingSale() — "each purchase rolls a chance
    // to spawn a dirt spot" (Section 17). Decor's DirtReduction category subtracts
    // from the base chance before the roll, per DecorManager.GetDirtChanceReduction().
    public void TryRollDirtSpawn()
    {
        float dirtReduction = decorManager != null ? decorManager.GetDirtChanceReduction() : 0f;
        float spawnChance = Mathf.Max(0f, baseDirtSpawnChance - dirtReduction);

        if (Random.value >= spawnChance)
            return;

        if (!TryGetRandomFreeSpawnPoint(out DirtSpawnPoint spot))
            return; // every valid spot already has dirt on it — nothing to do

        SpawnDirtAt(spot);
    }

    private bool TryGetRandomFreeSpawnPoint(out DirtSpawnPoint result)
    {
        result = null;

        List<DirtSpawnPoint> freeSpots = new();
        foreach (DirtSpawnPoint spot in spawnPoints)
        {
            if (spot != null && !spot.IsOccupied)
                freeSpots.Add(spot);
        }

        if (freeSpots.Count == 0)
            return false;

        result = freeSpots[Random.Range(0, freeSpots.Count)];
        return true;
    }

    private void SpawnDirtAt(DirtSpawnPoint spot)
    {
        if (dirtVisualPrefab == null)
        {
            Debug.LogWarning($"{nameof(DirtManager)}: no dirtVisualPrefab assigned, cannot spawn dirt.", this);
            return;
        }

        GameObject instance = Instantiate(dirtVisualPrefab, spot.Position, spot.Rotation);

        DirtSpotInteractable interactable = instance.GetComponent<DirtSpotInteractable>();
        if (interactable == null)
        {
            Debug.LogWarning($"{nameof(DirtManager)}: dirtVisualPrefab has no DirtSpotInteractable.", this);
            Destroy(instance);
            return;
        }

        interactable.Initialize(this, spot);
        spot.Occupy();
        activeDirt.Add((spot, instance));

        OnDirtChanged?.Invoke();
    }

    // Called by DirtSpotInteractable.Interact() once the player presses E on a dirt spot.
    public void CleanDirt(DirtSpawnPoint spot, GameObject instance)
    {
        int index = activeDirt.FindIndex(entry => entry.spot == spot && entry.instance == instance);

        if (index < 0)
            return;

        activeDirt.RemoveAt(index);
        spot.Vacate();
        Destroy(instance);

        OnDirtChanged?.Invoke();
    }

    // Section 19, Final Sale Price Order step 4 ("apply dirt penalties") — wired into
    // ShopManager.CalculateBuyerSalePrice() in Step 4. Clamped at 0 so price never
    // goes negative if dirt piles up faster than it's cleaned.
    public float GetDirtPriceMultiplier()
    {
        return Mathf.Max(0f, 1f - (ActiveDirtCount * pricePenaltyPerDirtSpot));
    }
}
