using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiManager : MonoBehaviour
{ 
    [SerializeField] private float initialSpeed;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float maxDistance;

    private float speed;
    private Vector3 initialPosition;
    private Vector3 direction = Vector3.left;
    private Rigidbody2D rb;
    private bool foundInitialPosition = false;
    


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private IEnumerator GetDirection()
    {

        speed = initialSpeed;
        while (foundInitialPosition)
        {
            yield return new WaitForFixedUpdate();

            if (transform.localPosition.x >= (initialPosition.x + maxDistance))
            {
                direction = Vector3.left;
                transform.localScale = new Vector3(-1, 1, 1);
                speed = 0f;
                yield return new WaitForSeconds(2f);
                speed = initialSpeed;
            }
            else if (transform.localPosition.x <= (initialPosition.x - maxDistance))
            {
                direction = Vector3.right;
                transform.localScale = new Vector3(1, 1, 1);
                speed = 0f;
                yield return new WaitForSeconds(2f);
                speed = initialSpeed;

            } else
            {
               
            }
        }
    }

    private void FixedUpdate()
    {

        if (foundInitialPosition)
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        
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
