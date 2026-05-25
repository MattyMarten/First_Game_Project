// using UnityEngine;
// using TMPro;
// using UnityEngine.UI;

// public class GoodsDisplay : MonoBehaviour
// {
//     [Header("Current Displayed Good")]
//     public CraftingGood currentGood;

//     [Header("UI / Visuals")]
//     public TMP_Text nameText;
//     public TMP_Text valueText;
//     public Image iconImage;

//     public void SetGood(CraftingGood good)
//     {
//         currentGood = good;
//         RefreshVisuals();
//     }

//     public void ClearGood()
//     {
//         currentGood = null;
//         RefreshVisuals();
//     }

//     public void RefreshVisuals()
//     {
//         if (currentGood == null)
//         {
//             if (nameText != null) nameText.text = "";
//             if (valueText != null) valueText.text = "";
//             if (iconImage != null) iconImage.sprite = null;
//             return;
//         }

//         if (nameText != null)
//             nameText.text = currentGood.goodName;

//         if (valueText != null)
//             valueText.text = $"{currentGood.valueGold}G";

//         if (iconImage != null)
//             iconImage.sprite = currentGood.icon;
//     }
// }