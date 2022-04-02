using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "TradeData", menuName = "ScriptableObjects/TradeManagerSO", order = 1)]
public class ItemData : ScriptableObject
{
    public string ItemName;
    public Sprite ItemImage;
}
