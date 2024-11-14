using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject panel;
    public GameObject ContinueButton;
    public GameObject ExitButton;
    public GameObject GoToMenuButton;

    public void pause(){
        panel.SetActive(true);
        ContinueButton.SetActive(true);
        ExitButton.SetActive(true);
        GoToMenuButton.SetActive(true);
        Time.timeScale = 0;
    }
}
