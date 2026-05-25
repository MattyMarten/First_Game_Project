using UnityEngine;

public class HireVisitorSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HireDeskManager hireDeskManager;
    [SerializeField] private RecruitGenerator recruitGenerator;

    [Header("Spawn Setup")]
    [SerializeField] private HireVisitorNPC hireVisitorPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform deskPoint;
    [SerializeField] private Transform exitPoint;

    [Header("Spawn Timing")]
    [SerializeField] private float spawnInterval = 20f;
    [SerializeField] private bool useLocalAutoSpawn = true;

    private float spawnTimer;

    private void Awake()
    {
        if (hireDeskManager == null)
            hireDeskManager = FindAnyObjectByType<HireDeskManager>();

        if (recruitGenerator == null)
            recruitGenerator = FindAnyObjectByType<RecruitGenerator>();

        spawnTimer = spawnInterval;
    }

    private void Update()
    {
        if (!useLocalAutoSpawn)
            return;

        if (hireDeskManager == null || recruitGenerator == null)
            return;

        if (hireVisitorPrefab == null || spawnPoint == null || deskPoint == null || exitPoint == null)
            return;

        if (!recruitGenerator.CanGenerateRecruit())
            return;

        spawnTimer -= Time.deltaTime;

        if (spawnTimer > 0f)
            return;

        spawnTimer = spawnInterval;
        TrySpawnRecruitVisitor();
    }

    public bool CanSpawnHireVisitor()
    {
        if (hireDeskManager == null || recruitGenerator == null)
            return false;

        if (hireVisitorPrefab == null || deskPoint == null)
            return false;

        return recruitGenerator.CanGenerateRecruit();
    }

    public bool TrySpawnFromTrafficManager(Transform sharedSpawnPoint, Transform sharedExitPoint)
    {
        if (hireDeskManager == null || recruitGenerator == null)
            return false;

        if (hireVisitorPrefab == null || deskPoint == null)
            return false;

        if (!recruitGenerator.CanGenerateRecruit())
            return false;

        RecruitData generatedRecruit = recruitGenerator.GenerateRecruit();

        if (generatedRecruit == null)
            return false;

        Transform spawnToUse = sharedSpawnPoint != null ? sharedSpawnPoint : spawnPoint;
        Transform exitToUse = sharedExitPoint != null ? sharedExitPoint : exitPoint;

        if (spawnToUse == null || exitToUse == null)
            return false;

        HireVisitorNPC spawnedVisitor = Instantiate(hireVisitorPrefab, spawnToUse.position, spawnToUse.rotation);

        if (spawnedVisitor == null)
            return false;

        spawnedVisitor.Initialize(hireDeskManager, deskPoint, exitToUse, generatedRecruit);
        return true;
    }

    private void TrySpawnRecruitVisitor()
    {
        TrySpawnFromTrafficManager(spawnPoint, exitPoint);
    }
}