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
    private bool isDestroying = false;

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
        LevelManager.DestroyAIEmitter += OnDestroyAI;
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

    private void OnDestroyAI(GameObject ai)
    {
        // Debug.Log("Destroy " + ai.name + " tra 0.5 secondo");
        if(ai.gameObject.name == gameObject.name)
        {
            currentSpeed = 0f;
            isDestroying = true;
            currentState = AI.AiState.Jump;
            // Attendiamo per la durata dell'animazione che dovrebbe essere di circa un secondo
            Destroy(gameObject, 0.5f);
        }

    }

    private void SetAiState()
    {
        if (isDestroying)
        {
            currentState = AI.AiState.Jump;
        }
        else if (currentSpeed > 0.1f)
        {
            currentState = AI.AiState.Run;
        }
        else
        {
            currentState = AI.AiState.Idle;
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
        while (foundInitialPosition && !isDestroying)
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

    private void OnDestroy()
    {
        LevelManager.DestroyAIEmitter -= OnDestroyAI;
    }

}
