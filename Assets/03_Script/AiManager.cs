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

        if (currentAI.GetCanFly())
        {
            rb.isKinematic = true;
            foundInitialPosition = true;
            StartCoroutine(GetDirection());
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
