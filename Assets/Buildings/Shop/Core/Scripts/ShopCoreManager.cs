using UnityEngine;

public class ShopCoreManager : MonoBehaviour
{
    // Whole-Shop coordinator. Owns Shop-wide state and future shared systems.
    // Desk-specific behavior remains in ShopManager (Desk 1), ServiceDeskManager, and HireDeskManager.
    // Future whole-Shop ownership lives here: Appeal, daily visitor flow, spawn cycle, and report tracking.

    [Header("Shop State")]
    [SerializeField] private bool shopOpen;

    [Header("Desk Managers")]
    [SerializeField] private ShopManager desk1Manager;
    [SerializeField] private ServiceDeskManager desk2Manager;
    [SerializeField] private HireDeskManager desk3Manager;

    [Header("Future Shop Systems")]
    [SerializeField] private int shopAppeal = 50;
    [SerializeField] private bool spawnCycleRunning;
    [SerializeField] private int remainingDailyVisitors;

    [Header("Shared Spawn / Exit")]
    [SerializeField] private Transform sharedSpawnPoint;
    [SerializeField] private Transform sharedExitPoint;

    public bool IsShopOpen => shopOpen;

    public ShopManager Desk1Manager => desk1Manager;
    public ServiceDeskManager Desk2Manager => desk2Manager;
    public HireDeskManager Desk3Manager => desk3Manager;

    public int ShopAppeal => shopAppeal;
    public bool IsSpawnCycleRunning => spawnCycleRunning;
    public int RemainingDailyVisitors => remainingDailyVisitors;

    public Transform SharedSpawnPoint => sharedSpawnPoint;
    public Transform SharedExitPoint => sharedExitPoint;

    public void OpenShop()
    {
        shopOpen = true;

        if (desk1Manager != null)
            desk1Manager.OpenShop();
    }

    public void CloseShop()
    {
        shopOpen = false;

        if (desk1Manager != null)
            desk1Manager.CloseShop();
    }

    [ContextMenu("Open Shop")]
    private void DebugOpenShop()
    {
        OpenShop();
    }

    [ContextMenu("Close Shop")]
    private void DebugCloseShop()
    {
        CloseShop();
    }
}