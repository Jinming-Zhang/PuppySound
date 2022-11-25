using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    private float speed;
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
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

    }

    private void updateFacing()
    {
        if (facingRight && horizontal < 0f)
        {
            facingRight = false;
            facingLeft = true;
            facingU
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;

        } else if (!facingRight && horizontal > 0f)
        {
            facingRight = false;
            facingLeft = true;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;

        } else if (facingUp && vertical < 0f)
        {
            facing = false;
            facingLeft = true;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;

        }
    }
}
