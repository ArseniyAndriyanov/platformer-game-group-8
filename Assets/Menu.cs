using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void PlayGame()
    {
        PlayerPrefs.SetInt("currentLevel", SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("currentLevel"))
        {
            SceneManager.LoadScene(PlayerPrefs.GetInt("currentLevel"));
        }
    }
    public void ExitGame()
    {
        Debug.Log("Игра закрылась");
        Application.Quit();
    }
}