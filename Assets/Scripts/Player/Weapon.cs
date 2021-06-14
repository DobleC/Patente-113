using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;



public class Weapon : MonoBehaviour
{

    [SerializeField] private bool weaponObtained = false;
    [SerializeField] private int maxEnergy = 3;
    [SerializeField] private int energy = 3;
    [SerializeField] private int numVueltas = 13; // Muy importante, el numero de vueltas siempre tiene que ser 4 * n + 1 (siendo n la cantidad de giros de 360º que quieres) Ej: 13 = 3 vueltas (4*3+1)
    [SerializeField] private float shootLongitude = 25f;

    [SerializeField] private float luzMaxArma = 0.7f; // Intensidades de la luz que emite el player
    [SerializeField] private float luzMinArma = 0.3f;

    public bool canShoot = true;
    private bool visorOn = true;
    private int failShoots = 0;
    private int vueltasDadas = 0;
    private bool rechargeTextReaded = false;

    private ParticleSystem shootPartycle;
    private FirstPersonController firstPersonController;
    private CharacterController characterController;
    [SerializeField] private Animator anim;

    [SerializeField] private Material visorMaterial;
    [SerializeField] private GameObject visor;
    [SerializeField] private Light weaponLight;


    private void Start()
    {
        this.gameObject.SetActive(weaponObtained);
        //visorMaterial = visor.GetComponent<Material>();
        visor.SetActive(true);
        visorMaterial.color = new Color32(255, 255, 255, 255);


    }

    private void Awake()
    {
        //anim = GetComponent<Animator>();
        characterController = GetComponentInParent<CharacterController>();
        firstPersonController = GetComponentInParent<FirstPersonController>();
        shootPartycle = transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
        shootPartycle.Stop();
    }

 

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // He cambiado GetMouseButton por GetMouseButtonDown para evitar que se perdiese más de 1 de energia por disparo
        {
            if (canShoot) Shoot();
           // else if (!GameManager.instance.rechargeText.activeSelf) rememberTutorial();
        } 
        
