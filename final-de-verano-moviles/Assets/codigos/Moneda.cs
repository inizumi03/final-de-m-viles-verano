using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moneda : MonoBehaviour
{
    private ContadorPuntos contador;

    void Start()
    {
        contador = FindObjectOfType<ContadorPuntos>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (contador != null)
                contador.SumarPunto();

            Destroy(gameObject);
        }
    }
}
