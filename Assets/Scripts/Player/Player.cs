using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;



public class Player : MonoBehaviour
{

    //Vida(2 toques) : cuando le pegan -vel, 
    //viñeta rgba(255,0,0,0) => (255, 0, 0, 100)
    //Vel max, velHit -50%


    //Acciones:
    //Subir arma(el arma se mueve, pistola.subirarma)
    //Hit(animacion dañado, sprite ui, velHit, cameraShake)
    //Subir escalera(subir arma, animacion subir escalera, desactiva controles hasta que estes arriba)
    //Interactuar con el botón ascensor


   [SerializeField] private int maxLife = 3;
    private int currentLife = 0;
    [SerializeField] private int secondsToRecoverLife;

    [SerializeField] private Weapon weapon;
    private FirstPersonController firstPersonController;
    private Animator animator;

    [SerializeField] private GameObject marte;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private GameObject deathText;
    private Image screenEnd;
    [SerializeField] private GameObject creditText;
    bool creditos = false;

    [SerializeField] private GameObject CC;
    private Text ccText;
    int c = 0;
    int cc = 0;
    bool check = false;
    


    public  Vector3 initialPos;
    public  Quaternion rotation;


    private void Awake()
    {
        marte.SetActive(false);
        firstPersonController = gameObject.GetComponent<FirstPersonController>();
        animator = gameObject.GetComponent<Animator>();
        screenEnd = endScreen.GetComponent<Image>();
        currentLife = maxLife;
        setBlood(0);
        StartCoroutine(spawnSound());
        ccText = CC.GetComponent<Text>();
    }


    public void GetDamage()
    {
        StopCoroutine(playerRegen());

        --currentLife;
        SoundManager.PlaySound(SoundManager.Sound.PlayerGetHit);
        //SoundManager.PlaySound(SoundManager.Sound.EnemEncounter);

        

        switch (currentLife) 
        {
            case 2: setBlood(128);
                firstPersonController.m_WalkSpeed = 6f;
                break;

            case 1: setBlood(255, 170);
                firstPersonController.m_WalkSpeed = 4f;
                break;

            case 0:
                StartCoroutine(playerDeath());
                break;
        }

        StartCoroutine(playerRegen());
        //animación get hit
        //-vel, mirar como lo hacemos para q conecte con la speed del player controller
    }

    public RawImage bloodUI;
    void setBlood(byte alpha, byte red = 135)
    {
        bloodUI.color = new Color32(red, 0, 0, alpha);
    }

    void stopPlayer()
    {
        firstPersonController.m_WalkSpeed = 0;
        firstPersonController.canMove = false;
        weapon.canShoot = false;
    }

    private IEnumerator playerDeath()
    {
        stopPlayer();

        animator.SetTrigger("ceMurio");

        screenEnd.color = new Color32(0, 0, 0, 0);
        deathScreen.SetActive(true);
        deathText.SetActive(false);

        yield return new WaitForSeconds(0.4f);
        screenEnd.color = new Color32(0, 0, 0, 63);
        yield return new WaitForSeconds(0.4f);
        screenEnd.color = new Color32(0, 0, 0, 91);
        yield return new WaitForSeconds(0.4f);
        screenEnd.color = new Color32(0, 0, 0, 127);
        yield return new WaitForSeconds(0.4f);
        screenEnd.color = new Color32(0, 0, 0, 255);
        yield return new WaitForSeconds(0.2f);
        deathText.SetActive(true);

        yield return new WaitForSeconds(3.0f);
        GameAssets.instance.ReloadScene();
    }

    private IEnumerator End()
    {
        yield return new WaitForSeconds(10.0f);
        stopPlayer();

        
        screenEnd.color = new Color32(255, 55, 66, 0);
        endScreen.SetActive(true);

        yield return new WaitForSeconds(0.3f);
        screenEnd.color = new Color32(255, 55, 66, 63);
        yield return new WaitForSeconds(0.3f);
        screenEnd.color = new Color32(255, 55, 66, 91);
        yield return new WaitForSeconds(0.3f);
        screenEnd.color = new Color32(255, 55, 66, 127);
        yield return new WaitForSeconds(0.3f);
        screenEnd.color = new Color32(255, 55, 66, 200); 
        yield return new WaitForSeconds(0.3f);
        screenEnd.color = new Color32(255, 55, 66, 255);
      
        
        creditos = true;
    }

    private void Update()
    {
        if (creditos)
        {
            creditText.SetActive(true);
            creditText.transform.Translate(Vector3.up * 150 * Time.deltaTime);

            if(creditText.transform.localPosition.y > 180) GameAssets.instance.Exit();
        }
        #region CC
        if (Input.GetKeyDown(KeyCode.C))
        {
            ++c;
        }
        if (c > 29 && cc < 300)
        {
            CC.SetActive(true);
            ccText.color = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);
            ++cc;
        }
        else
        {
            CC.SetActive(false);       
        }
        #endregion

    }

    private IEnumerator playerRegen()
    {
        if (currentLife == 1)
        {
            yield return new WaitForSeconds(secondsToRecoverLife - 5);
            if(currentLife > 1) yield break;
            ++currentLife;
            setBlood(128);
            firstPersonController.m_WalkSpeed = 6f;
        }

        if (currentLife == 2)
        {
            yield return new WaitForSeconds(secondsToRecoverLife);
            if(currentLife < 2) yield break;
            ++currentLife;
            setBlood(0);
            firstPersonController.m_WalkSpeed = 8f;
        }
       
    }

    private IEnumerator spawnSound()
    {
        yield return new WaitForSeconds(0f);
        SoundManager.PlaySound(SoundManager.Sound.Spawn, transform.position);
        StopCoroutine(spawnSound());
    }

    public void ClimbLadder() // Ya no funciona por tiempo, simplemente empieza a escalar al pulsar F y no para hasta Finishclimb()
    {
        firstPersonController.finishingClimbing = false;
        firstPersonController.climbing = true;
        firstPersonController.canMove = false;
    }
    
    public IEnumerator FinishClimb() //Cuando sale de la escalera (se llama dentro de ladder.cs) deja de escalar
    {
        firstPersonController.finishingClimbing = true;

        yield return new WaitForSeconds(0.2f);
        firstPersonController.canMove = true;
        firstPersonController.climbing = false;
    }
    bool unaVez = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CogerArma")
        {
            SoundManager.PlaySound(SoundManager.Sound.LightOn);
            Destroy(other.gameObject);
            weapon.Activate();
            weapon.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Stop();
        }

        if (other.tag == "AbrirAscensor" && !unaVez)
        {
            Door d = other.GetComponent<Door>();
            ElevatorButton b = other.GetComponent<ElevatorButton>();
            weapon.Deactivate();
            b.DejarArma();
            d.Open();
            unaVez = true;
        }

        if (other.tag == "End")
        {
            marte.SetActive(true);
        }

        if (other.tag == "EndGame")
        {
            StartCoroutine(End());
        }
    }
}

#region Botón
#if UNITY_EDITOR
[CustomEditor(typeof(Player))]
public class MyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Player p = (Player)target;
        if (GUILayout.Button("Poner al PJ en su posición inicial"))
        {
            p.transform.position = p.initialPos;
            p.transform.rotation = p.rotation;
        }
    }

}
#endif
#endregion

