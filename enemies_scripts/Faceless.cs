using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Скрипт для управления врагом 'faceless', который может перемещаться, наносить урон и умирать
public class Faceless : MonoBehaviour
{
    [Header("Faceless Settings")]
    [SerializeField] private float speed = 3.5f;  // Скорость передвижения
    [SerializeField] private float maxDistance = 5f;  // Максимальное расстояние до разворота
    [SerializeField] private int maxHealth = 5;  // Максимальное количество жизней
    
    private int damage = 1;  // Урон, который получает faceless
    private int currentHealth;  // Текущее количество жизней
    private Vector3 dir;  // Направление движения
    private SpriteRenderer sprite;  // Компонент для управления отображением спрайта
    private Animator animator;  // Компонент анимации
    private Vector3 leftBoundary;  // Левая граница движения
    private Vector3 rightBoundary;  // Правая граница движения
    private bool boundariesSet = false;  // Флаг для разворота при столкновении с объектами
    private bool returnToStart = false;  // Флаг возврата к начальной попзиции
    private bool isDead = false;  // Флаг состояния смерти

    private void Start()
    {
        dir = transform.right;  // Начальное направление движения (вправо)
        sprite = GetComponentInChildren<SpriteRenderer>();  // Получаем компонент SpriteRenderer
        animator = GetComponentInChildren<Animator>();  // Получаем компонент Animator
        leftBoundary = transform.position;  // Начальная позиция как левая граница движения
        currentHealth = maxHealth;  // Устанавливаем текущие жизни на максимум
    }

    private void Update()
    {
        // Если объект не мертв
        if (!isDead)
        {
            Move();  // Вызываем метод движения в каждом кадре
        }
    }

    // Метод для перемещения и смены направления
    private void Move()
    {
        // Если границы ещё не установлены, устанавливаем правую границу
        if (!boundariesSet)
        {
            // Рассчитываем потенциальную правую границу, отступив на заданное расстояние (maxDistance) от левой границы в направлении движения
            Vector3 potentialRightBoundary = leftBoundary + dir * maxDistance;
            // Проверяем, находится ли объект на расстоянии, равном или превышающем maxDistance от левой границы
            if (Vector3.Distance(transform.position, leftBoundary) >= maxDistance)
            {
                rightBoundary = transform.position;  // Фиксируем правую границу
                boundariesSet = true;  // Отмечаем, что границы установлены
            }
        }

        // Проверка на возврат к начальной позиции
        if (returnToStart)
        {
            // Перемещение к начальной позиции (leftBoundary) с заданной скоростью
            transform.position = Vector3.MoveTowards(transform.position, leftBoundary, speed * Time.deltaTime);

            // Проверка достижения начальной позиции
            if (transform.position == leftBoundary)
            {
                returnToStart = false;  // Отключение возврата
                dir = transform.right;  // Установка направления движения вправо
                sprite.flipX = false;  // Установка отображения спрайта без разворота
            }
        }
        else
        {
            // Двигаем объект в текущем направлении
            transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);

            // Если объект достиг одной из границ, меняем направление
            if (boundariesSet && (transform.position.x <= leftBoundary.x || transform.position.x >= rightBoundary.x))
            {
                ReverseDirection();
            }
        }

        // Устанавливаем анимацию ходьбы в true
        animator.SetBool("walk", true);
    }
    
    // Метод, вызываемый при столкновении с другим объектом
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Проверяем, если столкнулись с героем
        if (collision.gameObject == PlayerMovement.Instance.gameObject)
        {
            // Герой получает урон
            PlayerMovement.Instance.TakeDamage(damage);

            // Разворачиваемся после нанесения урона герою
            ReverseDirection();
        }

        // Проверяем, если столкновение с объектом Faceless или Crow
        else if (collision.gameObject.CompareTag("Crow") || collision.gameObject.CompareTag("Faceless")) 
        {
            // Игнорируем столкновение
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);
        }

        // Проверяем, если столкновение с объектом Saw
        else if ((collision.gameObject.CompareTag("Saw")))
        {
            // Разворачиваемся
            ReverseDirection();
        }
    }

    // Метод для смены направления и поворота спрайта
    private void ReverseDirection()
    {
        dir *= -1f;  // Меняем направление
        sprite.flipX = !sprite.flipX;  // Поворачиваем спрайт по оси X
    }

    // Метод для получения урона
    public void GetDamage(int damaage)
    {
        if (isDead) return;  // Прекращаем выполнение, если объект уже мёртв
        
        currentHealth -= damaage;  // Уменьшаем количество жизней

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
        animator.SetBool("walk", false);  // Отключаем анимацию ходьбы
        animator.SetTrigger("death");  // Устанавливаем триггер анимации смерти
        Destroy(gameObject, 1f);  // Удаление объекта через 1 секунду после срабатывания анимации
    }
}
