using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [Header("Prompt")]
    [SerializeField] private GameObject promptObject;

    protected virtual void Awake()
    {
        if (promptObject != null)
            promptObject.SetActive(false);
    }

    public virtual void OnFocusEnter()
    {
        if (promptObject != null)
            promptObject.SetActive(true);
    }

    public virtual void OnFocusExit()
    {
        if (promptObject != null)
            promptObject.SetActive(false);
    }

    public abstract void Interact(PlayerInteraction player);
}