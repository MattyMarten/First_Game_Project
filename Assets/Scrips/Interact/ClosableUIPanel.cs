using UnityEngine;
using StarterAssets;

public class ClosableUIPanel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private StarterAssetsInputs input;
    [SerializeField] private InputModeManager inputModeManager;

    private void Awake()
    {
        if (panelRoot == null)
            panelRoot = gameObject;

        if (input == null)
            input = FindAnyObjectByType<StarterAssetsInputs>();

        if (inputModeManager == null)
            inputModeManager = FindAnyObjectByType<InputModeManager>();
    }

    private void Update()
    {
        if (panelRoot == null || !panelRoot.activeSelf)
            return;

        if (input != null && input.ConsumeCloseInventory())
        {
            panelRoot.SetActive(false);

            if (inputModeManager != null)
                inputModeManager.SetGameplayMode();
        }
    }
}
