// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;
// using System.Collections.Generic;

// public class GoodStorageUI : MonoBehaviour
// {
//     public GoodStorage storage;
//     public Transform contentRoot;
//     public GameObject rowPrefab;

//     private List<GameObject> spawnedRows = new();

//     public void RefreshUI()
//     {
//         foreach (var row in spawnedRows)
//             Destroy(row);
//         spawnedRows.Clear();

//         if (storage == null)
//             return;

//         Dictionary<CraftingGood, int> all = storage.GetAll();
//         foreach (var kv in all)
//         {
//             var go = Instantiate(rowPrefab, contentRoot);

//             var nameText = go.transform.Find("goodNameText").GetComponent<TMP_Text>();
//             var amountText = go.transform.Find("goodAmountText").GetComponent<TMP_Text>();
//             var iconImage = go.transform.Find("goodIcon").GetComponent<Image>();

//             nameText.text = kv.Key.goodName;
//             amountText.text = "x" + kv.Value;

//             if (iconImage != null)
//                 iconImage.sprite = kv.Key.icon;

//             spawnedRows.Add(go);
//         }
//     }

//     private void OnEnable()
//     {
//         RefreshUI();
//     }
// }