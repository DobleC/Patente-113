using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manivela : MonoBehaviour
{
 
    // Llevo 4h rayándome con los vectores que tirar para solo haber logrado esta basura
    // Si editas en update c > 19 por yokese, 1, se tirará un dibujo de raycast cada 2 frames y podrás ver que el círculo que se dibuja es una puta mierda
    private int c = 0;



    public void VectorToPoint()
    {
        
        Vector2 ratonPosition = Camera.main.WorldToScreenPoint (Input.mousePosition);
        ratonPosition.Normalize();
        Debug.DrawRay( transform.position, ratonPosition * 200, Color.green, 0.5f);

    }
        
    private void Update()
    {
        ++c;
        if (c > 19) // tira rayos 3 veces por seg
        {
            VectorToPoint();
            c = 0;
        }
    }
}
