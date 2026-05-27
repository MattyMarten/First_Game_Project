using System.Collections.Generic;
using UnityEngine;

public class ShopNpcTrafficManager : MonoBehaviour
{
    private enum TrafficSpawnType
    {
        Desk1Buyer,
        Desk2ServiceVisitor,
        Desk3HireVisitor
    }

    [Header("References")]
    [SerializeField] private ShopBuyerSpawner shopBuyerSpawner;
    [SerializeField] private ServiceVisitorSpawner serviceVisitorSpawner;
    [SerializeField] private HireVisitorSpawner hireVisitorSpawner;

    [Header("Shared Spawn/Exit")]
    [SerializeField] private Transform sharedSpawnPoint;
    [SerializeField] private Transform sharedExitPoint;

    [Header("Traffic Timing")]
    [SerializeField] private float sharedSpawnInterval = 6f;
    [SerializeField] private bool trafficEnabled = true;

    private float spawnTimer;

    private void Awake()
    {
        if (shopBuyerSpawner == null)
            shopBuyerSpawner = FindAnyObjectByType<ShopBuyerSpawner>();

        if (serviceVisitorSpawner == null)
            serviceVisitorSpawner = FindAnyObjectByType<ServiceVisitorSpawner>();

        if (hireVisitorSpawner == null)
            hireVisitorSpawner = FindAnyObjectByType<HireVisitorSpawner>();

        spawnTimer = sharedSpawnInterval;
    }

    private void Update()
    {
        if (!trafficEnabled)
            return;

        if (sharedSpawnPoint == null || sharedExitPoint == null)
            return;

        spawnTimer -= Time.deltaTime;

        if (spawnTimer > 0f)
            return;

        spawnTimer = sharedSpawnInterval;

        TrySpawnNextNpc();
    }

    private void TrySpawnNextNpc()
    {
        List<TrafficSpawnType> validSpawnTypes = new();

        if (shopBuyerSpawner != null && shopBuyerSpawner.CanSpawnBuyer())
            validSpawnTypes.Add(TrafficSpawnType.Desk1Buyer);

        if (serviceVisitorSpawner != null && serviceVisitorSpawner.CanSpawnServiceVisitor())
            validSpawnTypes.Add(TrafficSpawnType.Desk2ServiceVisitor);

        if (hireVisitorSpawner != null && hireVisitorSpawner.CanSpawnHireVisitor())
            validSpawnTypes.Add(TrafficSpawnType.Desk3HireVisitor);

        if (validSpawnTypes.Count == 0)
            return;

        int randomIndex = Random.Range(0, validSpawnTypes.Count);
        TrafficSpawnType chosenType = validSpawnTypes[randomIndex];

        switch (chosenType)
        {
            case TrafficSpawnType.Desk1Buyer:
                shopBuyerSpawner.TrySpawnFromTrafficManager(sharedSpawnPoint, sharedExitPoint);
                break;

            case TrafficSpawnType.Desk2ServiceVisitor:
                serviceVisitorSpawner.TrySpawnFromTrafficManager(sharedSpawnPoint, sharedExitPoint);
                break;

            case TrafficSpawnType.Desk3HireVisitor:
                hireVisitorSpawner.TrySpawnFromTrafficManager(sharedSpawnPoint, sharedExitPoint);
                break;
        }
    }

    public void SetTrafficEnabled(bool enabled)
    {
        trafficEnabled = enabled;

        if (enabled)
            spawnTimer = sharedSpawnInterval;
    }
}