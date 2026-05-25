using UnityEngine;
using System.Collections.Generic;

public enum UtilityCategory
{
    Utility,
    Backpack,
    Charm
}

[CreateAssetMenu(fileName = "UtilityCraftable", menuName = "Scriptable Objects/Utility Craftable")]
public class UtilityCraftable : ScriptableObject
{
    [Header("Display")]
    public string itemName;
    
    [TextArea]
    public string description;

    public Sprite icon;

    [Header("Category")]
    public UtilityCategory category;

    [Header("Merchant")]
    public int baseMerchantPrice = 20;

    [System.Serializable]
    public struct MaterialRequirement
    {
        public RawMaterial material;
        public int amount;
    }

    [Header("Materials Required")]
    public List<MaterialRequirement> requiredMaterials = new();
}
