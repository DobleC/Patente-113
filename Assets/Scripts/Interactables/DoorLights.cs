using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLights : MonoBehaviour
{
    public bool isOn;
    public Door puerta;
    [SerializeField] private float lightIntensity = 1f;
    [SerializeField] private float lightRange = 20f;
    private MeshRenderer meshRenderer, meshLight;

    private void Awake()
    {
        meshRenderer = GetComponentInParent<MeshRenderer>();
       /* meshLight = GetComponentInParent<Transform>().GetChild(0).GetComponent<MeshRenderer>(); */

       
    }
    void SwitchOn()
    {
        SoundManager.PlaySound(SoundManager.Sound.LightOn, transform.position);
        GetComponent<Light>().intensity = lightIntensity;
        GetComponent<Light>().range = lightRange;
        if (puerta != null) puerta.CheckLights();
        isOn = true;
        transform.GetChild(0).GetComponent<MeshRenderer>().material = GameAssets.instance.lightMaterialOn;
        transform.GetChild(1).GetComponent<MeshRenderer>().material = GameAssets.instance.lightMaterialOnVisor;
    }

    public void RayTargetHit() // Se enciende cuando recibe un disparo del arma
    {
        if (!isOn) SwitchOn();
    }

    private void Start()
    {
        if (isOn)
        {
            GetComponent<Light>().intensity = lightIntensity;
            transform.GetChild(0).GetComponent<MeshRenderer>().material = GameAssets.instance.lightMaterialOn;
            transform.GetChild(1).GetComponent<MeshRenderer>().material = GameAssets.instance.lightMaterialOnVisor;
            
        }
    }

    //private void Update()
    //{
    //    if(isOn) SoundManager.PlaySound(SoundManager.Sound.LightOnLoop, transform.position);
    //}
}
