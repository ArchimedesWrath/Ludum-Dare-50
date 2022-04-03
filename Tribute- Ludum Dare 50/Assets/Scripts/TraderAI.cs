using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TraderAI : MonoBehaviour
{
    private Rigidbody2D rb2d;

    public GameObject TradeUI;
    public string InputItem;
    public GameObject OutputObject;
    Vector2 tradeOffset = new Vector2(0f, 1.1f);

    [SerializeField] bool moving = true;
    [SerializeField] bool checkingLedge = true;
    [SerializeField] bool checkingWall = true;
    [SerializeField] bool showingTrade = false;
    [SerializeField] bool canTrade = true;

    [SerializeField] int movingTimer;
    [SerializeField] int waitTimer;
    [SerializeField] int tradeTimer;

    Vector3 InitialPos;


    float direction = 1f;
    float speed;
    
    // Ledge Checking
    Vector2 groundOffsetCheck = new Vector2(0.3f, -0.35f);
    Vector2 groundCheckPosition;
    float groundCheckDistance = 0.5f;

    // Wall Checkign
    Vector2 wallCheckPosition;
    float wallCheckDistance = 0.5f;
    Vector2 heading;

    private void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        speed = Random.Range(2f, 6f);
        movingTimer = Random.Range(30, 500);
        waitTimer = Random.Range(150, 300);

        InitialPos = gameObject.transform.position;

    }

    private void FixedUpdate()
    {
        CheckAction();
        CheckLedge();
        CheckWall();


        if (moving) rb2d.velocity = new Vector2(speed, rb2d.velocity.y);

        if (TradeUI)
        {
            TradeUI.transform.position = new Vector2(transform.position.x + tradeOffset.x, transform.position.y + tradeOffset.y);
            TradeUI.gameObject.SetActive(showingTrade);
        }
    }

    private void CheckAction()
    {
        if (moving) movingTimer -= 1;
        if (!moving) waitTimer -= 1;
        
        if (showingTrade) tradeTimer -= 1;

        if (tradeTimer <= 0) showingTrade = false;

        if (moving && movingTimer <= 0)
        {
            moving = false;
            checkingLedge = true;
            waitTimer = Random.Range(30, 500);
            if (waitTimer % 2 == 0) checkingLedge = false;
            if (!showingTrade && waitTimer % 4 == 0) StartTrade();
        }
        if (!moving && waitTimer <= 0)
        {
            moving = true;
            movingTimer = Random.Range(150, 300);
            speed = Random.Range(2f, 6f) * Mathf.Sign(speed);
            if (movingTimer % 2 == 0) ChangeDirection();
            if (!showingTrade && waitTimer % 4 == 0) StartTrade();
        }
    }

    private void StartTrade()
    {
        showingTrade = true;
        tradeTimer = Random.Range(150, 400);
    }
    
    private void CheckWall()
    {
        if (!checkingWall) return;
        wallCheckPosition = new Vector2(transform.position.x * direction, transform.position.y);

        heading = new Vector2(speed / Mathf.Abs(speed), 0f);
        RaycastHit2D hitInfo = Physics2D.Raycast(wallCheckPosition, heading, wallCheckDistance);

        if (hitInfo.collider != null && hitInfo.collider.name != "Player" && hitInfo.collider.name != "Trader") ChangeDirection();

    }

    private void CheckLedge()
    {
        if (!checkingLedge) return;
        groundCheckPosition = new Vector2(transform.position.x * direction + groundOffsetCheck.x, 
            transform.position.y + groundOffsetCheck.y);

        RaycastHit2D hitInfo = Physics2D.Raycast(groundCheckPosition, Vector2.down, groundCheckDistance);

        if (hitInfo.collider == null) ChangeDirection();
    }

    private void ChangeDirection()
    {
        speed *= -1;
        groundOffsetCheck.x *= -1;
        transform.localScale = new Vector2(Mathf.Sign(speed), 1);
    }

    private void MakeTrade(Item item)
    {
        Destroy(item.gameObject);
        GameObject spawnedItem = Instantiate(OutputObject, transform.position, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(groundCheckPosition, Vector2.down * groundCheckDistance);
        Gizmos.DrawRay(wallCheckPosition, heading * wallCheckDistance);
    }

    public void Respawn()
    {
        gameObject.transform.position = InitialPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            Item item = collision.gameObject.transform.parent.gameObject.GetComponent<Item>();
            if (item.Name == InputItem)
            {
                MakeTrade(item);
            }
        }
    }
}
