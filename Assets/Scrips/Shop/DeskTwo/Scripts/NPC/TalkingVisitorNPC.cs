using UnityEngine;

public class TalkingVisitorNPC : ServiceVisitorNPC
{
    [Header("Dialogue")]
    [SerializeField] private DialogueEncounterData dialogueData;

    protected override void OnReachedDesk()
    {
        if (serviceDeskManager == null)
        {
            Leave();
            return;
        }

        if (dialogueData == null)
        {
            Debug.LogWarning("TalkingVisitorNPC has no dialogue data assigned.", this);
            Leave();
            return;
        }

        bool created = serviceDeskManager.TryCreatePendingDialogue(this, dialogueData);

        if (!created)
        {
            Leave();
            return;
        }
    }
}