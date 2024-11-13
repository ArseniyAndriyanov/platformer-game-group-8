using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpHight;

    private Rigidbody2D rb;

    private int money;
    private Interactable interactableObj;

    private bool grounded;

    public bool useKeyBoard;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            Move();
        }

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            InteractWithObj();
        }
    }

    private void Jump()
    {
        grounded = false;

        rb.velocity = new Vector2(0, jumpHight);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Interactable")
        {
            interactableObj = collision.gameObject.GetComponent<Interactable>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Interactable")
        {
            interactableObj = null;
        }
    }

    public void InteractWithObj()
    {
        if (interactableObj != null)
        {
            interactableObj.Interact();
        }
    }

    private void Move()
    {
        float direction = Input.GetAxis("Horizontal");

        if (direction > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (direction < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

        float step = speed * Time.deltaTime;

        transform.position = Vector2.MoveTowards(transform.position,
            new Vector2(transform.position.x + direction * step, transform.position.y), step);
    }

}