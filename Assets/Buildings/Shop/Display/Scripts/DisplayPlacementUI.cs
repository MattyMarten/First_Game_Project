// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;
// using System.Collections.Generic;

// public class DisplayPlacementUI : MonoBehaviour
// {
//     public GameObject panel;
//     public Transform contentRoot;
//     public GameObject rowPrefab;
//     public GoodStorage goodStorage;

//     private GoodsDisplay currentDisplay;
//     private readonly List<GameObject> spawnedRows = new();

//     public void OpenForDisplay(GoodsDisplay display)
//     {
//         currentDisplay = display;

//         if (panel != null)
//             panel.SetActive(true);

//         RefreshUI();
//     }

//     public void Close()
//     {
//         currentDisplay = null;

//         if (panel != null)
//             panel.SetActive(false);
//     }

//     public void RefreshUI()
//     {
//         foreach (var row in spawnedRows)
//             Destroy(row);
//         spawnedRows.Clear();

//         if (goodStorage == null || currentDisplay == null)
//             return;

//         foreach (var kv in goodStorage.GetAll())
//         {
//             CraftingGood good = kv.Key;
//             int amount = kv.Value;

//             if (amount <= 0 || good == null)
//                 continue;

//             var go = Instantiate(rowPrefab, contentRoot);

//             var nameText = go.transform.Find("goodNameText").GetComponent<TMP_Text>();
//             var amountText = go.transform.Find("goodAmountText").GetComponent<TMP_Text>();
//             var iconImage = go.transform.Find("goodIcon").GetComponent<Image>();
//             var button = go.GetComponent<Button>();

//             nameText.text = good.goodName;
//             amountText.text = "x" + amount;

//             if (iconImage != null)
//                 iconImage.sprite = good.icon;

//             CraftingGood capturedGood = good;
//             button.onClick.AddListener(() => SelectGood(capturedGood));

//             spawnedRows.Add(go);
//         }
//     }

//     private void SelectGood(CraftingGood good)
//     {
//         if (currentDisplay == null || goodStorage == null || good == null)
//             return;

//         if (!goodStorage.TrySpend(good, 1))
//             return;

//         currentDisplay.SetGood(good);
//         RefreshUI();
//     }
// }