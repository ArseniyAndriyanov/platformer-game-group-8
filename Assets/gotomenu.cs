using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gotomenu : MonoBehaviour
{
    public void menu(){
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1f;
    }
}
