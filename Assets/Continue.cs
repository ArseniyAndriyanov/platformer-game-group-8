using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Continue : MonoBehaviour
{
    public GameObject panel;
    public GameObject ContinueButton;
    public GameObject ExitButton;
    public GameObject GoToMenuButton;

    public void continuegame(){
        Time.timeScale = 1f;
        panel.SetActive(false);
        ContinueButton.SetActive(false);
        ExitButton.SetActive(false);
        GoToMenuButton.SetActive(false);
    }
}
