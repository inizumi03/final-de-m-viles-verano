using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMovil : MonoBehaviour
{
    public static float Horizontal;
    public static float Vertical;
    public static bool Salto;

    public void SetHorizontal(float value)
    {
        Horizontal = value;
    }

    public void SetVertical(float value)
    {
        Vertical = value;
    }

    public void BotonSalto()
    {
        Salto = true;
    }

    void LateUpdate()
    {
        Salto = false; // se resetea cada frame
    }
}
