using UnityEngine;

[CreateAssetMenu(fileName = "NewShopRequest", menuName = "Quest Board/Request Data")]
public class ShopRequestData : ScriptableObject
{
    [Header("NPC Info")]
    public string npcName;

    [TextArea(3, 6)]
    public string npcDialogue;

    [Header("Task Info")]
    public string requestId;
    public string requestTitle;
    public RequestDifficulty difficulty = RequestDifficulty.E;

    [TextArea(3, 6)]
    public string description;

    public int timeLimitDays = 1;

    [Header("Reward")]
    public int rewardMoney;

    [Header("Rules")]
    public bool oneTimeOnly = true;
    public int minDay = 1;
}