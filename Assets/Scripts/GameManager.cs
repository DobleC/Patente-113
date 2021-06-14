using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // lol no se mostrar textos por pantalla con el otro controlador, por lo demás, sigue siendo más flexible lo de que el otro se invoque en cualquier escena
    // no me cuentes tu vida

    public GameObject climbText;
    public GameObject rechargeText;

    public int triggerNum = 0;

    public static GameManager instance
    {
        get;
        private set;
    }
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }

        else
        {
            instance = this;

        }
    }

    public Monster monster;   
    public void CheckTrigger()
    {
        ++triggerNum;

        switch (triggerNum)
        {
            case 1:
                StartHunt(Monster.State.chase); //laberinto
                monster.Appear(monster.transform.position);
                break;

            case 2:
                StartHunt(Monster.State.notAtSight);
                monster.disappearToAppear = false; //sales del laberinto
                break;
            case 3:
                
                StartHunt(Monster.State.observing); //entras en la sala bífida
                monster.Appear(monster.spawnZones[3].position);
                break;
            case 4:
                StartHunt(Monster.State.chase); //sales de ella y te sigue
                monster.Appear(monster.spawnZones[4].position);
                monster.disappearToAppear = true;
                break;
            case 5:
                StartHunt(Monster.State.notAtSight); //subes ascensor
                break;
            case 6:
                StartHunt(Monster.State.chase);
                monster.Appear(monster.spawnZones[5].position);//arriba del ascnsor
                break;

        }
    }

    private void StartHunt(Monster.State _state)
    {
        monster.state = _state;
        monster.done = false;
    }



}