using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorButton : MonoBehaviour
{
    [SerializeField] GameObject armaPuesta;
    bool unaVez = false;

    public void DejarArma()
    {
        if (!unaVez)
        {
            armaPuesta.SetActive(true);
            unaVez = true;
        }
        
    }
}
