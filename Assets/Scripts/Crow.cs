
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Скрипт для управления поведением врага, который летает, преследует героя и атакует его
public class Crow : MonoBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField] private Transform pointA;  // Точка A для патрулирования
    [SerializeField] private Transform pointB;  // Точка B для патрулирования
    [SerializeField] private float patrolSpeed = 3f;  // Скорость движения при патрулировании

    [Header("Chase Settings")]
    [SerializeField] private float detectionRadius = 5f;  // Радиус обнаружения героя
    [SerializeField] private float chaseSpeed = 5f;  // Скорость движения при преследовании

    [Header("Attack Settings")]
    [SerializeField] private float attackRate = 1f;  // Частота атак (раз в секунду)

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 3;  // Максимальное количество жизней
    private int currentHealth;  // Текущее количество жизней

    private Transform player;  // Ссылка на героя
    private Vector3 currentTarget;  // Текущая цель для патрулирования
    private bool isChasing = false;  // Флаг состояния преследования
    private float lastAttackTime = 0f;  // Время последней атаки
    private SpriteRenderer spriteRenderer;  // Компонент для управления отображением спрайта
    private Animator animator;  // Компонент анимации
    private bool hasReachedPointAOnce = false;  // Флаг для первого достижения точки A
    private bool isDead = false;  // Флаг состояния смерти

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>()?.transform;  // Поиск героя в сцене
        currentTarget = pointA.position;  // Установка первой цели для патрулирования (точка А)
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();  // Получаем компонент SpriteRenderer
        animator = GetComponent<Animator>();  // Получаем компонент Animator
        currentHealth = maxHealth;  // Устанавливаем здоровье на максимум
    }

    private void Update()
    {
        // Если враг мертв, то пропускаем все дальнейшие действия
        if (isDead) return;

        // Если текущее здоровье больше нуля, активируем анимацию полета
        animator.SetBool("flying", true);

        // Если враг находится в режиме преследования, выполняем преследование героя
        if (isChasing)
        {
            ChasePlayer();
        }
        // Если ворона не преследует героя, выполняем патрулирование и проверку на наличие героя
        else
        {
            Patrol();
            CheckForPlayer();
        }
    }

    // Метод для патрулирования между двумя точками
    private void Patrol()
    {
        // Проверка направления к текущей цели и настройка поворота
        if (currentTarget.x > transform.position.x)
        {
            spriteRenderer.flipX = false;  // Враг смотрит вправо, если цель справа
        }
        else
        {
            spriteRenderer.flipX = true;  // Враг смотрит влево, если цель слева
        }

        // Двигаем врага к текущей цели
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, patrolSpeed * Time.deltaTime);

        // Меняем цель, если враг достиг одной из точек
        if (Vector3.Distance(transform.position, currentTarget) < 0.1f)
        {
            // Проверяем, достигла ли ворона точки A
            if (currentTarget == pointA.position)
            {
                // Если точка A уже была достигнута ранее, переворачиваем спрайт
                if (hasReachedPointAOnce)
                {
                    spriteRenderer.flipX = !spriteRenderer.flipX;
                }
                // Если это первый раз, когда ворона достигает точки A, устанавливаем флаг
                else
                {
                    hasReachedPointAOnce = true;
                }
            }
            // Если текущая цель — точка B, также переворачиваем спрайт
            else
            {
                spriteRenderer.flipX = !spriteRenderer.flipX;
            }
            // Обновляем цель на противоположную точку
            currentTarget = currentTarget == pointA.position ? pointB.position : pointA.position;
        }
    }

    // Метод для преследования героя
    private void ChasePlayer()
    {
        // Проверяем, что игрок существует, прежде чем продолжить преследование
        if (player == null) return;

        // Проверка направления к герою и настройка поворота
        if (player.position.x > transform.position.x)
        {
            spriteRenderer.flipX = false;  // Враг смотрит вправо, если герой справа
        }
        else
        {
            spriteRenderer.flipX = true;  // Враг смотрит влево, если герой слева
        }

        // Двигаем врага в сторону героя
        transform.position = Vector3.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);

        // Проверяем дистанцию до героя, чтобы атаковать
        if (Vector3.Distance(transform.position, player.position) < 2f)
        {
            Attack();
        }

        // Если герой ушел из зоны обнаружения, враг возвращается к патрулированию
        if (Vector3.Distance(transform.position, player.position) > detectionRadius || player.position.y >= transform.position.y)
        {
            isChasing = false;  // Выключаем режим преследования
            currentTarget = pointA.position;  // Устанавливаем начальную точку патрулирования
        }
    }

    // Метод для проверки, находится ли герой в радиусе обнаружения и ниже вороны по оси Y
    private void CheckForPlayer()
    {
        // Проверяем, что игрок существует и находится в радиусе обнаружения и под врагом
        if (player && Vector3.Distance(transform.position, player.position) <= detectionRadius && player.position.y < transform.position.y)
        {
            // Активируем режим преследования
            isChasing = true;
        }
    }

    // Метод атаки героя
    private void Attack()
    {
        // Проверяем, прошел ли достаточный интервал времени для следующей атаки
        if (Time.time - lastAttackTime >= attackRate)
        {
            lastAttackTime = Time.time;  // Обновляем время последней атаки

            if (PlayerMovement.Instance != null) // Если экземпляр героя существует
            {
                Debug.Log("Атака героя, оставшиеся жизни: ");
                PlayerMovement.Instance.TakeDamage(3);  // Наносим урон герою
                PlayerMovement.Instance.PlayHeroAnimation("Hurt");
            }
        }
    }

    // Метод, вызываемый при столкновении с другими объектами
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Проверяем, если столкновение с объектом Faceless или Crow
        if (collision.gameObject.CompareTag("Faceless") || collision.gameObject.CompareTag("Crow"))
        {
            // Игнорируем столкновение
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);
        }
    }

    // Метод для получения урона
    public void GetDamage(int damage)
    {
        PlayerMovement.Instance.PlayHeroAnimation("Damage");
        currentHealth -= damage;  // Уменьшаем текущее здоровье на 1
        Debug.Log("Оставшиеся жизни Crow: " + currentHealth);

        // Если жизни закончились, вызываем метод смерти
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Метод для уничтожения объекта при смерти
    private void Die()
    {
        isDead = true;  // Установка флага состояния смерти
        animator.SetBool("flying", false);  // Отключение анимации полета
        animator.SetTrigger("death"); // Триггер анимации смерти
        Destroy(gameObject, 1f); // Удаление объекта через 1 секунду после анимации
    }
}
