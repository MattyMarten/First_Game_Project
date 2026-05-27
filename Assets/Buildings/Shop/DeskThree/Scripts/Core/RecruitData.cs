using System;
using UnityEngine;

[Serializable]
public class RecruitData
{
    public string recruitId;
    public string recruitName;
    public RecruitType recruitType;
    public RecruitClass recruitClass;
    public int level;
    public int hireCost;
    public bool canLevelUp;
    [TextArea] public string motivationText;
    public GameObject modelPrefab;
    public RecruitStats stats = new();

    [Header("Quarters")]
    public int assignedBedIndex = -1;

    [Header("Equipment Slots")]
    public string equippedBackpackId;
    public string equippedUtilityBeltId;
    public string accessorySlot1Id;
    public string accessorySlot2Id;
    public string accessorySlot3Id;

    [Header("Availability")]
    public RecruitStatus status = RecruitStatus.Idle;
    public int unavailableForDays;

    public bool IsAvailable => status == RecruitStatus.Idle;
    public bool IsFreeRecruit => recruitType == RecruitType.Free;
    public bool IsPaidRecruit => recruitType == RecruitType.Paid;
}