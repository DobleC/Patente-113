using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameAssets : MonoBehaviour
{

    // ¡¡¡ MUY IMPORTANTE !!! Para funcionar como repositorio de assets en cualquier escena necesito que haya un prefab llamado GameAssets en la carpeta Assets > Resources


    // He estado buscando y la mejor manera de hacer un manager de sonidos era con una clase estática que recogiese los sonidos de un controlador de assets
    // Como tal, si introduces aquí cualquier tipo de objeto, vas a poder llamarlo en tiempo de ejecución en cualquier escena que quieras, se instancia en ella



    private static GameAssets _i;
    public static GameAssets instance
    {
        get
        {
            if (_i == null) _i = (Instantiate(Resources.Load("GameAssets")) as GameObject).GetComponent<GameAssets>();
            return _i;
        }
    }

    public Material lightMaterialOn;
    public Material lightMaterialOnVisor;

   

    public void ReloadScene() 
    {
        SceneManager.LoadScene(1);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
        Application.Quit();
    }


    //---------------------------------------------------------Biblioteca de sonido-------------------------------------------------------------------------------------------//

    [System.Serializable]
    public class SoundAudioClip // Clase personalizada que consiste en el clip de audio a reproducir y su identificador (enum)
    {
        public SoundManager.Sound sound; // enum
        public AudioClip audioClip;

    }

    public SoundAudioClip[] soundAudioClipCollection; // Array que guarda todos los sonidos del juego, I guess que podemos hacer otro distinto para música

    private void Awake()
    {
        SoundManager.InitializeDictionary();      // Cuando tengamos algo como un controlador general del juego, esta sentencia debe ir en su awake, de momento apaña así 
       
    }
}
