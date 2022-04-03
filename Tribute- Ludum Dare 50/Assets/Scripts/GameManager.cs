using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager instnace;
    public static GameManager Instance { get { return instnace; } }

    public GameObject WorldUI;
    public GameObject TradeUIPrefab;
    public GameObject Altar;
    public TextMeshProUGUI CountDownText;
    public Image ReqImage;

    public DialogueTrigger NPC;

    public int DaysPassed = 0;

    private int countDownTimer;

    public bool GamePaused = false;
    public bool InDialogue = false;
    private bool firstDay = true;

    public TraderAI[] Traders;
    public List<ItemData> Items = new List<ItemData>();

    private List<ItemData> tradeableItems = new List<ItemData>();

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
        NPC.TriggerDialogue();
        RestartDay();
        firstDay = false;
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !InDialogue) 
            GamePaused = !GamePaused;
        IncTimer();
    }

    private void RestartDay()
    {
        ClearItems();
        InitTraders();
        StartRound();
    }

    IEnumerator GiveTask(ItemData itemData)
    {
        yield return new WaitForSeconds(1f);

        NPC.TriggerTask(itemData);
        GamePaused = false;
    } 

    private void ClearItems()
    {
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("Item")) Destroy(item);
    }
    private void StartRound()
    {
        countDownTimer = 3600;
        ItemData chosenItem = tradeableItems[Random.Range(0, tradeableItems.Count)];
        Altar.GetComponent<Altar>().DesiredItem = chosenItem;
        ReqImage.sprite = chosenItem.ItemImage;

        if (!firstDay) StartCoroutine(GiveTask(chosenItem));
    }

    private void InitTraders()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("HUD")) Destroy(obj);

        Traders = FindObjectsOfType<TraderAI>();

        GameObject[] spawns = GameObject.FindGameObjectsWithTag("SpawnPoint");

        foreach (TraderAI trader in Traders) trader.gameObject.transform.position = spawns[Random.Range(0, spawns.Length)].transform.position;

        ItemData nextInput = Items[Random.Range(0, Items.Count)];
        Instantiate(nextInput.ItemGameObject, new Vector3(8.5f, 0.5f, 0f), Quaternion.identity);

        // Make new list that excludes InputItem
        List<ItemData> oldItems = new List<ItemData>();
        List<ItemData> newItems;

        foreach (ItemData item in Items) oldItems.Add(item);

        foreach (TraderAI trader in Traders)
        {
            // Clear out newItem list
            newItems = new List<ItemData>();

            // Instantiate UI for each Trader
            GameObject tradeUI = Instantiate(TradeUIPrefab, WorldUI.transform);
            TradeUI tradeUIScript = tradeUI.GetComponent<TradeUI>();

            foreach (ItemData item in oldItems) newItems.Add(item);
            newItems.Remove(nextInput);

            // Set Output Item 
            ItemData output = newItems[Random.Range(0, newItems.Count)];
            
            // TODO: Not needed?
            tradeableItems.Add(output);

            tradeUIScript.TradeInputSprite.sprite = nextInput.ItemImage;
            tradeUIScript.TradeOuputSprite.sprite = output.ItemImage;
            trader.TradeUI = tradeUI;
            trader.InputItem = nextInput.ItemName;
            trader.OutputObject = output.ItemGameObject;

            nextInput = output;
            oldItems = newItems;
        }
    }

    private void IncTimer()
    {

        if (!GamePaused && !InDialogue) countDownTimer -= 1;
        CountDownText.text = Mathf.Round(countDownTimer / 60).ToString();
        if (countDownTimer <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        GamePaused = true;
        
        // Bring up final score
        // Transition to end scene? 
    }

    public void WinDay()
    {
        DaysPassed += 1;
        RestartDay();
    }

}
