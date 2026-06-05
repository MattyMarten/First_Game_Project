using System.Text;
using TMPro;
using UnityEngine;

public class ShopSpawnPlanDebugUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ShopCoreManager shopCoreManager;
    [SerializeField] private TextMeshProUGUI queueText;

    [Header("Display")]
    [SerializeField] private string title = "=== Daily Visitor Queue ===";
    [SerializeField] private bool showIndexes = true;
    [SerializeField] private bool showQueueCount = true;
    [SerializeField] private string emptyMessage = "No planned visitors.";

    private void Awake()
    {
        if (shopCoreManager == null)
            shopCoreManager = FindAnyObjectByType<ShopCoreManager>();
    }

    private void OnEnable()
    {
        if (shopCoreManager != null)
            shopCoreManager.OnDailyVisitorListChanged += RefreshDisplay;

        RefreshDisplay();
    }

    private void OnDisable()
    {
        if (shopCoreManager != null)
            shopCoreManager.OnDailyVisitorListChanged -= RefreshDisplay;
    }

    public void RefreshDisplay()
    {
        if (queueText == null)
            return;

        if (shopCoreManager == null)
        {
            queueText.text = "ShopCoreManager reference missing.";
            return;
        }

        var visitorList = shopCoreManager.DailyVisitorSpawnList;

        StringBuilder builder = new();
        builder.AppendLine(title);

        if (showQueueCount)
            builder.AppendLine($"Count: {visitorList.Count}");

        if (visitorList.Count == 0)
        {
            builder.AppendLine(emptyMessage);
            queueText.text = builder.ToString();
            return;
        }

        for (int i = 0; i < visitorList.Count; i++)
        {
            string label = FormatSpawnType(visitorList[i]);

            if (showIndexes)
                builder.AppendLine($"{i + 1}. {label}");
            else
                builder.AppendLine(label);
        }

        queueText.text = builder.ToString();
    }

    private string FormatSpawnType(ShopCoreManager.ShopSpawnType spawnType)
    {
        return spawnType switch
        {
            ShopCoreManager.ShopSpawnType.Desk1Buyer => "Desk 1 Buyer",
            ShopCoreManager.ShopSpawnType.Desk2TalkingVisitor => "Desk 2 Talking Visitor",
            ShopCoreManager.ShopSpawnType.Desk2RequestVisitor => "Desk 2 Request Visitor",
            ShopCoreManager.ShopSpawnType.Desk2MerchantVisitor => "Desk 2 Merchant Visitor",
            ShopCoreManager.ShopSpawnType.Desk3HireVisitor => "Desk 3 Hire Visitor",
            _ => spawnType.ToString()
        };
    }
}