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
    [SerializeField] private LayerMask ground;
    [SerializeField] private float horizontalForce = 44f;
    [SerializeField] private float rotationalForce = 2000f;
    [SerializeField] private float jumpForce = 1000f;
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float jumpOnAIForce = 800f;

    private CapsuleCollider2D wheelsTriggerCollider;
    private CircleCollider2D backWheelCollider;
    private CircleCollider2D frontWheelCollider;
    private float horizontalMove;
    private bool isJumping = false;
    private bool isJumpingOnAI = false;
    private bool isHurtedEnemy = false;
    private bool isHurtedFriend = false;
    private bool isFrozen = false;
    private Rigidbody2D rb;

    private Animator anim;
    private PlayerData.PlayerState currentState = PlayerData.PlayerState.Idle;

    void Start()
    {
        UIManager.OnRightBtnTouch += Jump;
        rb = GetComponent<Rigidbody2D>();
        GetColliders();
        GetAnimator();

    }

    private void GetColliders()
    {
        wheelsTriggerCollider = GetComponent<CapsuleCollider2D>();
        CircleCollider2D[] circleColliders = gameObject.GetComponentsInChildren<CircleCollider2D>();
        if(circleColliders != null && circleColliders.Length > 0)
        {
            backWheelCollider = circleColliders[0];
            frontWheelCollider = circleColliders[1];
        }
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
        
        if (isHurtedEnemy)
        {
            currentState = PlayerData.PlayerState.Hurted_Enemy;
        }
        else if (isHurtedFriend)
        {
            currentState = PlayerData.PlayerState.Hurted_Friend;
        }
        else if (!IsTouchingGround())
        {
            // Caduta
            if (rb.velocity.y < 0.1f)
            {
                if (horizontalMove > 0)
                {
                     currentState = gameObject.transform.localScale.x > 0 ? PlayerData.PlayerState.Jump_Front : PlayerData.PlayerState.Jump_Back;
                    
                }
                else if (horizontalMove < 0)
                {
                    currentState = gameObject.transform.localScale.x > 0 ? PlayerData.PlayerState.Jump_Back : PlayerData.PlayerState.Jump_Front;
                }
                else
                {
                    currentState = PlayerData.PlayerState.Fall;
                }
            }
            else
            {
                // Stacco
                if(horizontalMove > 0)
                {
                    currentState = gameObject.transform.localScale.x > 0 ? PlayerData.PlayerState.Jump_Front : PlayerData.PlayerState.Jump_Back;
                }
                else if(horizontalMove < 0)
                {
                    currentState = gameObject.transform.localScale.x > 0 ? PlayerData.PlayerState.Jump_Back : PlayerData.PlayerState.Jump_Front;
                }
                else
                {
                    currentState = PlayerData.PlayerState.Jump;
                }
                
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

    private void RotatePlayer()
    {
        if (horizontalMove == 0f)
            return;

        rb.AddTorque(-rotationalForce * horizontalMove * Time.deltaTime);
    }


    private void MovePlayer()
    {
        if (isHurtedEnemy || isHurtedFriend)
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
        else if (isJumpingOnAI)
        {
            rb.AddForce(transform.up * jumpOnAIForce);
            isJumpingOnAI = false;
        }
        else if (horizontalMove != 0 && IsTouchingGround() && Mathf.Abs(rb.velocity.x) < maxSpeed && backWheelCollider.IsTouchingLayers(ground))
        {
            AudioHelper.PlayOneShotSound(AudioHelper.Sounds.Fargo);
            if (horizontalMove < 0)
            {
                rb.AddForce(Vector2.left * horizontalForce);

                if(rb.velocity.x < 0f)
                    transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                rb.AddForce(Vector2.right * horizontalForce);

                if (rb.velocity.x > 0f)
                    transform.localScale = new Vector3(1, 1, 1);
            }
        }
        else if (!IsTouchingGround())
        {
            RotatePlayer();
        }
    }

    private bool IsTouchingGround()
    {
        return wheelsTriggerCollider.IsTouchingLayers(ground);
    }

    private  IEnumerator OnEnemyHurted() {
        //Debug.Log("HURT");
        isHurtedEnemy = true;
        isFrozen = false;
        yield return new WaitForSeconds(.5f);
        isHurtedEnemy = false;
    }

    private IEnumerator OnFriendHurted()
    {
        //Debug.Log("HURT");
        isHurtedFriend = true;
        isFrozen = false;
        yield return new WaitForSeconds(.5f);
        isHurtedFriend = false;
    }

    public void Jump()
    {
        //Debug.Log("JUMP");
        isJumping = true;
    }

    public void JumpOnAI()
    {
        //Debug.Log("JUMP");
        isJumpingOnAI = true;
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
        if (!IsTouchingGround() && rb.velocity.y < 0.1f)
        {
            JumpOnAI();
        }
        else
        {
            StartCoroutine(OnFriendHurted());
        }

        FriendCollisionEmitter?.Invoke(collision.gameObject);
    }

    private void ManageEnemyCollision(Collision2D collision)
    {
        if (!IsTouchingGround() && rb.velocity.y < 0.1f)
        {
            JumpOnAI();
            EnemyJumpEmitter?.Invoke(collision.gameObject);
        }
        else
        {
            StartCoroutine(OnEnemyHurted());
            EnemyCollisionEmitter?.Invoke(collision.gameObject);
        }
    }

    private void OnDestroy()
    {
        UIManager.OnRightBtnTouch -= Jump;
    }
}