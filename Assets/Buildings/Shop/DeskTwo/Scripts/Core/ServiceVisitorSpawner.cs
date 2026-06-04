using System.Collections.Generic;
using UnityEngine;

public class ServiceVisitorSpawner : MonoBehaviour
{
    // Temporary Desk 2 spawn executor and prototype local/planned spawn logic.
    // Final visitor scheduling will move to ShopCoreManager.   
    private enum PlannedVisitorType
    {
        Request,
        Talking,
        Merchant
    }

    [Header("References")]
    [SerializeField] private ServiceDeskManager serviceDeskManager;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform deskPoint;
    [SerializeField] private Transform exitPoint;

    [Header("Visitor Prefabs")]
    [SerializeField] private RequestVisitorNPC requestVisitorPrefab;
    [SerializeField] private TalkingVisitorNPC talkingVisitorPrefab;
    [SerializeField] private MerchantVisitorNPC merchantVisitorPrefab;

    [Header("Planned Counts")]
    [SerializeField] private int requestVisitorsToSpawn = 3;
    [SerializeField] private int talkingVisitorsToSpawn = 2;
    [SerializeField] private int merchantVisitorsToSpawn = 1;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 20f;
    [SerializeField] private bool buildPoolOnAwake = true;
    [SerializeField] private bool useLocalAutoSpawn = false;

    private float spawnTimer;
    private readonly List<PlannedVisitorType> plannedVisitors = new();

    private void Awake()
    {
        if (serviceDeskManager == null)
            serviceDeskManager = FindAnyObjectByType<ServiceDeskManager>();

        spawnTimer = spawnInterval;

        if (buildPoolOnAwake)
            BuildPlannedVisitorPool();
    }

    private void Update()
    {
        if (!useLocalAutoSpawn)
            return;

        if (serviceDeskManager == null)
            return;

        if (!serviceDeskManager.IsShopOpen)
        return;

        if (spawnPoint == null || deskPoint == null || exitPoint == null)
            return;

        if (plannedVisitors.Count == 0)
            return;

        if (!serviceDeskManager.CanAcceptAnotherVisitor())
            return;

        spawnTimer -= Time.deltaTime;

        if (spawnTimer > 0f)
            return;

        spawnTimer = spawnInterval;

        TrySpawnNextPlannedVisitor();
    }

    public void BuildPlannedVisitorPool()
    {
        plannedVisitors.Clear();

        AddPlannedVisitors(PlannedVisitorType.Request, requestVisitorsToSpawn);
        AddPlannedVisitors(PlannedVisitorType.Talking, talkingVisitorsToSpawn);
        AddPlannedVisitors(PlannedVisitorType.Merchant, merchantVisitorsToSpawn);

        ShufflePlannedVisitors();

        //Debug.Log($"Built service visitor pool. Total planned visitors: {plannedVisitors.Count}");
    }

    public bool CanSpawnServiceVisitor()
    {
        if (serviceDeskManager == null)
            return false;

        if (deskPoint == null)
            return false;

        if (plannedVisitors.Count == 0)
            return false;

        if (!serviceDeskManager.CanAcceptAnotherVisitor())
            return false;

        if (!serviceDeskManager.IsShopOpen)
        return false;

        return true;
    }

    public bool TrySpawnFromTrafficManager(Transform sharedSpawnPoint, Transform sharedExitPoint)
    {
        if (!CanSpawnServiceVisitor())
            return false;

        return TrySpawnNextPlannedVisitor(sharedSpawnPoint, sharedExitPoint);
    }

    private void AddPlannedVisitors(PlannedVisitorType visitorType, int count)
    {
        int safeCount = Mathf.Max(0, count);

        for (int i = 0; i < safeCount; i++)
            plannedVisitors.Add(visitorType);
    }

    private void ShufflePlannedVisitors()
    {
        for (int i = 0; i < plannedVisitors.Count; i++)
        {
            int randomIndex = Random.Range(i, plannedVisitors.Count);
            (plannedVisitors[i], plannedVisitors[randomIndex]) = (plannedVisitors[randomIndex], plannedVisitors[i]);
        }
    }

    private void TrySpawnNextPlannedVisitor()
    {
        TrySpawnNextPlannedVisitor(spawnPoint, exitPoint);
    }

    private bool TrySpawnNextPlannedVisitor(Transform spawnToUse, Transform exitToUse)
    {
        if (plannedVisitors.Count == 0)
            return false;

        PlannedVisitorType nextVisitorType = plannedVisitors[0];
        ServiceVisitorNPC prefabToSpawn = GetPrefabForType(nextVisitorType);

        if (prefabToSpawn == null)
        {
            //Debug.LogWarning($"No prefab assigned for planned visitor type: {nextVisitorType}");
            plannedVisitors.RemoveAt(0);
            return false;
        }

        ServiceVisitorNPC spawnedVisitor = Instantiate(prefabToSpawn, spawnToUse.position, spawnToUse.rotation);

        if (spawnedVisitor == null)
            return false;

        plannedVisitors.RemoveAt(0);
        spawnedVisitor.Initialize(serviceDeskManager, deskPoint, exitToUse);
        return true;
    }

    private ServiceVisitorNPC GetPrefabForType(PlannedVisitorType visitorType)
    {
        return visitorType switch
        {
            PlannedVisitorType.Request => requestVisitorPrefab,
            PlannedVisitorType.Talking => talkingVisitorPrefab,
            PlannedVisitorType.Merchant => merchantVisitorPrefab,
            _ => null
        };
    }
}