using UnityEngine;

// Room_Shop.md Section 17 — "dirt spots must be cleaned by going to them and
// pressing E." Same direct-Interactable pattern as CoreDepositInteractable
// (Assets/Buildings/Core/Scripts/CoreDepositInteractable.cs), except cleaning takes
// several presses instead of one — each E chips away at it, and only the last
// press actually removes it via DirtManager.CleanDirt().
public class DirtSpotInteractable : Interactable
{
    [Header("Cleaning")]
    [Tooltip("Number of E presses required before this spot is actually cleaned.")]
    [SerializeField] private int requiredCleanPresses = 5;

    [Header("Optional Feedback")]
    [Tooltip("Shrinks the dirt visual a little with each press so partial progress is visible without needing a dedicated UI. Purely cosmetic — safe to leave on or off.")]
    [SerializeField] private bool shrinkOnProgress = true;

    private DirtManager dirtManager;
    private DirtSpawnPoint assignedSpot;
    private int currentPresses;
    private Vector3 initialScale;

    public int RequiredCleanPresses => requiredCleanPresses;
    public int CurrentPresses => currentPresses;
    public float CleanProgress01 => requiredCleanPresses <= 0 ? 1f : (float)currentPresses / requiredCleanPresses;

    public event System.Action OnCleanProgressChanged;

    protected override void Awake()
    {
        base.Awake();
        initialScale = transform.localScale;
    }

    // Called by DirtManager right after Instantiate — this prefab has no scene
    // references to wire up front, since it doesn't exist until it's spawned.
    public void Initialize(DirtManager manager, DirtSpawnPoint spot)
    {
        dirtManager = manager;
        assignedSpot = spot;
        currentPresses = 0;
    }

    public override void Interact(PlayerInteraction player)
    {
        if (dirtManager == null || assignedSpot == null)
        {
            Debug.LogWarning($"{nameof(DirtSpotInteractable)}: not initialized, cannot clean.", this);
            return;
        }

        currentPresses++;

        if (shrinkOnProgress)
            transform.localScale = initialScale * (1f - CleanProgress01);

        OnCleanProgressChanged?.Invoke();

        if (currentPresses >= requiredCleanPresses)
            dirtManager.CleanDirt(assignedSpot, gameObject);
    }
}
