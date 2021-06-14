using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    //Ahora que tengo tu atención, nuevos sonidos que imagino que irán asociados al enemigo:
    // Romper cristal:                                                                                              SoundManager.PlaySound(SoundManager.Sound.BrokenGlass, transform.position);
    // Encontrarse al enemigo:                                                                                      SoundManager.PlaySound(SoundManager.Sound.EnemEncounter, transform.position);
    // Chirrido (este no es del enemigo pero meh, ya lo pongo aquí):                                                SoundManager.PlaySound(SoundManager.Sound.Screech, transform.position);
    // Perseguir al player (este se repite cada 7s, así que lo puedes poner en una función que vaya en el update):  SoundManager.PlaySound(SoundManager.Sound.ChasingPlayer, transform.position);
    // Si borras las position se reproducirán en 2D (se oirán iwal independientemente de la posición)


    //[SerializeField]
    //[Range(2, 8)]
    //private float maxSpeed;
    //[SerializeField]
    //[Range(0, 5)]
    //private float dmgSpeed;


    public enum State { chase, notAtSight,  observing};
    public State state;

    public bool done = false;

    [SerializeField]
    private Transform target;
    private Player targetScript;

    private NavMeshAgent agent;
    private Animator animator;

 
    public Transform[] spawnZones; //las 3 primeras son rnd, la 3 es sala bifida, la 4 escaleras, la 5 arriba
    public Transform despawnPoint;

    public bool disappearToAppear = true;

    [SerializeField]
    private float attackTimer;
    private bool canAttack = true;
    private bool hitted = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        state = Monster.State.observing;
       
    }
    // Start is called before the first frame update
    void Start()
    {
        targetScript = target.gameObject.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.observing && !done) Stay();
        else if (state == State.notAtSight && !done) StartCoroutine(Disappear());
        else if (state == State.chase)
        {
            float distanceTarget = Vector3.Distance(transform.position, target.position);

            if (distanceTarget > agent.stoppingDistance)
                Chase();
            else if(canAttack && distanceTarget <= agent.stoppingDistance && !hitted)
                Atacar();
        }       
        
    }

    private void Chase()
    {
        if (agent.isOnNavMesh) 
        {
            SoundManager.PlaySound(SoundManager.Sound.ChasingPlayer, transform.position);
            agent.SetDestination(target.position);
        }


    }

    private void GetHit() //cambia vel, ejecuta animación de hit
    {
        if(!hitted) SoundManager.PlaySound(SoundManager.Sound.EnemyHitted);
        hitted = true;

        StartCoroutine(Disappear());

        //anim dmg
    }
  
    private void Atacar()
    {
        targetScript.GetDamage();

        StartCoroutine(attCor());
        //reproducir sonido

        //animación atacar
    }

    IEnumerator attCor()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackTimer);
        canAttack = true;
    }

    public IEnumerator Disappear()
    {
        Stay();
        yield return new WaitForSeconds(0.5f);        
        transform.position = despawnPoint.position;

        yield return new WaitForSeconds(5);
        if (disappearToAppear)
        {
            if (GameManager.instance.triggerNum <= 1)
                Appear(rndPos());
            else if (GameManager.instance.triggerNum == 4) Appear(spawnZones[4].position);
            else if (GameManager.instance.triggerNum == 5) Appear(spawnZones[5].position);
        }
        hitted = false;
    }

    private void Stay()
    {

            agent.isStopped = true;
        agent.enabled = false;
        done = true;
    }

    public void Appear(Vector3 pos) //Para que aparezca en los puntos q nos interesa
    {
        transform.position = pos;
        agent.enabled = true;
        agent.isStopped = false;
    }

    public void RayTargetHit() //la función que llama la weapon
    {
        GetHit();
    }

    private Vector3 rndPos()
    {
        return spawnZones[Random.Range(0, 3)].position; 
    }


}
