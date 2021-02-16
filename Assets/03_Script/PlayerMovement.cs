using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Joystick joystick;
    [SerializeField] private float velocity;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask ground;
    private float horizontalMove;
    private float verticalMove;
    private bool isJumping = false;
    private Rigidbody2D rb;
    private CapsuleCollider2D coll;
    private Animator anim;
    private int currentState = 0;
    // Start is called before the first frame update
    void Start()
    {
        UIManager.OnRightBtnTouch += Jump;
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        Transform childTransform = transform.GetChild(0);
        if (childTransform)
        {
            GameObject child = transform.GetChild(0).gameObject;
            if (child)
            {
                anim = child.GetComponent<Animator>();
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        // horizontalMove = joystick.Horizontal;
        verticalMove = joystick.Vertical;
        horizontalMove = Input.GetAxisRaw("Horizontal");
        SetPlayerState();
    }

    private void SetPlayerState()
    {
        // Jump
        if (!coll.IsTouchingLayers(ground))
        {
            if(rb.velocity.y < 0.1f)
            {
                currentState = 3;
            } else
            {
                currentState = 2;
            }

        }
        else if (Mathf.Abs(horizontalMove) > 0.1f)
        {
            currentState = 1;
        }
        else
        {
            currentState = 0;
        }

        anim.SetInteger("state", currentState);
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (isJumping)
        {

            rb.AddForce(transform.up * jumpForce);
            isJumping = false;
        }

        else if (horizontalMove != 0 && coll.IsTouchingLayers(ground))
        {

            if (horizontalMove < 0)
            {
                rb.AddForce(-transform.right * velocity);
                transform.localScale = new Vector2(-1, 1);
            }
            else
            {
                rb.AddForce(transform.right * velocity);
                transform.localScale = new Vector2(1, 1);
            }
        } 
        else
        {

        }

        // rb.velocity = new Vector2(horizontalMove * velocity, rb.velocity.y); 
    }


    public void Jump()
    {
        Debug.Log("JUMP player");
        if(coll.IsTouchingLayers(ground))
        {
            isJumping = true;
        }
    }

    private void OnDestroy()
    {
        UIManager.OnRightBtnTouch -= Jump;
    }
}
