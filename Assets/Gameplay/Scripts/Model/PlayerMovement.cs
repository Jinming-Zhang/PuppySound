using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    WalkableChecker checker;
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

    public void Move()
    {
        //rb.velocity = new Vector2(horizontal * speed, vertical * speed);
        Vector3 pos = transform.position;
        pos.x = pos.x + horizontal * speed * Time.fixedDeltaTime;
        pos.y = pos.y + vertical * speed * Time.fixedDeltaTime;
        if (IsPositionOnMaze(pos))
        {
            Debug.Log("on board");
            rb.MovePosition(pos);
        }
        else
        {
            Debug.Log("off board");
        }
    }

    public void ReadInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        updateFacing();
    }

    bool IsPositionOnMaze(Vector3 position)
    {
        return checker.IsOnBoard(position - transform.position);
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

        }
        else if (horizontal > 0f && vertical == 0f)
        {
            facingRight = true;
            facingLeft = false;
            facingUp = false;
            facingDown = false;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;

        }
        else if (horizontal == 0f && vertical < 0f)
        {
            facingRight = false;
            facingLeft = false;
            facingUp = false;
            facingDown = true;
            Vector3 localScale = transform.localScale;
            localScale.y *= -1f;
            transform.localScale = localScale;

        }
        else if (horizontal == 0f && vertical > 0f)
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
