using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float runSpeed = 6f;
    public float jumpForce = 10f;
    public int attackDamage = 1;
    public float attackRange = 2f;
    public float maxHealth = 100f;
    public float currentHealth;
    private bool isGrounded;

    private Rigidbody2D rb;
    public Animator animator;

    public static PlayerMovement Instance { get; set; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = 100f;

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        animator.SetBool("TakeDamage", false);
        Move();
        Jump();
        Attack();

    }

    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");

        float currentSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentSpeed = runSpeed;
            animator.SetBool("isRunning", true); // ���������� ��� 
        }
        else
        {
            animator.SetBool("isRunning", false); // ������������ ��� 
        }

        rb.velocity = new Vector2(moveInput * currentSpeed, rb.velocity.y);

        animator.SetFloat("Speed", Mathf.Abs(moveInput)); // ��������� �������� 

        if (moveInput != 0)
        {
            rb.velocity = new Vector2(moveInput * currentSpeed, rb.velocity.y);
            animator.SetFloat("Speed", Mathf.Abs(moveInput));
            transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            animator.SetFloat("Speed", 0);
        }

    }

    void Jump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
            animator.SetBool("Jump", true);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
        animator.SetBool("Jump", false);

        if (collision.gameObject.CompareTag("Enemy"))
            TakeDamage(10f);
    }

    void Attack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Attack");
            animator.Play("Damage");
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Enemy"))
                {
                    if (hit.TryGetComponent<Faceless>(out var faceless))
                    {
                        faceless.GetDamage(attackDamage);
                    }
                    else if (hit.TryGetComponent<Crow>(out var crow))
                    {
                        crow.GetDamage(attackDamage);
                    }
                }
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("TakeDamage");
        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        animator.SetBool("Die", true);
        Destroy(gameObject);

        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void AddForce(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void PlayHeroAnimation(string animationName)
    {
        animator.Play(animationName);
    }
}