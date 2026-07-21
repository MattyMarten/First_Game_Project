// Target path in your project: Assets/Buildings/Workshop/Scripts/IUnlockableRecipe.cs

/// <summary>
/// Shared contract for anything Data Sticks can unlock (Room_Workshop.md Section 9).
/// Implemented by CraftingGood (Goods Workbench) and UtilityCraftable (Gear Workbench)
/// so RecipeUnlockManager doesn't need to know which Workbench a recipe belongs to.
/// </summary>
public interface IUnlockableRecipe
{
    /// Stable identifier for this recipe. Uses the ScriptableObject asset name —
    /// simple and sufficient for now; revisit if asset renames ever become a problem.
    string RecipeId { get; }

    /// True for recipes that don't need a Data Stick at all (available from the start).
    bool IsUnlockedByDefault { get; }
}
