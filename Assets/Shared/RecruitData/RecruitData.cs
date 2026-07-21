using System;
using System.Collections.Generic;
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

    // Data slot only — Druid (Stage 9) applies/treats these. Cursed stacks are represented
    // by repeated entries (e.g. two Cursed entries = 2 stacks). No reader of this list may
    // ever treat any effect, or combination of effects, as disabling control — see
    // Room_RecruitQuarters.md Section 16 ("no debuff rule").
    public List<RecruitStatusEffect> activeStatusEffects = new();

    [Header("Equipment Slots")]
    public string equippedBackpackId;
    public string equippedUtilityBeltId;
    public string accessorySlot1Id;
    public string accessorySlot2Id;
    public string accessorySlot3Id;

    public bool IsFreeRecruit => recruitType == RecruitType.Free;
    public bool IsPaidRecruit => recruitType == RecruitType.Paid;
}