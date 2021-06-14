using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen = false;
    [SerializeField] private int numLights;
     private int lightsEnabled = -1;

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        CheckLights();
    }

    public void CheckLights() // Comprueba si todas las luces que son necesarias para abrir la puerta están encendidas y de ser así, la abre
    {
        ++lightsEnabled; // ++ a las luces encendidas por el player ya que esta función solo se llama cuando una luz es encendida
        if (!isOpen && lightsEnabled == numLights) Open();
    }
    public void Open()
    {
        SoundManager.PlaySound(SoundManager.Sound.OpenDoor, transform.position);
        isOpen = true;
        animator.SetBool("opening", isOpen);
    }

    public void Close()
    {
        isOpen = false;
        animator.SetBool("opening", isOpen);
    }
}
