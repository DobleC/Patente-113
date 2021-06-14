using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public enum Sound // Identifica los distintos sonidos
    {
        ElectricShoot,
        LightOn,
        EnemyHitted,
        Screech,
        EnemEncounter,
        BrokenGlass,
        Spawn,
        HorrorAmbientMenu,
        ChasingPlayer,
        PlayerGetHit,
        OpenDoor,
        LightOnLoop,
        TurnOffWeapon,
        Click,
        TestSoundCannotLoop, // Sonido de prueba que no se puede repetir a menos que hayan pasado 5s
    }

    private static GameObject oneShotGameObject;
    private static AudioSource oneShotAudioSource;


    public static void PlaySound(Sound sound) // Recibe un nombre de sonido (enum) y reproduce su audio clip asociado
    {
        if (CanPlaySound(sound))
        {
            if (oneShotGameObject == null)  // Evita crear un objeto Sound para cada repetición de un sonido
            { 
                oneShotGameObject = new GameObject("Sound2D");
                oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();

            }
            oneShotAudioSource.PlayOneShot(GetAudioClip(sound));
        }
    }

    public static void PlaySound(Sound sound, Vector3 position) // PlaySound pero reproduce el audio en 3D (perfecto para cosas como pasos que te persigan o movidas así con cascos)
    {
        if (CanPlaySound(sound))
        {
            GameObject soundGameObject = new GameObject("Sound3D");
            soundGameObject.transform.position = position;
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.clip = GetAudioClip(sound);

            audioSource.minDistance = 0.5f;
            audioSource.maxDistance = 40f;
            audioSource.spatialBlend = 1f;
            audioSource.spread = 360f;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            //^Tipo de parametros que se pueden editar en el sonido 3D
            audioSource.Play();

            Object.Destroy(soundGameObject, audioSource.clip.length);
            // Los sonidos en 3d no pueden reutilizar el mismo gameobject de sonido (como pasa en PlaySound sin position) pq necesitan una posición en el espacio.
            // Para no gastar recursos innecesarios se destruyen al acabar su clip de audio
        }
    }


    private static AudioClip GetAudioClip(Sound sound) // Permite seleccionar el sonido a reproducir entre los que hay en soundAudioClipCollection[]
    {
        foreach (GameAssets.SoundAudioClip soundAudioClip in GameAssets.instance.soundAudioClipCollection)
        {
            if (soundAudioClip.sound == sound) return soundAudioClip.audioClip;
        }

        Debug.LogError("Si estás leyendo esto seguramente se te haya olvidado meter el nombre del sonido en la enum del SoundManager o el sonido en el GameAssets.");
        Debug.LogError("El sonido problemático en cuestión es: " + sound);
        return null;
    }



    //------------------------------------------------------------------Gestión de repetición de sonidos en el tiempo-------------------------------------------------------------------------------------------//


  

    private static Dictionary<Sound, float> soundTimerDictionary; // Almacena el sonido y el timer que tiene que tiene que esperar para repetirse

    public static void InitializeDictionary() // Inicializa el dictionary (recuerda que en el dictionary solo se guardan sonidos que tengan timer para no poder repetirse)
    {
       soundTimerDictionary = new Dictionary<Sound, float>();
        soundTimerDictionary[Sound.ChasingPlayer] = 0f;
        soundTimerDictionary[Sound.LightOnLoop] = -10f;
        soundTimerDictionary[Sound.TestSoundCannotLoop] = 0f; 
    }

    private static bool CanPlaySound(Sound sound) // Establece intervalos de tiempo para que sonidos que se repiten no lo hagan 1 vez por frame
    {
        float lastTimePlayed;
        float playerMoveTimerMax;
        switch (sound)
        {
            default: return true; // Para cualquier sonido que no se repita simplemente devuelve true

            
            // en el caso de un sonido que se ejecute en una función dentro de un update (por ejemplo el sonido de pasos al andar si no viniesen ya en el asset),
            // este switch comprueba que el timer para repetirse no ha finalizado y le impide reproducir el audio clip
            case Sound.ChasingPlayer:
                {
                    if (soundTimerDictionary.ContainsKey(sound))
                    {
                        lastTimePlayed = soundTimerDictionary[sound];
                        playerMoveTimerMax = 7f; // solo se puede repetir tras 7s

                        if (lastTimePlayed + playerMoveTimerMax < Time.time)
                        {
                            soundTimerDictionary[sound] = Time.time;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else // Si en el diccionario no hay un timer asociado para él es que se puede repetir
                    {
                        return true;
                    }
                    //break;
                }
            case Sound.LightOnLoop:
                {
                    if (soundTimerDictionary.ContainsKey(sound))
                    {
                        lastTimePlayed = soundTimerDictionary[sound];
                        playerMoveTimerMax = 27f; // solo se puede repetir tras 27s (duración de la canción)
            
                        if (lastTimePlayed + playerMoveTimerMax < Time.time)
                        {
                            soundTimerDictionary[sound] = Time.time;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else // Si en el diccionario no hay un timer asociado para él es que se puede repetir
                    {
                        return true;
                    }
                    //break;
                }
                
        } 
    }
}
