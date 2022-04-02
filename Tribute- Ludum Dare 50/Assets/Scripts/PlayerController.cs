using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float speed = 10f;
    float jumpHeight = 20f;
    float throwForce = 4f;
    bool facingRight = true;

    // Raycast for ground check
    Vector2 direction = Vector2.down;
    float distance = 1.0f;
    RaycastHit2D groundHit;
    [SerializeField] private LayerMask groundLayer;

    Rigidbody2D rb2d;
    SpriteRenderer playerSprite;

    public GameObject Item;

    private void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        playerSprite = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        GetInput();
        if (Item) Item.transform.position = gameObject.transform.position;
    }

    private void GetInput()
    {
        float input = Input.GetAxisRaw("Horizontal");

        if (input < 0)
        {
            rb2d.velocity = new Vector2(input * speed, rb2d.velocity.y);
            if (facingRight) Flip();
        } else if (input > 0)
        {
            rb2d.velocity = new Vector2(input * speed, rb2d.velocity.y);
            if (!facingRight) Flip();
        } else if (input == 0)
        {
            rb2d.velocity = new Vector2(0f, rb2d.velocity.y);
        }

        if (Input.GetAxisRaw("Vertical") > 0) Jump();
        if (Input.GetKeyDown(KeyCode.E)) ThrowItem();
    }

    private bool IsGrounded()
    {
        groundHit = Physics2D.Raycast(transform.position, direction, distance, groundLayer);

        if (groundHit.collider != null)
            return true;

        return false; 
    }

    private void Jump()
    {
        if (!IsGrounded())
            return;
        else
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpHeight);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        playerSprite.flipX = !playerSprite.flipX;
    }

    private void ThrowItem()
    {
        if (!Item) return;
        Vector2 vel = new Vector2(facingRight ? rb2d.velocity.x / 2 + throwForce : rb2d.velocity.x / 2 - throwForce, 2.5f);
        Rigidbody2D itemRb = Item.GetComponent<Rigidbody2D>();
        itemRb.velocity = vel;
        itemRb.isKinematic = false;
        Item = null;

    }

    private void PickupItem(GameObject item)
    {
        Item = item;
        item.GetComponent<Rigidbody2D>().isKinematic = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Item")
        {
            if(!Item) PickupItem(collision.gameObject.transform.parent.gameObject);
        }
    }
}
