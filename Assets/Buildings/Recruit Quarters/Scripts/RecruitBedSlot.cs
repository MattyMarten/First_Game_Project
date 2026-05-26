using UnityEngine;

public class RecruitBedSlot : MonoBehaviour
{
    [Header("Bed Slot")]
    [SerializeField] private int bedIndex = -1;
    [SerializeField] private Transform standPoint;

    public int BedIndex => bedIndex;
    public Transform StandPoint => standPoint;
}