using System;

[Serializable]
public class ExpeditionMemberData
{
    public string recruitId;
    public string recruitName;
    public RecruitType recruitType;
    public RecruitClass recruitClass;
    public int level;

    public string equippedBackpackId;
    public string equippedUtilityBeltId;
    public string accessorySlot1Id;
    public string accessorySlot2Id;
    public string accessorySlot3Id;

    public static ExpeditionMemberData FromRecruit(RecruitData recruit)
    {
        if (recruit == null)
            return null;

        return new ExpeditionMemberData
        {
            recruitId = recruit.recruitId,
            recruitName = recruit.recruitName,
            recruitType = recruit.recruitType,
            recruitClass = recruit.recruitClass,
            level = recruit.level,
            equippedBackpackId = recruit.equippedBackpackId,
            equippedUtilityBeltId = recruit.equippedUtilityBeltId,
            accessorySlot1Id = recruit.accessorySlot1Id,
            accessorySlot2Id = recruit.accessorySlot2Id,
            accessorySlot3Id = recruit.accessorySlot3Id
        };
    }
}