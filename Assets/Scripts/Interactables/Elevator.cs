using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Transform finalPos;
    public GameObject invisibleWall;
    public float elevatorTime;
    private Door door;

    [SerializeField] GameObject armaPuesta;

    private bool hasGone = false;

    private void Start()
    {
        door = transform.GetChild(0).GetComponent<Door>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasGone && other.CompareTag("Player")) GoingUp(other.transform);
    }

    private void GoingUp(Transform player)
    {
        Destroy(this.GetComponent<Collider>());

        door.Close();
        invisibleWall.SetActive(true);
        
        
        
        StartCoroutine(GoingUpCoroutine(player));
       
        //podemos poner un headbob

    }

    IEnumerator GoingUpCoroutine(Transform player)
    {
        

        yield return new WaitForSeconds(elevatorTime);

        player.SetParent(this.transform);
        Vector3 pos = player.localPosition;
        player.GetComponent<CharacterController>().enabled = false;

        transform.position = finalPos.position;
        transform.rotation = finalPos.rotation;

        player.localPosition = pos;

        //yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.01f);
        player.GetComponent<CharacterController>().enabled = true;

        invisibleWall.SetActive(false);
        player.SetParent(null);

        Destroy(armaPuesta);

        door.Open();

    }
}
