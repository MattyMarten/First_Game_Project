using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Raw Material", fileName = "RawMaterial")]
public class RawMaterial : ScriptableObject
{
    public string displayName;
    public Sprite icon;
    [TextArea] public string description;

    [Header("Merchant")]
    public int baseMerchantPrice = 5;
}