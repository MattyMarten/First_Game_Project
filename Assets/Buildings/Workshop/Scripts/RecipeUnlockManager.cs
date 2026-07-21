// Target path in your project: Assets/Buildings/Workshop/Scripts/RecipeUnlockManager.cs

using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Owns the "which recipes are unlocked" flag list for the whole Workshop
/// (Room_Workshop.md Section 9). Goods Workbench and Gear Workbench both
/// query this instead of keeping their own unlock state.
///
/// Not a recipe list itself, and not a Storage — just a set of unlocked IDs.
/// A recipe with IsUnlockedByDefault == true is always considered unlocked
/// and never needs to appear in unlockedRecipeIds at all.
/// </summary>
public class RecipeUnlockManager : MonoBehaviour
{
    /// Raised whenever a recipe transitions from locked to unlocked.
    /// Workbench UIs can subscribe to this to refresh their recipe lists live.
    public event Action<IUnlockableRecipe> OnRecipeUnlocked;

    private readonly HashSet<string> unlockedRecipeIds = new();

    public bool IsUnlocked(IUnlockableRecipe recipe)
    {
        if (recipe == null)
            return false;

        return recipe.IsUnlockedByDefault || unlockedRecipeIds.Contains(recipe.RecipeId);
    }

    /// <returns>
    /// True if this call actually unlocked something new.
    /// False if the recipe was already unlocked (default-unlocked or previously unlocked) —
    /// callers use this to decide whether to trigger the Data Stick duplicate-conversion path.
    /// </returns>
    public bool UnlockRecipe(IUnlockableRecipe recipe)
    {
        if (recipe == null || IsUnlocked(recipe))
            return false;

        unlockedRecipeIds.Add(recipe.RecipeId);
        OnRecipeUnlocked?.Invoke(recipe);
        return true;
    }
}
