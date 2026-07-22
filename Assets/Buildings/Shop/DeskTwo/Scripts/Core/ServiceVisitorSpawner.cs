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
    [SerializeField] private HireVisitorNPC hireVisitorPrefab;

    [Header("Recruit Generation (merged in from the old Desk Three)")]
    [SerializeField] private RecruitGenerator recruitGenerator;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 20f;
    [SerializeField] private bool useLocalAutoSpawn = false;

    private float spawnTimer;

    private void Awake()
    {
        if (serviceDeskManager == null)
            serviceDeskManager = FindAnyObjectByType<ServiceDeskManager>();

        if (recruitGenerator == null)
            recruitGenerator = FindAnyObjectByType<RecruitGenerator>();

        spawnTimer = spawnInterval;
    }

    private void Update()
    {
        if (!useLocalAutoSpawn)
            return;
    }

    public bool CanSpawnServiceVisitor(ShopCoreManager.ShopSpawnType spawnType)
    {
        if (serviceDeskManager == null)
            return false;

        if (deskPoint == null)
            return false;

        if (!serviceDeskManager.CanAcceptAnotherVisitor())
            return false;

        if (!serviceDeskManager.IsShopOpen)
            return false;

        // Recruit visitors additionally need the roster to actually have a
        // recruit to offer (Room_Shop.md Section 13 — the free-slot-ratio chance
        // already decided whether one was planned for today; this is just "is
        // there a generatable recruit right now").
        if (spawnType == ShopCoreManager.ShopSpawnType.Desk3HireVisitor)
            return hireVisitorPrefab != null && recruitGenerator != null && recruitGenerator.CanGenerateRecruit();

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

        if (spawnType == ShopCoreManager.ShopSpawnType.Desk3HireVisitor)
            return TrySpawnHireVisitor(spawnToUse, exitToUse);

        ServiceVisitorNPC prefabToSpawn = GetPrefabForShopSpawnType(spawnType);

        if (prefabToSpawn == null)
            return false;

        ServiceVisitorNPC spawnedVisitor = Instantiate(prefabToSpawn, spawnToUse.position, spawnToUse.rotation);

        if (spawnedVisitor == null)
            return false;

        spawnedVisitor.Initialize(serviceDeskManager, deskPoint, exitToUse);
        return true;
    }

    private bool TrySpawnHireVisitor(Transform spawnToUse, Transform exitToUse)
    {
        if (hireVisitorPrefab == null || recruitGenerator == null)
            return false;

        if (!recruitGenerator.CanGenerateRecruit())
            return false;

        RecruitData generatedRecruit = recruitGenerator.GenerateRecruit();

        if (generatedRecruit == null)
            return false;

        HireVisitorNPC spawnedVisitor = Instantiate(hireVisitorPrefab, spawnToUse.position, spawnToUse.rotation);

        if (spawnedVisitor == null)
            return false;

        spawnedVisitor.Initialize(serviceDeskManager, deskPoint, exitToUse, generatedRecruit);
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