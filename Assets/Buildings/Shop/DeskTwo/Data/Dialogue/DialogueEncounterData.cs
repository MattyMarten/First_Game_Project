using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueEncounter", menuName = "Shop/Desk 2/Dialogue Encounter")]
public class DialogueEncounterData : ScriptableObject
{
    [Header("NPC Info")]
    public string npcName;

    [Header("Encounter Info")]
    public string encounterId;
    public string encounterTitle;

    [TextArea(3, 6)]
    public string openingLine;

    [Header("Answers")]
    public List<DialogueChoiceData> choices = new();
}

[System.Serializable]
public class DialogueChoiceData
{
    [TextArea(2, 4)]
    public string playerReply;

    [TextArea(2, 4)]
    public string npcResponse;

    public int rewardMoney;
    public bool givesInfo;

    [TextArea(2, 4)]
    public string infoText;
}