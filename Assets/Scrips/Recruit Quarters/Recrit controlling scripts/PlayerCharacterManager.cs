using UnityEngine;

public class PlayerCharacterManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform activePlayerRig;
    [SerializeField] private CharacterController activeCharacterController;
    [SerializeField] private RecruitQuartersManager recruitQuartersManager;

    [Header("Shopkeeper")]
    [SerializeField] private Transform shopkeeperStandPoint;
    [SerializeField] private GameObject shopkeeperWorldActor;

    private RecruitData activeRecruit;

    public bool IsControllingRecruit => activeRecruit != null;
    public bool IsControllingShopkeeper => activeRecruit == null;
    public RecruitData ActiveRecruit => activeRecruit;

    private void Awake()
    {
        if (activePlayerRig == null)
            activePlayerRig = transform;

        if (activeCharacterController == null)
            activeCharacterController = GetComponent<CharacterController>();

        if (recruitQuartersManager == null)
            recruitQuartersManager = FindAnyObjectByType<RecruitQuartersManager>();

        RefreshWorldActorVisibility();
    }

    public bool SwitchToRecruit(RecruitData recruit)
    {
        if (recruit == null)
            return false;

        if (recruitQuartersManager == null)
            return false;

        RecruitQuartersActor targetActor = recruitQuartersManager.GetSpawnedActorByRecruitId(recruit.recruitId);
        if (targetActor == null)
        {
            Debug.LogWarning($"PlayerCharacterManager: No spawned actor found for recruit {recruit.recruitName}", this);
            return false;
        }

        if (activeRecruit != null && activeRecruit.recruitId == recruit.recruitId)
            return true;

        Vector3 switchFromPosition = activePlayerRig != null ? activePlayerRig.position : transform.position;

        HandlePreviousControlledCharacterOnSwitch(switchFromPosition);

        Vector3 targetPosition = targetActor.transform.position;
        Quaternion targetRotation = targetActor.transform.rotation;

        MovePlayerRigTo(targetPosition, targetRotation);

        recruitQuartersManager.SetRecruitActorVisible(recruit.recruitId, false);
        activeRecruit = recruit;

        RefreshWorldActorVisibility();
        return true;
    }

    public void SwitchToShopkeeper()
    {
        Vector3 switchFromPosition = activePlayerRig != null ? activePlayerRig.position : transform.position;

        HandlePreviousControlledCharacterOnSwitch(switchFromPosition);

        if (shopkeeperStandPoint != null)
            MovePlayerRigTo(shopkeeperStandPoint.position, shopkeeperStandPoint.rotation);

        activeRecruit = null;

        RefreshWorldActorVisibility();
    }

    private void HandlePreviousControlledCharacterOnSwitch(Vector3 switchFromPosition)
    {
        if (IsControllingRecruit)
            ReleaseActiveRecruitAtSwitchPosition(switchFromPosition);
        else
            ReleaseShopkeeperAtSwitchPosition(switchFromPosition);
    }

    private void ReleaseActiveRecruitAtSwitchPosition(Vector3 switchFromPosition)
    {
        if (activeRecruit == null || recruitQuartersManager == null)
            return;

        RecruitQuartersActor actor = recruitQuartersManager.GetSpawnedActorByRecruitId(activeRecruit.recruitId);
        if (actor == null)
            return;

        Vector3 homePosition = actor.transform.position;
        Quaternion homeRotation = actor.transform.rotation;

        actor.transform.SetPositionAndRotation(switchFromPosition, homeRotation);
        actor.gameObject.SetActive(true);

        ReturnToPointWalker walker = actor.GetComponent<ReturnToPointWalker>();
        if (walker != null)
            walker.WalkTo(homePosition);
    }

    private void ReleaseShopkeeperAtSwitchPosition(Vector3 switchFromPosition)
    {
        if (shopkeeperWorldActor == null || shopkeeperStandPoint == null)
            return;

        shopkeeperWorldActor.transform.SetPositionAndRotation(
            switchFromPosition,
            shopkeeperStandPoint.rotation
        );

        shopkeeperWorldActor.SetActive(true);

        ReturnToPointWalker walker = shopkeeperWorldActor.GetComponent<ReturnToPointWalker>();
        if (walker != null)
            walker.WalkTo(shopkeeperStandPoint.position);
    }

    private void RefreshWorldActorVisibility()
    {
        if (shopkeeperWorldActor != null)
            shopkeeperWorldActor.SetActive(IsControllingRecruit);
    }

    private void MovePlayerRigTo(Vector3 position, Quaternion rotation)
    {
        if (activePlayerRig == null)
            return;

        bool hadController = activeCharacterController != null;

        if (hadController)
            activeCharacterController.enabled = false;

        activePlayerRig.SetPositionAndRotation(position, rotation);

        if (hadController)
            activeCharacterController.enabled = true;
    }
}