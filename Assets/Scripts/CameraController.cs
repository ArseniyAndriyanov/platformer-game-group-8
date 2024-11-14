using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Скрипт для управления положением камеры, которая следует за игроком
public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;  // Ссылка на игрока
    private Vector3 pos;  // Переменная для хранения позиции, к которой будет стремиться камера

    private void Awake()
    {
        // Проверяем, задан ли объект игрока, если нет — находим его автоматически
        if (!player)
            player = FindObjectOfType<hero>().transform;
    }

    private void Update()
    {
        pos = player.position;  // Получаем текущую позицию игрока
        pos.z = -10f;  // Устанавливаем фиксированное значение Z, чтобы камера всегда находилась позади игрока
        pos.y += 3f;  // Поднимаем камеру по оси Y, чтобы лучше видеть игрока
        
        // Плавно перемещаем камеру к новой позиции для создания эффекта следования
        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime);
    }
}
