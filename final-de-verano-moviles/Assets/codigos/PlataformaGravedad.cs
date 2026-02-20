using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaGravedad : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            JugadorPlaneta jugador = collision.gameObject.GetComponent<JugadorPlaneta>();
            if (jugador != null)
            {
                jugador.ActivarGravedadPersonalizada(transform.up);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            JugadorPlaneta jugador = collision.gameObject.GetComponent<JugadorPlaneta>();
            if (jugador != null)
            {
                jugador.DesactivarGravedadPersonalizada();
            }
        }
    }
}
