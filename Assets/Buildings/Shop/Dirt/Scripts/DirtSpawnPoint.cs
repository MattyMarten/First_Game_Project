using UnityEngine;

// Room_Shop.md Section 17 — "dirt spawns randomly among valid dirt spawn points."
// Pure marker + occupancy flag, same role ShopBrowsePoint/ShopQueueSpot play for
// buyers — DirtManager owns the logic, this just marks "a dirt spot could go here"
// and tracks whether one currently is.
public class DirtSpawnPoint : MonoBehaviour
{
    public bool IsOccupied { get; private set; }

    public Vector3 Position => transform.position;
    public Quaternion Rotation => transform.rotation;

    public void Occupy()
    {
        IsOccupied = true;
    }

    public void Vacate()
    {
        IsOccupied = false;
    }
}
