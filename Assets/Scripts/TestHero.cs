using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHero : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        // Получаем компонент Animator, прикрепленный к герою
        animator = GetComponent<Animator>();

        // Запускаем анимацию "Idle" сразу при старте
        if (animator != null)
        {
            animator.Play("Idle");
        }
    }
}
