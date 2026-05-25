using UnityEngine;

public class ShopBrowsePoint : MonoBehaviour
{
    private ShopManager shopManager;
    private ShopBuyerNPC occupyingBuyer;

    public bool IsOccupied => occupyingBuyer != null;

    private void Awake()
    {
        shopManager = FindAnyObjectByType<ShopManager>();
    }

    private void OnEnable()
    {
        if (shopManager == null)
            shopManager = FindAnyObjectByType<ShopManager>();

        if (shopManager != null)
            shopManager.RegisterBrowsePoint(this);
    }

    private void OnDisable()
    {
        if (shopManager != null)
            shopManager.UnregisterBrowsePoint(this);

        occupyingBuyer = null;
    }

    public bool TryReserve(ShopBuyerNPC buyer)
    {
        if (buyer == null)
            return false;

        if (occupyingBuyer == buyer)
            return true;

        if (occupyingBuyer != null)
            return false;

        occupyingBuyer = buyer;
        return true;
    }

    public void Release(ShopBuyerNPC buyer)
    {
        if (buyer == null)
            return;

        if (occupyingBuyer == buyer)
            occupyingBuyer = null;
    }
}