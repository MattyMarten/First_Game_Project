// Target path in your project: Assets/Buildings/Workshop/Scripts/DataStickConsumer.cs

using UnityEngine;

/// <summary>
/// Entry point for "a Data Stick reached the base" (Room_Workshop.md Section 9).
///
/// Nothing calls this yet — the real sources (a sector category's loot table,
/// the Merchant's Data Stick pedestal, request/talking visitors) don't exist
/// as systems yet. This is deliberately just a flag-unlock event handler per
/// the roadmap's Stage 3 note, ready for those later stages to call into.
///
/// For now, use the debug button below (or call Acquire() directly from the
/// Inspector / a test script) to unlock a recipe manually and confirm the
/// system works end to end.
/// </summary>
public class DataStickConsumer : MonoBehaviour
{
    [SerializeField] private RecipeUnlockManager unlockManager;
    [SerializeField] private CobaltCoinStorage coinStorage;

    [Header("Debug")]
    [Tooltip("Assign a stick here and use the context-menu action below to test unlocking without a real acquisition source yet.")]
    [SerializeField] private DataStickItem debugStickToAcquire;

    private void Awake()
    {
        if (unlockManager == null)
            unlockManager = FindAnyObjectByType<RecipeUnlockManager>();

        if (coinStorage == null)
            coinStorage = FindAnyObjectByType<CobaltCoinStorage>();
    }

    /// <summary>
    /// Called whenever a Data Stick reaches the base. Auto-unlocks the target
    /// recipe, or auto-converts to coins if the recipe was already unlocked.
    /// </summary>
    public void Acquire(DataStickItem stick)
    {
        if (stick == null)
        {
            Debug.LogWarning("DataStickConsumer.Acquire called with a null stick.");
            return;
        }

        if (unlockManager == null)
        {
            Debug.LogWarning("DataStickConsumer has no RecipeUnlockManager in the scene.");
            return;
        }

        IUnlockableRecipe recipe = stick.GetTargetRecipe();
        if (recipe == null)
        {
            Debug.LogWarning($"DataStick '{stick.stickName}' has no target recipe assigned (fill goodsRecipe or gearRecipe).");
            return;
        }

        bool wasNewlyUnlocked = unlockManager.UnlockRecipe(recipe);

        if (!wasNewlyUnlocked)
        {
            // Duplicate acquisition — auto-convert to coins instead of doing nothing.
            if (coinStorage != null)
                coinStorage.Add(stick.duplicateCoinValue);
            else
                Debug.LogWarning($"DataStick '{stick.stickName}' was a duplicate but no CobaltCoinStorage was found to pay out into.");
        }
    }

    [ContextMenu("Debug: Acquire Assigned Stick")]
    private void DebugAcquireAssignedStick()
    {
        Acquire(debugStickToAcquire);
    }
}
