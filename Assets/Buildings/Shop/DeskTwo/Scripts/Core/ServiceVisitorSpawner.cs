using System.Collections.Generic;
using UnityEngine;

public class ServiceVisitorSpawner : MonoBehaviour
{
    // Temporary Desk 2 spawn executor and prototype local/planned spawn logic.
    // Final visitor scheduling will move to ShopCoreManager.   

    [Header("References")]
    [SerializeField] private ServiceDeskManager serviceDeskManager;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform deskPoint;
    [SerializeField] private Transform exitPoint;

    [Header("Visitor Prefabs")]
    [SerializeField] private RequestVisitorNPC requestVisitorPrefab;
    [SerializeField] private TalkingVisitorNPC talkingVisitorPrefab;
    [SerializeField] private MerchantVisitorNPC merchantVisitorPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 20f;
    [SerializeField] private bool useLocalAutoSpawn = false;

    private float spawnTimer;

    private void Awake()
    {
        if (serviceDeskManager == null)
            serviceDeskManager = FindAnyObjectByType<ServiceDeskManager>();

        spawnTimer = spawnInterval;
    }

    private void Update()
    {
        if (!useLocalAutoSpawn)
            return;
    }

    public bool CanSpawnServiceVisitor()
    {
        if (serviceDeskManager == null)
            return false;

        if (deskPoint == null)
            return false;

        if (!serviceDeskManager.CanAcceptAnotherVisitor())
            return false;

        if (!serviceDeskManager.IsShopOpen)
        return false;

        return true;
    }

    public bool TrySpawnSpecificVisitor(ShopCoreManager.ShopSpawnType spawnType, Transform sharedSpawnPoint, Transform sharedExitPoint)
    {
        if (serviceDeskManager == null)
            return false;

        if (deskPoint == null)
            return false;

        if (!serviceDeskManager.CanAcceptAnotherVisitor())
            return false;

        if (!serviceDeskManager.IsShopOpen)
            return false;

        Transform spawnToUse = sharedSpawnPoint != null ? sharedSpawnPoint : spawnPoint;
        Transform exitToUse = sharedExitPoint != null ? sharedExitPoint : exitPoint;

        if (spawnToUse == null || exitToUse == null)
            return false;

        ServiceVisitorNPC prefabToSpawn = GetPrefabForShopSpawnType(spawnType);

        if (prefabToSpawn == null)
            return false;

        ServiceVisitorNPC spawnedVisitor = Instantiate(prefabToSpawn, spawnToUse.position, spawnToUse.rotation);

        if (spawnedVisitor == null)
            return false;

        spawnedVisitor.Initialize(serviceDeskManager, deskPoint, exitToUse);
        return true;
    }

    private ServiceVisitorNPC GetPrefabForShopSpawnType(ShopCoreManager.ShopSpawnType spawnType)
    {
        return spawnType switch
        {
            ShopCoreManager.ShopSpawnType.Desk2TalkingVisitor => talkingVisitorPrefab,
            ShopCoreManager.ShopSpawnType.Desk2RequestVisitor => requestVisitorPrefab,
            ShopCoreManager.ShopSpawnType.Desk2MerchantVisitor => merchantVisitorPrefab,
            _ => null
        };
    }
}