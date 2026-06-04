using UnityEngine;

public class DayCounter : MonoBehaviour
{
    [SerializeField] private int currentDay = 1;

    public int CurrentDay => Mathf.Max(1, currentDay);
}
