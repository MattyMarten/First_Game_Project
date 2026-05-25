using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CraftingGood", menuName = "Scriptable Objects/Crafting Good")]
public class CraftingGood : ScriptableObject
{
    [Header("Display")]
    public string goodName;
    [TextArea]
    public string description;
    public Sprite icon;
    public int valueGold;
    public GameObject goodsPrefab;

    [System.Serializable]
    public struct MaterialRequirement
    {
        public RawMaterial material;
        public int amount;
    }

    [Header("Materials Required")]
    public List<MaterialRequirement> requiredMaterials = new();
}