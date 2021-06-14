using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    [SerializeField]
    private GameObject LoadingScreen;
    [SerializeField]
    private Slider loadingPorcentaje; // O esto o un texto que ponga el porcentaje de carga

    bool reproduciendoMusic = false;

    // Update is called once per frame
    void Update()
    {
        if (!reproduciendoMusic) StartCoroutine(loopMusicMenu());
        // Soy puto retrasado y he metido 2 variables no estáticas en el código que gestiona cuando loopea la música, así que hago este apaño para poder loopear desde el menú, zorry

    }

    public void PlayGame(string Scene)
    {
        Debug.Log("Cambiando a la escena " + Scene); //Para que en la consola de unity aparezca si se esta realizando
        StartCoroutine(OperationAsync(Scene));

    }
    //corrutina que creamos para la pantalla durante la carga de escenas, a modo de transicion.
    IEnumerator OperationAsync(string Scene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(Scene);
        LoadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            //float progress = Mathf.Clamp01(operation.progress / 0.9f);
            //loadingPorcentaje.value = progress;
            yield return null;
        }
    }

    public void ExitGame()
    {
        Debug.Log("Saliendo del juego");
        Application.Quit();
    }

    private IEnumerator loopMusicMenu()
    {
        reproduciendoMusic = true;
        SoundManager.PlaySound(SoundManager.Sound.HorrorAmbientMenu);
        yield return new WaitForSeconds(89.0f); // Duración en segundos de la canción (esta dura 91, pero queda mejor si loopea en el 89 pq los 2s del final están super bajitos)   
        reproduciendoMusic = false;
        StopCoroutine(loopMusicMenu());
    }
}
