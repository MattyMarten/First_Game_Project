using UnityEngine;

public class GrinderInteractable : Interactable
{
    [Header("Grinder Reference")]
    [SerializeField] private GrinderMachine grinderMachine;

    public override void Interact(PlayerInteraction player)
    {
        if (grinderMachine != null)
            grinderMachine.Grind();
    }
}
