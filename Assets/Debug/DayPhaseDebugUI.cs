using UnityEngine;

/// <summary>
/// This file replaces: Assets/Debug/DayPhaseDebugUI.cs (same path, same class
/// name). DEBUG-ONLY test harness, Stage 0 + Stage 2 combined.
///
/// New in Stage 2: deposit / upgrade-unlock / upgrade buttons and a Core
/// status readout, so you can test the whole Core flow without building
/// real UI yet - trigger Warning by letting Evening pass with 0 loaded coins,
/// then watch it go Offline, then deposit to recover it.
/// </summary>
public class DayPhaseDebugUI : MonoBehaviour
{
    private void OnGUI()
    {
        var system = DayPhaseSystem.Instance;

        GUILayout.BeginArea(new Rect(10, 10, 280, 620), GUI.skin.box);
        GUILayout.Label("<b>Day/Phase Debug Panel</b>");

        if (system == null)
        {
            GUILayout.Label("No DayPhaseSystem found in scene.");
        }
        else
        {
            GUILayout.Label($"Day: {system.CurrentDay}");
            GUILayout.Label($"Phase: {system.CurrentPhase}");

            GUILayout.Space(8);

            GUI.enabled = system.CurrentPhase == DayPhase.Morning;
            if (GUILayout.Button("Open Shop (Morning -> Day)"))
                system.OpenShop();

            GUI.enabled = system.CurrentPhase == DayPhase.Day;
            if (GUILayout.Button("Close Shop (Day -> Evening)"))
                system.CloseShop();

            GUI.enabled = system.CurrentPhase == DayPhase.Evening;
            if (GUILayout.Button("Launch Night (Evening -> Night)"))
                system.LaunchNight();

            GUI.enabled = system.CurrentPhase == DayPhase.Night;
            if (GUILayout.Button("Resolve Night (Night -> Morning, Day+1)"))
                system.ResolveNight();

            GUI.enabled = true;
        }

        GUILayout.Space(12);
        GUILayout.Label("<b>Core Room</b>");

        var core = CoreRoomManager.Instance;
        if (core == null)
        {
            GUILayout.Label("No CoreRoomManager found in scene.");
        }
        else
        {
            GUILayout.Label($"State: {core.CurrentState}  (Online: {core.IsOnline})");
            GUILayout.Label($"Loaded: {core.LoadedCoins} / {core.Capacity}");
            GUILayout.Label($"Daily consumption: {core.DailyConsumption}");
            GUILayout.Label($"Range: {core.Range}");
            GUILayout.Label($"Upgrade level: LV{core.UpgradeLevel} (slot {(core.UpgradeSlotUnlocked ? "unlocked" : "locked")})");

            if (GUILayout.Button($"Deposit {core.DepositAmount} coins"))
                core.TryDeposit();

            GUI.enabled = !core.UpgradeSlotUnlocked;
            if (GUILayout.Button("Force-unlock upgrade slot (debug)"))
                core.UnlockUpgradeSlotDebug();

            GUI.enabled = core.UpgradeSlotUnlocked && !core.IsMaxLevel;
            if (GUILayout.Button("Try upgrade"))
                core.TryUpgrade();

            GUI.enabled = true;
        }

        GUILayout.Space(12);

        var coinStorage = FindAnyObjectByType<CobaltCoinStorage>();
        if (coinStorage != null)
        {
            GUILayout.Label($"Storage reserve coins: {coinStorage.CoinCount}");
            if (GUILayout.Button("Add 100 coins to reserve (debug)"))
                coinStorage.Add(100);
        }

        GUILayout.EndArea();
    }
}
