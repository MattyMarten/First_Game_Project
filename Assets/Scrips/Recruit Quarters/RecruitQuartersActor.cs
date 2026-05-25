using UnityEngine;

public class RecruitQuartersActor : MonoBehaviour
{
    private RecruitData recruitData;
    private RecruitBedSlot assignedBedSlot;

    public RecruitData RecruitData => recruitData;
    public RecruitBedSlot AssignedBedSlot => assignedBedSlot;

    public void Initialize(RecruitData recruit, RecruitBedSlot bedSlot)
    {
        recruitData = recruit;
        assignedBedSlot = bedSlot;

        RefreshVisualState();
    }

    public void SetPositionToAssignedBed()
    {
        if (assignedBedSlot == null || assignedBedSlot.StandPoint == null)
            return;

        Transform standPoint = assignedBedSlot.StandPoint;
        transform.SetPositionAndRotation(standPoint.position, standPoint.rotation);
    }

    public void RefreshVisualState()
    {
        SetPositionToAssignedBed();
    }
}