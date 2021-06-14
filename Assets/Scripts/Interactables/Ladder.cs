using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{

    private void OnTriggerStay(Collider other) // Sube al pulsar F
    {
        //Debug.Log(other.name);
        if (other.CompareTag("Player"))
        {


            //if(Input.GetKeyDown(KeyCode.F))
            other.GetComponent<Player>().ClimbLadder();
        }
    }
    private void OnTriggerEnter(Collider other)  //pop up pulsar F
    {
        if (other.CompareTag("Player"))
        {
            //GameManager.instance.climbText.SetActive (true);  /
        }
    }
    private void OnTriggerExit(Collider other) // Al salir se quita el pop up y deja de subir
    {
        if (other.CompareTag("Player"))
        {
            //GameManager.instance.climbText.SetActive (false);

            StartCoroutine(other.GetComponent<Player>().FinishClimb());

        }
    }
}
