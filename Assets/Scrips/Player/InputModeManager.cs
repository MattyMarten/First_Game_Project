using UnityEngine;
using UnityEngine.InputSystem;

public class InputModeManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInput playerInput;

    [Header("Action Map Names")]
    [SerializeField] private string playerMapName = "Player";
    [SerializeField] private string inventoryMapName = "Inventory";
    [SerializeField] private string uiMapName = "UI";
    [SerializeField] private string debugMapName = "Debug";

    private InputActionMap playerMap;
    private InputActionMap inventoryMap;
    private InputActionMap uiMap;
    private InputActionMap debugMap;

    private void Awake()
    {
        if (playerInput == null)
            playerInput = GetComponent<PlayerInput>();

        if (playerInput == null)
        {
            //Debug.LogError($"{nameof(InputModeManager)}: No PlayerInput found.", this);
            return;
        }

        playerMap = playerInput.actions.FindActionMap(playerMapName, true);
        inventoryMap = playerInput.actions.FindActionMap(inventoryMapName, true);
        uiMap = playerInput.actions.FindActionMap(uiMapName, true);
        debugMap = playerInput.actions.FindActionMap(debugMapName, false);
    }

    private void Start()
    {
        SetGameplayMode();
    }

    public void SetGameplayMode()
    {
        //Debug.Log("InputModeManager: Gameplay Mode");
        EnableOnly(playerMap, uiMap, debugMap);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void SetInventoryMode()
    {
        //Debug.Log("InputModeManager: Inventory Mode");
        EnableOnly(inventoryMap, uiMap, debugMap);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void EnableOnly(params InputActionMap[] mapsToEnable)
    {
        DisableIfExists(playerMap);
        DisableIfExists(inventoryMap);
        DisableIfExists(uiMap);
        DisableIfExists(debugMap);

        foreach (var map in mapsToEnable)
        {
            if (map != null)
                map.Enable();
        }
    }

    private void DisableIfExists(InputActionMap map)
    {
        if (map != null)
            map.Disable();
    }
}