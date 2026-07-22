using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInfoManager : MonoBehaviour
{
    [SerializeField] private List<string> learnedInfoEntries = new();

    public event Action OnInfoChanged;

    public List<string> GetLearnedInfo()
    {
        return new List<string>(learnedInfoEntries);
    }

    public bool HasInfo(string infoText)
    {
        if (string.IsNullOrWhiteSpace(infoText))
            return false;

        return learnedInfoEntries.Contains(infoText);
    }

    public bool TryAddInfo(string infoText)
    {
        if (string.IsNullOrWhiteSpace(infoText))
            return false;

        if (learnedInfoEntries.Contains(infoText))
            return false;

        learnedInfoEntries.Add(infoText);
        OnInfoChanged?.Invoke();

        Debug.Log($"Learned info: {infoText}");
        return true;
    }
}