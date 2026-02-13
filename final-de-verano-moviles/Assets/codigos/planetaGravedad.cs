using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planetaGravedad : MonoBehaviour
{
    public Transform jugador;
    public float radioPlaneta = 5f;
    public float rangoActivacion = 15f;

    void FixedUpdate()
    {
        if (jugador == null) return;

        float distancia = Vector3.Distance(jugador.position, transform.position);

        if (distancia <= rangoActivacion)
        {
            // Dirección desde el centro al jugador
            Vector3 direccion = (jugador.position - transform.position).normalized;

            // Forzar posición sobre la superficie
            jugador.position = transform.position + direccion * radioPlaneta;

            // Opcional: alinear visualmente al planeta
            jugador.up = direccion;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioPlaneta);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, rangoActivacion);
    }
}
