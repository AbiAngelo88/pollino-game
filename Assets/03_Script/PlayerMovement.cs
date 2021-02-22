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
    [SerializeField] private GameObject frontWheel;
    [SerializeField] private GameObject backWheel;


    private float horizontalMove;
    private bool isJumping = false;
    private Rigidbody2D rb;
    private PolygonCollider2D coll;
    private CapsuleCollider2D triggerCollider;
    private CircleCollider2D frontWheelCollider;
    private CircleCollider2D backWheelCollider;

    private Animator anim;
    private PlayerData.PlayerState currentState = PlayerData.PlayerState.Idle;
    private float zRotation;
    private bool isCrashed = false;

    private float distToGroundFrontWheel;
    private float distToGroundBackWheel;

    void Start()
    {
        UIManager.OnRightBtnTouch += Jump;
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<PolygonCollider2D>();
        triggerCollider = GetComponent<CapsuleCollider2D>();
        frontWheelCollider = frontWheel.GetComponent<CircleCollider2D>();
        distToGroundFrontWheel = frontWheelCollider.bounds.extents.y;
        backWheelCollider = backWheel.GetComponent<CircleCollider2D>();
        distToGroundBackWheel = backWheelCollider.bounds.extents.y;

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
        else if (!IsTouchingGround() && !triggerCollider.IsTouchingLayers(ground))
        {
            if(rb.velocity.y < 0.1f)
            {
                currentState = PlayerData.PlayerState.Fall;
                Debug.Log("FAAAAAAAAAAAAAAAAAAAAAAAAAAAAAALL!");
              
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
        // Debug.Log("CURRENT STATE " + currentState);
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

        else if (horizontalMove != 0 && IsTouchingGround())
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

    private bool IsTouchingGround()
    {
        return (backWheelCollider.IsTouchingLayers(ground) || frontWheelCollider.IsTouchingLayers(ground)) ;
        //return Physics.Raycast(backWheelCollider.transform.position, -Vector3.up, distToGroundBackWheel) || Physics.Raycast(frontWheelCollider.transform.position, -Vector3.up, distToGroundFrontWheel);
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
        if(IsTouchingGround())
        {
            isJumping = true;
        }
    }

    private void OnDestroy()
    {
        UIManager.OnRightBtnTouch -= Jump;
    }
}
