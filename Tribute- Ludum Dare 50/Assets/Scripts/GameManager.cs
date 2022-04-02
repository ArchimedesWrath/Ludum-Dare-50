using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instnace;
    public static GameManager Instance { get { return instnace; } }

    public GameObject WorldUI;
    public GameObject TradeUIPrefab;

    public TraderAI[] Traders;
    public List<ItemData> Items = new List<ItemData>();

    private void Awake()
    {
        if (instnace != null && instnace != this)
        {
            Destroy(this.gameObject);
            Debug.Log("Game Manager was destroyed");
            return;
        }
        else
        {
            instnace = this;
        }
    }

    private void Start()
    {
        Traders = FindObjectsOfType<TraderAI>();

        foreach (TraderAI trader in Traders)
        {
            if (trader.TradeUI) return;

            // Instantiate UI if trader doens't have one
            GameObject tradeUI = Instantiate(TradeUIPrefab, WorldUI.transform);
            // Set InputItem
            TradeUI tradeUIScript = tradeUI.GetComponent<TradeUI>();

            ItemData input = Items[Random.Range(0, Items.Count)];
            tradeUIScript.TradeInputSprite.sprite = input.ItemImage;

            // Make new list that excludes InputItem
            List<ItemData> newItems = Items;
            newItems.Remove(input);
            // Set Output Item 
            ItemData output = newItems[Random.Range(0, newItems.Count)];
            tradeUIScript.TradeOuputSprite.sprite = output.ItemImage;

            trader.TradeUI = tradeUI;
            trader.InputItem = input.ItemName;
        }
    }
}
