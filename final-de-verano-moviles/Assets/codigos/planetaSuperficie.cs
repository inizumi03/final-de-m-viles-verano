using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planetaSuperficie : MonoBehaviour
{
    public float radioPlaneta = 5f;
    public float rangoInfluencia = 15f;

    public bool EstaEnRango(Vector3 posicionJugador)
    {
        float distancia = Vector3.Distance(posicionJugador, transform.position);
        return distancia <= rangoInfluencia;
    }

    public Vector3 ObtenerUp(Vector3 posicionJugador)
    {
        return (posicionJugador - transform.position).normalized;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioPlaneta);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, rangoInfluencia);
    }
}
