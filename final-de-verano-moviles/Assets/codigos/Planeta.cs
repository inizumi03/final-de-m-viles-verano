using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planeta : MonoBehaviour
{
    public float gravedad = 25f;
    public float rangoGravedad = 20f;

    public void Atraer(Rigidbody rb)
    {
        Vector3 direccion = (transform.position - rb.position).normalized;
        rb.AddForce(direccion * gravedad, ForceMode.Acceleration);
    }

    public bool EstaEnRango(Vector3 posicion)
    {
        return Vector3.Distance(posicion, transform.position) <= rangoGravedad;
    }

    public Vector3 ObtenerUp(Vector3 posicion)
    {
        return (posicion - transform.position).normalized;
    }

    // GIZMOS
    void OnDrawGizmos()
    {
        // Centro del planeta
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.3f);

        // Rango de gravedad
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, rangoGravedad);
    }

    void OnDrawGizmosSelected()
    {
        // Línea hacia abajo (visualizar "centro de gravedad")
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * 3f);
    }
}
