using UnityEngine;

public class ShopDeskVisuals : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform deskItemSpot;

    [Header("Item Placement")]
    [SerializeField] private Vector3 itemLocalPosition = Vector3.zero;
    [SerializeField] private Vector3 itemLocalEulerAngles = Vector3.zero;
    [SerializeField] private Vector3 itemLocalScale = Vector3.one;

    private GameObject currentDeskItemVisual;

    public void ShowPendingItem(CraftingGood good)
    {
        ClearPendingItem();

        if (good == null || good.goodsPrefab == null || deskItemSpot == null)
            return;

        currentDeskItemVisual = Instantiate(good.goodsPrefab, deskItemSpot);
        currentDeskItemVisual.transform.localPosition = itemLocalPosition;
        currentDeskItemVisual.transform.localRotation = Quaternion.Euler(itemLocalEulerAngles);
        currentDeskItemVisual.transform.localScale = itemLocalScale;
    }

    public void ClearPendingItem()
    {
        if (currentDeskItemVisual != null)
        {
            Destroy(currentDeskItemVisual);
            currentDeskItemVisual = null;
        }
    }
}