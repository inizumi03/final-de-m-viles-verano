using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZonaVelocidad : MonoBehaviour
{
    public float aumentoAdelante = 5f;
    public float aumentoLateral = 3f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            JugadorPlaneta jugador = other.GetComponent<JugadorPlaneta>();

            if (jugador != null)
            {
                jugador.velocidadAdelante += aumentoAdelante;
                jugador.velocidadLateral += aumentoLateral;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            JugadorPlaneta jugador = other.GetComponent<JugadorPlaneta>();

            if (jugador != null)
            {
                jugador.velocidadAdelante -= aumentoAdelante;
                jugador.velocidadLateral -= aumentoLateral;
            }
        }
    }
}
