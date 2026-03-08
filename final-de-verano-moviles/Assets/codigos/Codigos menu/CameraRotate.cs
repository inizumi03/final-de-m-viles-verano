using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public float velocidad = 30f; // velocidad de rotación

    void Update()
    {
        transform.Rotate(0, velocidad * Time.deltaTime, 0);
    }
}
