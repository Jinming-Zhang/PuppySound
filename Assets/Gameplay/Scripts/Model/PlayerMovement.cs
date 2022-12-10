using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField]
    private Animator animator;

    private Dictionary<string, bool> animationState;

    private void Awake()
    {
        animationState = new Dictionary<string, bool>();
        animationState.Add("face front", true);
        animationState.Add("face back", false);
        animationState.Add("face right", false);
        animationState.Add("face left", false);
        animationState.Add("walk front", false);
        animationState.Add("walk back", false);
        animationState.Add("walk right", false);
        animationState.Add("walk left", false);

    }

    private void SetAnimationState(string name)
    {
        foreach (string i in animationState.Keys.ToList())
        {
            animationState[i] = i == name;
            if (animator.GetBool(i) != animationState[i])
            {
                animator.SetBool(i, animationState[i]);
            }
        }
    }

    public void Move()
    {
        Vector3 pos = transform.position;
        pos.x = pos.x + horizontal * speed * Time.fixedDeltaTime;
        pos.y = pos.y + vertical * speed * Time.fixedDeltaTime;
        if (IsPositionOnMaze(pos))
        {
            rb.MovePosition(pos);
        }
    }

    public void ReadInput(float speed)
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        this.speed = speed;
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

            SetAnimationState("walk left");

        }
        else if (horizontal > 0f && vertical == 0f)
        {
            facingRight = true;
            facingLeft = false;
            facingUp = false;
            facingDown = false;

            SetAnimationState("walk right");

        }
        else if (horizontal == 0f && vertical < 0f)
        {
            facingRight = false;
            facingLeft = false;
            facingUp = false;
            facingDown = true;

            SetAnimationState("walk front");

        }
        else if (horizontal == 0f && vertical > 0f)
        {
            facingRight = false;
            facingLeft = false;
            facingUp = true;
            facingDown = false;

            SetAnimationState("walk back");

        }
        else if (horizontal == 0f && vertical == 0f)
        {
            if (facingDown)
            {
                SetAnimationState("face front");
            }
            else if (facingLeft)
            {
                SetAnimationState("face left");
            }
            else if (facingRight)
            {
                SetAnimationState("face right");
            }
            else if (facingUp)
            {
                SetAnimationState("face back");
            }

        }
    }
}
