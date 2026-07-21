using UnityEngine;

/// <summary>
/// A physical, persistent locker placed at a fixed bed position — one per bed, matching
/// RecruitBedSlot.BedIndex. This is the room's "main equipment/info interaction point"
/// per Implementation_Status doc. It is NOT destroyed when a recruit is retired, dies,
/// or is reassigned — unlike RecruitQuartersActor, which is transient and gets
/// destroyed/respawned as recruits come and go.
/// </summary>
public class RecruitLocker : MonoBehaviour
{
    [SerializeField] private int bedIndex = -1;
    [SerializeField] private RecruitQuartersManager recruitQuartersManager;

    public int BedIndex => bedIndex;

    private void Awake()
    {
        if (recruitQuartersManager == null)
            recruitQuartersManager = FindAnyObjectByType<RecruitQuartersManager>();
    }

    /// <summary>
    /// Looks up whichever recruit currently occupies this bed, live, at interact time.
    /// Returns null if the bed is empty (locker still exists, just has nothing to show).
    /// </summary>
    public RecruitData GetOccupyingRecruit()
    {
        if (recruitQuartersManager == null)
            return null;

        return recruitQuartersManager.GetRecruitAtBedIndex(bedIndex);
    }
}
