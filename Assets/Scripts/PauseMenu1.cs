using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused = false; 
    
    void Update() 
    { 
        if (Input.GetKeyDown(KeyCode.Escape)) 
        { 
            if (isPaused) 
            { 
                Resume(); 
            } 
            else 
            { 
                Pause(); 
            } 
        } 
    } 
    
    public void Resume() 
    { 
        Time.timeScale = 1f; 
        isPaused = false; 
    } 
    
    void Pause() 
    { 
        Time.timeScale = 0f; 
        isPaused = true; 
    } 
}
