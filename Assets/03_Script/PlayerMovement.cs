using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    public delegate void OnFriendCollision(GameObject collision);
    public delegate void OnEnemyCollision(GameObject collision);
    public delegate void OnEnemyJump(GameObject collision);
    public static OnFriendCollision FriendCollisionEmitter;
    public static OnEnemyCollision EnemyCollisionEmitter;
    public static OnEnemyJump EnemyJumpEmitter;

    [SerializeField] private Joystick joystick;
    [SerializeField] private float horizontalForce;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask ground;

    private CapsuleCollider2D wheelsCollider;
    private float horizontalMove;
    private bool isJumping = false;
    private bool isHurted = false;
    private bool isFrozen = false;
    private Rigidbody2D rb;

    private Animator anim;
    private PlayerData.PlayerState currentState = PlayerData.PlayerState.Idle;

    void Start()
    {
        UIManager.OnRightBtnTouch += Jump;
        rb = GetComponent<Rigidbody2D>();
        wheelsCollider = GetComponent<CapsuleCollider2D>();
        GetAnimator();

    }

    private void GetAnimator()
    {
        // Valorizzo l'animator del component child
        Transform childTransform = transform.GetChild(0);
        if (childTransform)
        {
            GameObject child = transform.GetChild(0).gameObject;
            if (child != null)
            {
                anim = child.GetComponent<Animator>();
            }
        }
    }

    void Update()
    {
        GetHorizontalMove();

        if (Input.GetButtonDown("Jump") && IsTouchingGround())
        {
            Jump();
        }

        SetPlayerState();
    }


    private void GetHorizontalMove()
    {
        if (PersistentDataManager.Instance != null && PersistentDataManager.Instance.IsMobileDevice())
        {
            horizontalMove = joystick.Horizontal;
        }
        else
        {
            horizontalMove = Input.GetAxisRaw("Horizontal");
        }

    }

    private void SetPlayerState()
    {
        
        if (isHurted)
        {
            currentState = PlayerData.PlayerState.Idle;
        }
        else if (!IsTouchingGround())
        {
            if (rb.velocity.y < 0.1f)
            {
                currentState = PlayerData.PlayerState.Fall;
            }
            else
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
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (isHurted)
        {
            // In questo modo evito di attivare più volte 
            if(!isFrozen)
            {
                Vector3 forceDirection = transform.localScale.x > 0f ? Vector3.left : Vector3.right;
                rb.AddForce(forceDirection * jumpForce);
                isFrozen = true;
            }
        }
        else if (isJumping)
        {
            rb.AddForce(transform.up * jumpForce);
            isJumping = false;
        }
        else if (horizontalMove != 0 && IsTouchingGround() && Mathf.Abs(rb.velocity.x) < 30f)
        {

            if (horizontalMove < 0)
            {
                rb.AddForce(Vector3.left * horizontalForce);
                transform.localScale = new Vector2(-1, 1);
            }
            else
            {
                rb.AddForce(Vector3.right * horizontalForce);
                transform.localScale = new Vector2(1, 1);
            }
        }
    }

    private bool IsTouchingGround()
    {
        return wheelsCollider.IsTouchingLayers(ground);
    }

    private  IEnumerator Hurt() {
        //Debug.Log("HURT");
        isHurted = true;
        isFrozen = false;
        yield return new WaitForSeconds(1f);
        isHurted = false;
    }


    public void Jump()
    {
        //Debug.Log("JUMP");
        isJumping = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Friend")
            ManageFriendCollision(collision);
        else if(collision.gameObject.tag == "Enemy")       
            ManageEnemyCollision(collision);
    }

    private void ManageFriendCollision(Collision2D collision)
    {
        if (currentState == PlayerData.PlayerState.Fall)
        {
            Jump();
        }
        else
        {
            StartCoroutine(Hurt());
        }

        FriendCollisionEmitter?.Invoke(collision.gameObject);
    }

    private void ManageEnemyCollision(Collision2D collision)
    {
        if (currentState == PlayerData.PlayerState.Fall)
        {
            Jump();
            EnemyJumpEmitter?.Invoke(collision.gameObject);
        }
        else
        {
            StartCoroutine(Hurt());
            EnemyCollisionEmitter?.Invoke(collision.gameObject);
        }
    }

    private void OnDestroy()
    {
        UIManager.OnRightBtnTouch -= Jump;
    }
}