using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public delegate void PickedCollectable(GameObject collectable);
    public static event PickedCollectable PickedCollectableEmitter;
    [SerializeField] private Joystick joystick;
    [SerializeField] private float horizontalForce;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask ground;
    private float horizontalMove;
    private bool isJumping = false;
    private Rigidbody2D rb;
    private CapsuleCollider2D coll;
    private Animator anim;
    private PlayerData.PlayerState currentState = PlayerData.PlayerState.Idle;
    private float zRotation;
    private bool isCrashed = false;
    
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

    
    void Update()
    {
        horizontalMove = joystick.Horizontal;
        // horizontalMove = Input.GetAxisRaw("Horizontal");
        if(Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        zRotation = transform.rotation.eulerAngles.z;
        SetPlayerState();
    }

    private void SetPlayerState()
    {
        // Jump
        if (zRotation >= 90 && zRotation <= 270 || isCrashed)
        {
            isCrashed = true;
            currentState = PlayerData.PlayerState.Crash;
        }
        else if (!coll.IsTouchingLayers(ground))
        {
            if(rb.velocity.y < 0.1f)
            {
                currentState = PlayerData.PlayerState.Fall;
            } else
            {
                currentState = PlayerData.PlayerState.Jump;
            }

        }
        else if (Mathf.Abs(horizontalMove) > 0.1f)
        {
            currentState = PlayerData.PlayerState.Run;
        }
        else
        {
            currentState = PlayerData.PlayerState.Idle;
        }

        anim.SetInteger("state", currentState.GetHashCode());
    }

    private void FixedUpdate()
    {
        if(!isCrashed)
        {
            MovePlayer();
        }
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
                rb.AddForce(-transform.right * horizontalForce);
                transform.localScale = new Vector2(-1, 1);
            }
            else
            {
                rb.AddForce(transform.right * horizontalForce);
                transform.localScale = new Vector2(1, 1);
            }
        } 
        else
        {

        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Collectable")
        {
            PickedCollectableEmitter?.Invoke(collision.gameObject);
            Debug.Log("Collected");
        }
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
