// Target path in your project: Assets/Buildings/Merchant/Scripts/UI/MerchantSignUI.cs

using TMPro;
using UnityEngine;

/// <summary>
/// A standalone sign/speech-cloud somewhere in the Merchant Room (not tied
/// to any one pedestal). Shows a rotating flavor line that changes every
/// time the player buys something anywhere in the room — gives the static
/// pedestal room a bit of personality without needing an NPC.
///
/// Just drop this on whatever GameObject holds your speech-cloud's text
/// (a world-space TMP_Text works well for a physical sign; a screen-space
/// one works too if you'd rather it read like a caption).
/// </summary>
public class MerchantSignUI : MonoBehaviour
{
    [SerializeField] private MerchantRoomManager roomManager;
    [SerializeField] private TMP_Text lineText;

    private void Awake()
    {
        if (roomManager == null)
            roomManager = FindAnyObjectByType<MerchantRoomManager>();
    }

    private void OnEnable()
    {
        if (roomManager != null)
        {
            roomManager.OnFlavorLineChanged += HandleFlavorLineChanged;

            // In case the room already rolled a line before this sign enabled
            // (e.g. scene load order), show it immediately rather than waiting
            // for the next purchase.
            if (!string.IsNullOrEmpty(roomManager.CurrentFlavorLine))
                HandleFlavorLineChanged(roomManager.CurrentFlavorLine);
        }
    }

    private void OnDisable()
    {
        if (roomManager != null)
            roomManager.OnFlavorLineChanged -= HandleFlavorLineChanged;
    }

    private void HandleFlavorLineChanged(string newLine)
    {
        if (lineText != null)
            lineText.text = newLine;
    }
}
