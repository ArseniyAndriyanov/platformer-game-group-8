using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 2f; // Скорость движения
    public float runSpeed = 6f; // Скорость бега
    public float jumpForce = 10f; // Сила прыжка
    public int attackDamage = 1; // Урон от атаки
    public float attackRange = 2f; // Дальность атаки
    public int maxHealth = 100; // Максимальное здоровье
    private int currentHealth; // Текущее здоровье
    private bool isGrounded; // Состояние "на земле"

    private Rigidbody2D rb; // Компонент Rigidbody2D
    public Animator animator; // Аниматор игрока

    public static PlayerMovement Instance { get; set; } // Singleton для доступа к игроку

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Получаем Rigidbody2D
        animator = GetComponent<Animator>(); // Получаем аниматор
        currentHealth = maxHealth; // Устанавливаем текущее здоровье

        if (Instance == null)
        {
            Instance = this; // Устанавливаем экземпляр игрока
        }
        else
        {
            Destroy(gameObject); // Удаляем дубликат
        }
    }

    void Update()
    {
        Move(); // Вызываем метод движения
        Jump(); // Вызываем метод прыжка
        Attack(); // Вызываем метод атаки
    }

    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal"); // Получаем ввод по горизонтали
        float currentSpeed = moveSpeed; // Устанавливаем скорость по умолчанию

        // Проверяем, удерживается ли Shift для бега
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentSpeed = runSpeed; // Устанавливаем скорость бега
            animator.SetBool("isRunning", true); // Анимация бега
        }
        else
        {
            animator.SetBool("isRunning", false); // Анимация обычного движения
        }

        // Устанавливаем скорость Rigidbody
        rb.velocity = new Vector2(moveInput * currentSpeed, rb.velocity.y);
        animator.SetFloat("Speed", Mathf.Abs(moveInput)); // Обновляем анимацию по скорости

        // Устанавливаем направление взгляда игрока
        if (moveInput != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y); // Останавливаемся
            animator.SetFloat("Speed", 0); // Сбрасываем анимацию скорости
        }
    }

    void Jump()
    {
        // Прыжок, если игрок на земле и нажата кнопка прыжка
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // Прыжковая сила
            isGrounded = false; // Игрок в воздухе
            animator.SetBool("Jump", true); // Анимация прыжка
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Проверка на столкновение с землёй
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true; // Игрок на земле
        animator.SetBool("Jump", false); // Сброс анимации прыжка

        // Получение урона при столкновении с врагом
        if (collision.gameObject.CompareTag("Enemy"))
            TakeDamage(10);
    }

    void Attack()
    {
        // Атака при нажатии кнопки атаки
        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Attack"); // Анимация атаки
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange); // Проверка вокруг на предмет врагов

            foreach (var hit in hits)
            {
                // Если попали в врага, передаем ему урон
                if (hit.CompareTag("Enemy"))
                {
                    // Для спец врагов 
                    if (hit.TryGetComponent<Faceless>(out var faceless))
                    {
                        faceless.GetDamage(attackDamage); // Получение урона
                    }
                    else if (hit.TryGetComponent<Crow>(out var crow))
                    {
                        crow.GetDamage(attackDamage);
                    }
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Уменьшение здоровья
        animator.SetTrigger("TakeDamage"); // Анимация получения урона
        if (currentHealth <= 0)
            Die(); // Погибаем, если здоровье упало до 0
    }

    void Die()
    {
        animator.SetBool("Die", true); // Анимация смерти
        Destroy(gameObject); // Удаление игрока
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name); // Перезагрузка уровня
    }

    public void AddForce(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse); // Применение силы к игроку
    }
}
