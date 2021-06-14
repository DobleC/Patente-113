using UnityEngine;
using Random = UnityEngine.Random;

public class CameraShake : MonoBehaviour
{
    public Transform camTransform, camTransformVisor;


    private static float shakeDuration = 0;

    private static float shakeAmount = 0;

    private float vel;
    private Vector3 vel2 = Vector3.zero;

    Vector3 originalPos, originalPos2;

    private void Awake()
    {
        
    }
    public static void ShakeOnce(float lenght, float strength)
    {
        shakeDuration = lenght;
        shakeAmount = strength;
    }

    void Update()
    {

        if (shakeDuration > 0)
        {
            Vector3 newPos = originalPos + Random.insideUnitSphere * shakeAmount;

            camTransform.localPosition = Vector3.SmoothDamp(camTransform.localPosition, newPos, ref vel2, 0.05f);

            shakeDuration -= Time.deltaTime;
            shakeAmount = Mathf.SmoothDamp(shakeAmount, 0, ref vel, 0.7f);
        }
        else
        {
            camTransform.localPosition = originalPos;
        }

    }
}
