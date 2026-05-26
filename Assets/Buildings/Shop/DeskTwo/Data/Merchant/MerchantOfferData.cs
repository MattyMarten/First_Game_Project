using UnityEngine;

[CreateAssetMenu(fileName = "NewMerchantOffer", menuName = "Shop/Desk 2/Merchant Offer")]
public class MerchantOfferData : ScriptableObject
{
    [Header("Info")]
    public string offerId;
    public string offerTitle;

    [TextArea(3, 6)]
    public string description;

    [Header("Offer")]
    public UtilityCraftable utilityItem;
    public int price = 25;
}