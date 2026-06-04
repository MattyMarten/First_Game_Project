using UnityEngine;

public class ShopCoreManager : MonoBehaviour
{
    [Header("Shop State")]
    [SerializeField] private bool shopOpen;

    [Header("Desk Managers")]
    [SerializeField] private ShopManager desk1Manager;
    [SerializeField] private ServiceDeskManager desk2Manager;
    [SerializeField] private HireDeskManager desk3Manager;

    [Header("Shared Spawn / Exit")]
    [SerializeField] private Transform sharedSpawnPoint;
    [SerializeField] private Transform sharedExitPoint;

    public bool IsShopOpen => shopOpen;

    public ShopManager Desk1Manager => desk1Manager;
    public ServiceDeskManager Desk2Manager => desk2Manager;
    public HireDeskManager Desk3Manager => desk3Manager;

    public Transform SharedSpawnPoint => sharedSpawnPoint;
    public Transform SharedExitPoint => sharedExitPoint;

    public void OpenShop()
    {
        shopOpen = true;
    }

    public void CloseShop()
    {
        shopOpen = false;
    }
}