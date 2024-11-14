using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBar;
    public PlayerMovement player;

    void Start()
    {
        healthBar = GetComponent<Image>();
        player = FindObjectOfType<PlayerMovement>();
    }

    
    void Update()
    {
        healthBar.fillAmount = player.currentHealth / player.maxHealth;
    }
}
