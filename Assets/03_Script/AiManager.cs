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
    private bool isHurtedPlayer = false;
    private bool isSaved = false;

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
        LevelManager.SaveAIEmitter += OnSaveAI;
        LevelManager.HurtedPlayerEmitter += OnHurtedPlayer;
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
        if(ai.gameObject.GetInstanceID() == gameObject.GetInstanceID())
        {
            currentSpeed = 0f;
            isDestroying = true;
            Collider2D[] colliders = gameObject.GetComponentsInChildren<Collider2D>();
            foreach(Collider2D coll in colliders)
            {
                Destroy(coll);
            }
            // Attendiamo per la durata dell'animazione che dovrebbe essere di circa un secondo
            Destroy(gameObject, 1f);
        }
    }

    private void OnHurtedPlayer(GameObject ai)
    {
        if (ai.gameObject.GetInstanceID() == gameObject.GetInstanceID())
            StartCoroutine(HurtPlayer());
    }

    private IEnumerator HurtPlayer()
    {
        currentSpeed = 0f;
        isHurtedPlayer = true;
        yield return new WaitForSeconds(.4f);
        isHurtedPlayer = false;
    }

    private void OnSaveAI(GameObject ai)
    {
        // Debug.Log("SAVE " + ai.name + " tra 0.5 secondo");
        if (ai.gameObject.GetInstanceID() == gameObject.GetInstanceID())
        {
            currentSpeed = 0f;
            isSaved = true;
            // Attendiamo per la durata dell'animazione che dovrebbe essere di circa un secondo
            Destroy(gameObject, 0.5f);
        }
    }

    private void SetAiState()
    {
        if(anim == null)
            return;

        if (isSaved)
        {
            currentState = AI.AiState.Save;
        }
        else if (isDestroying)
        {
            currentState = AI.AiState.Destroy;
        }
        else if (isHurtedPlayer)
        {
            currentState = AI.AiState.Hurted;
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

                if (anim == null)
                    Debug.Log("No animator for " + gameObject.name);
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

        if (foundInitialPosition && currentAI.GetCanMove())
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

                if (currentAI.GetCanMove())
                    StartCoroutine(GetDirection());
                else
                    currentSpeed = 0f;
            }
        }
    }

    private void OnDestroy()
    {
        LevelManager.DestroyAIEmitter -= OnDestroyAI;
        LevelManager.SaveAIEmitter -= OnSaveAI;
        LevelManager.HurtedPlayerEmitter -= OnHurtedPlayer;
    }

}
