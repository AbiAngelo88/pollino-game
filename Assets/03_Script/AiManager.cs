using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiManager : MonoBehaviour
{ 
    [SerializeField] private LayerMask ground;

    // Variabili che devono essere settate dall'editor per poter modificare a piacere il movimento
    [SerializeField] private float speed = 1;
    [SerializeField] private float maxDistance = 3;

    private Vector3 initialPosition;
    private Vector3 direction = Vector3.left;
    private Rigidbody2D rb;
    private float currentSpeed;
    private bool foundInitialPosition = false;
    private AI currentAI;
    private Animator anim;
    private AI.AiState currentState = AI.AiState.Idle;

    public AI GetAI()
    {
        return currentAI;
    }

    public void SetAI(AI data)
    {
        currentAI = data;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GetAnimator();

        if (currentAI.GetCanFly())
        {
            rb.isKinematic = true;
            foundInitialPosition = true;
            StartCoroutine(GetDirection());
        }

    }

    private void Update()
    {
        SetAiState();
    }

    private void SetAiState()
    {
        if(rb.velocity.x == 0f)
        {
            currentState = AI.AiState.Idle;
        }
        else
        {
            currentState = AI.AiState.Run;
        }

        anim.SetInteger("state", currentState.GetHashCode());
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

    private IEnumerator GetDirection()
    {

        currentSpeed = speed;
        while (foundInitialPosition)
        {
            yield return new WaitForFixedUpdate();

            if (transform.localPosition.x >= (initialPosition.x + maxDistance))
            {
                currentSpeed = 0f;
                yield return new WaitForSeconds(2f);
                direction = Vector3.left;
                transform.localScale = new Vector3(1, 1, 1);
                currentSpeed = speed;
            }
            else if (transform.localPosition.x <= (initialPosition.x - maxDistance))
            {
                currentSpeed = 0f;
                yield return new WaitForSeconds(2f);
                direction = Vector3.right;
                transform.localScale = new Vector3(-1, 1, 1);
                currentSpeed = speed;

            }
        }
    }

    private void FixedUpdate()
    {

        if (foundInitialPosition)
            transform.Translate(direction * currentSpeed * Time.deltaTime, Space.World);
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!foundInitialPosition)
        {
            if (rb.IsTouchingLayers(ground))
            {
                initialPosition = transform.localPosition;
                foundInitialPosition = true;
                StartCoroutine(GetDirection());
            }
        }
    }

}