        if (!canShoot && !visorOn) StartCoroutine(Recharge());
        else StopCoroutine(Recharge());
    }

    private void Shoot()
    {
        //play sonido disparar
        SoundManager.PlaySound(SoundManager.Sound.ElectricShoot);
        //SoundManager.PlaySound(SoundManager.Sound.EnemyHitted);

        //play camers shake 

        Vector3 diagonalDch = Vector3.Normalize((Camera.main.transform.forward + Camera.main.transform.right)) ; // Angulo de 45 grados hacia la derecha del disparo
        Vector3 diagonalIzq = Vector3.Normalize((Camera.main.transform.forward + -Camera.main.transform.right)) ; // Angulo de 45 grados hacia la izquierda del disparo
        Vector3 diagonalIzq2 = Vector3.Normalize((Camera.main.transform.forward + diagonalIzq)); //Angulo entre medias
        Vector3 diagonalDch2 = Vector3.Normalize((Camera.main.transform.forward + diagonalDch)); //Angulo entre medias

        //Dibujar los Raycast en el editor durante timeDrawingRaycast segundos

        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootLongitude, Color.green, 10); // Dibujo de rayo hacia delante
        Debug.DrawRay(Camera.main.transform.position, diagonalDch * shootLongitude, Color.green, 10);                   // Dibujo de rayo hacia delante-dcha
        Debug.DrawRay(Camera.main.transform.position, diagonalIzq * shootLongitude, Color.green, 10);
        Debug.DrawRay(Camera.main.transform.position, diagonalIzq2 * shootLongitude, Color.green, 10);
        Debug.DrawRay(Camera.main.transform.position, diagonalDch2 * shootLongitude, Color.green, 10);

        //Disparar los Raycast


        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, shootLongitude))
        { // Dispara los 3 raycast (efecto de escopeta / cono)
            Debug.Log(hit.collider);
            hit.collider.SendMessage("RayTargetHit", SendMessageOptions.DontRequireReceiver);
            
            // Invoca a "public void RayTargetHit()" si esta se encuentra en algún script del gameobject contra el que colisionan los RayCast
        }
        if(Physics.Raycast(Camera.main.transform.position, diagonalDch, out RaycastHit hit1, shootLongitude)){
            Debug.Log(hit1.collider);
            hit1.collider.SendMessage("RayTargetHit", SendMessageOptions.DontRequireReceiver);
        }
        if (Physics.Raycast(Camera.main.transform.position, diagonalIzq, out RaycastHit hit2, shootLongitude))
        {
            Debug.Log(hit2.collider);
            hit2.collider.SendMessage("RayTargetHit", SendMessageOptions.DontRequireReceiver);
        }
        if (Physics.Raycast(Camera.main.transform.position, diagonalIzq2, out RaycastHit hit3, shootLongitude))
        {
            Debug.Log(hit3.collider);
            hit3.collider.SendMessage("RayTargetHit", SendMessageOptions.DontRequireReceiver);
        }
        if (Physics.Raycast(Camera.main.transform.position, diagonalDch2, out RaycastHit hit4, shootLongitude))
        {
            Debug.Log(hit4.collider);
            hit4.collider.SendMessage("RayTargetHit", SendMessageOptions.DontRequireReceiver);
        }



        shootPartycle.Play();
        --energy;

        if (energy < 1) TurnOff();
    }

    private void TurnOff()
    {
       
        StartCoroutine(turnOffVisor()); // Consigue un parpadeo al apagar el visor en vez de que se apague y ya
        canShoot = false;
        weaponLight.intensity = luzMinArma;
        //Apagar pistola y visor
    }

    private IEnumerator turnOffVisor()
    {
        //SoundManager.PlaySound(SoundManager.Sound.TurnOffWeapon);
        yield return new WaitForSeconds(0.1f);
        visorMaterial.color = new Color32(255, 255, 255, 191);
        yield return new WaitForSeconds(0.1f);
        visorMaterial.color = new Color32(255, 255, 255, 127);
        yield return new WaitForSeconds(0.1f);
        visorMaterial.color = new Color32(255, 255, 255, 63);
        yield return new WaitForSeconds(0.1f);
        visorMaterial.color = new Color32(255, 255, 255, 0);
        yield return new WaitForSeconds(0.05f);
        visor.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        SoundManager.PlaySound(SoundManager.Sound.TurnOffWeapon);


        if (!rechargeTextReaded)
        {
            GameManager.instance.rechargeText.SetActive(true);
            rechargeTextReaded = true;
        }

        visorOn = false;
    }

    private IEnumerator turnOnVisor()
    {
        visorMaterial.color = new Color32(255, 255, 255, 0);
        visor.SetActive(true);

        yield return new WaitForSeconds(0.1f);
        visorMaterial.color = new Color32(255, 255, 255, 63);
        yield return new WaitForSeconds(0.1f);
        visorMaterial.color = new Color32(255, 255, 255, 91);
        yield return new WaitForSeconds(0.1f);
        visorMaterial.color = new Color32(255, 255, 255, 127);
        yield return new WaitForSeconds(0.1f);
        visorMaterial.color = new Color32(255, 255, 255, 255);
        yield return new WaitForSeconds(0.05f);
        StopCoroutine(turnOnVisor());
    }

    private void TurnOn()
    {
        vueltasDadas = 0;

        

        StartCoroutine(turnOnVisor());
        weaponLight.intensity = luzMaxArma;

        energy = maxEnergy;
        canShoot = true;
        visorOn = true;
    }
    private IEnumerator Recharge()
    {
        StopCoroutine(turnOffVisor());

        if (Input.GetKeyDown(KeyCode.R)) // Cambiar por recarga de manivela o wasd
        {
            GameManager.instance.rechargeText.SetActive(false);

            if (vueltasDadas == 0)
            {
                anim.SetBool("isRecharging", true);
                ++vueltasDadas;
               
            }
            else
            {
                SoundManager.PlaySound(SoundManager.Sound.Click);
                anim.SetTrigger("rightRound");
                ++vueltasDadas;

                if (vueltasDadas == numVueltas)
                {
                    anim.SetBool("isRecharging", false);
                  
                    
                    yield return new WaitForSeconds(0.75f);
                    TurnOn();
                }
            }


            
        }
    }
    private void rememberTutorial()
    {
        ++failShoots;
        if (failShoots > 4)
        {
            GameManager.instance.rechargeText.SetActive(true);
            failShoots = 0;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(Camera.main.transform.position,Camera.main.transform.forward * shootLongitude);
    }

    public void Activate()
    {
        //SoundManager.PlaySound(SoundManager.Sound.ElectricShoot);
        this.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        //SoundManager.PlaySound(SoundManager.Sound.ElectricShoot);
        this.gameObject.SetActive(false);
    }

    public void SetCanShoot(bool algo)
    {
        if(algo!=false && energy!=0 && canShoot==false)
            canShoot = algo;
    }
}
