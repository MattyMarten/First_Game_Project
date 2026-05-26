using UnityEngine;
using UnityEngine.UI;
using StarterAssets;

public class InventoryControls : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject inventoryUI;

    [Header("Pages (content panels). Same order as tabs.")]
    [SerializeField] private GameObject[] pages;

    [Header("Top Tabs (the Image objects with Button+Text). Same order as pages.")]
    [SerializeField] private Image[] tabImages;
    [SerializeField] private Color tabSelectedColor = Color.red;
    [SerializeField] private Color tabNormalColor = Color.white;

    [Header("Optional References (auto-found on same GameObject if missing)")]
    [SerializeField] private StarterAssetsInputs input;
    [SerializeField] private InputModeManager inputModeManager;

    [Header("Optional: Grid controls (same GameObject or assigned)")]
    [SerializeField] private GridInventoryControls gridControls;

    private int currentPage = 0;
    private bool isOpen;

    private void Awake()
    {
        if (input == null)
            input = GetComponent<StarterAssetsInputs>();

        if (inputModeManager == null)
            inputModeManager = GetComponent<InputModeManager>();

        if (gridControls == null)
            gridControls = GetComponent<GridInventoryControls>();

        SetOpen(false);
        ShowPage(0);
    }

    private void Update()
    {
        if (input == null)
            return;

        if (!isOpen)
        {
            if (input.ConsumeOpenInventory())
                SetOpen(true);

            return;
        }

        // Inventory is open
        if (input.ConsumeCloseInventory())
        {
            SetOpen(false);
            return;
        }

        if (input.ConsumeRightPage())
            NextPage();

        if (input.ConsumeLeftPage())
            PreviousPage();
    }

    public bool IsOpen => isOpen;
    public void OpenAndGoToPage(int pageIndex)
    {
        if (!isOpen)
            SetOpen(true);

        ShowPage(pageIndex);
    }

    private void SetOpen(bool open)
    {
        isOpen = open;
       // Debug.Log("Inventory SetOpen: " + open);

        if (inventoryUI != null)
            inventoryUI.SetActive(isOpen);

        if (gridControls != null)
            gridControls.SetUIOpen(isOpen);

        if (isOpen && gridControls != null)
            gridControls.RefreshUIOnOpen();

        if (input != null)
            input.uiBlocked = isOpen;

        if (inputModeManager != null)
        {
            if (isOpen)
            {
                //Debug.Log("Calling SetInventoryMode");
                inputModeManager.SetInventoryMode();
            }
            else
            {
                //Debug.Log("Calling SetGameplayMode");
                inputModeManager.SetGameplayMode();
            }
        }
    }

    private void ShowPage(int pageIndex)
    {
        if (pages == null || pages.Length == 0)
            return;

        if (pageIndex < 0) pageIndex = pages.Length - 1;
        else if (pageIndex >= pages.Length) pageIndex = 0;

        for (int i = 0; i < pages.Length; i++)
        {
            if (pages[i] != null)
                pages[i].SetActive(i == pageIndex);
        }

        currentPage = pageIndex;
        RefreshTabColors();
    }

    private void RefreshTabColors()
    {
        if (tabImages == null || tabImages.Length == 0)
            return;

        for (int i = 0; i < tabImages.Length; i++)
        {
            if (tabImages[i] != null)
                tabImages[i].color = (i == currentPage) ? tabSelectedColor : tabNormalColor;
        }
    }

    private void NextPage() => ShowPage(currentPage + 1);
    private void PreviousPage() => ShowPage(currentPage - 1);
}