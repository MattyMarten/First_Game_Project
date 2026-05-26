using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MerchantProfile", menuName = "Shop/Desk 2/Merchant Profile")]
public class MerchantProfileData : ScriptableObject
{
    [Header("Identity")]
    public string merchantName = "Merchant";

    [TextArea(2, 4)]
    public List<string> dialogueLines = new()
    {
        "I've got some new wares. Take a look.",
        "You should see what I brought today.",
        "I've come with a fresh stock of goods.",
        "Take a look at my wares.",
        "I've brought a few things you may want."
    };

    [Header("Utility Items")]
    public List<UtilityCraftable> utilityItems = new();

    [Header("Misc Items")]
    public List<UtilityCraftable> miscItems = new();

    [Header("Material Items")]
    public List<RawMaterial> materialItems = new();
}