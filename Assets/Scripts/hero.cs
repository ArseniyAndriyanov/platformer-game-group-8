using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hero : Entity
{
    [SerializeField] private float speed = 3f; // скорость движения
    [SerializeField] private int lives = 5; // количество жизней
    [SerializeField] private float jumpForce = 15f; // сила прыжка
    private bool isGrounded = false;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    public static hero Instance { get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        if (Instance == null)
        {
            Instance = this; // Устанавливаем текущий объект в качестве Instance
        }
        else
        {
            Destroy(gameObject); // Удаляем объект, если Instance уже установлен
        }
    }

    private void FixedUpdate() 
    {
        CheckGround();
    }
    
    private void Update()
    {
        if (Input.GetButton("Horizontal"))
            Run();
        
        if (isGrounded && Input.GetButtonDown("Jump"))
            Jump();
    }
    
    private void Run()
    {
        Vector3 dir = transform.right * Input.GetAxis("Horizontal");

        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);

        sprite.flipX = dir.x < 0.0f;
    }

    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void CheckGround()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 0.3f);
        isGrounded = collider.Length > 1;
    }

    public override void GetDamage()
    {
        lives -= 1;
        Debug.Log(lives);
    }

    public void AddForce(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }
}
