using UnityEngine;

public class Door : MonoBehaviour, Interactable
{
    public void Interact()
    {
        GetComponent<Animator>().SetTrigger("Open");
    }
}