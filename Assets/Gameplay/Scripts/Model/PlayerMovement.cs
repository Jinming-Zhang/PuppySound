using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    private float speed = 2f;
    private bool facingRight = false;
    private bool facingLeft = false;
    private bool facingDown = false;
    private bool facingUp = true;

    [SerializeField] private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        updateFacing();
    }

    private void FixedUpdate()
    {
        if (facingLeft || facingRight)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        } else if (facingUp || facingDown)
        {
            rb.velocity = new Vector2(rb.velocity.x, vertical * speed);
        } else
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }
        
    }

    private void updateFacing()
    {
        if (horizontal < 0f && vertical == 0f)
        {
            facingRight = false;
            facingLeft = true;
            facingUp = false;
            facingDown = false;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;

        } else if (horizontal > 0f && vertical == 0f)
        {
            facingRight = true;
            facingLeft = false;
            facingUp = false;
            facingDown = false;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;

        } else if (horizontal == 0f && vertical < 0f)
        {
            facingRight = false;
            facingLeft = false;
            facingUp = false;
            facingDown = true;
            Vector3 localScale = transform.localScale;
            localScale.y *= -1f;
            transform.localScale = localScale;

        } else if (horizontal == 0f && vertical > 0f)
        {
            facingRight = false;
            facingLeft = false;
            facingUp = true;
            facingDown = false;
            Vector3 localScale = transform.localScale;
            localScale.y *= -1f;
            transform.localScale = localScale;

        }
    }
}
