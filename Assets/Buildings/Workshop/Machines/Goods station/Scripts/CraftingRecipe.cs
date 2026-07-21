// Target path in your project: Assets/Buildings/Workshop/Machines/Goods station/Scripts/CraftingRecipe.cs
// (this REPLACES your existing file of the same name — only the Unlock header + interface are new)

using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CraftingGood", menuName = "Scriptable Objects/Crafting Good")]
public class CraftingGood : ScriptableObject, IUnlockableRecipe
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

    [Header("Unlock (Room_Workshop.md Section 9)")]
    [Tooltip("If true, this recipe is available from the start and never needs a Data Stick.")]
    public bool isUnlockedByDefault = true;

    public string RecipeId => name;
    public bool IsUnlockedByDefault => isUnlockedByDefault;
}
