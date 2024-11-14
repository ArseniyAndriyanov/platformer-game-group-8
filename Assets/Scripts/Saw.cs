// Tag - Saw
// Layer - Enenmy
// Pixels per unit - 100
// Body type - kinematic
// Freeze position - x, y
// Animation: saw - every 3s

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Скрипт для управлением препятсвия (лезвия), наносящего урон и отталкивающего при этом игрока
public class Saw : MonoBehaviour
{
    [Header("Saw Settings")]
    [SerializeField] private float knockbackForce = 5f;  // Сила отталкивания, которая будет применена к игроку при столкновении

    // Метод, вызываемый при столкновении с другим объектом
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        // Проверка, является ли объект, с которым произошло столкновение, героем
        if (collision.gameObject == PlayerMovement.Instance.gameObject)
        {
            // Вызываем метод получения урона у героя
            PlayerMovement.Instance.TakeDamage(1);

            // Получаем направление отталкивания
            Vector2 knockbackDirection = (PlayerMovement.Instance.transform.position - transform.position).normalized;

            // Применяем силу отталкивания к герою
            PlayerMovement.Instance.AddForce(knockbackDirection * knockbackForce);
        }
    }
}
