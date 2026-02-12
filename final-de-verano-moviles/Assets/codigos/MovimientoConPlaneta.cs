using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoConPlaneta : MonoBehaviour
{
    [Header("Configuración Planeta")]
    public Transform planeta;
    public float radioPlaneta = 5f;
    public float distanciaActivacion = 1.5f;

    private MovimientoJugadorRB movimiento;

    void Start()
    {
        movimiento = GetComponent<MovimientoJugadorRB>();
    }

    void FixedUpdate()
    {
        if (planeta == null || movimiento == null) return;

        float distancia = Vector3.Distance(transform.position, planeta.position);
        bool enPlaneta = distancia <= radioPlaneta + distanciaActivacion;

        if (enPlaneta)
        {
            Vector3 normalPlaneta = (transform.position - planeta.position).normalized;

            // Alinear jugador a la superficie
            transform.up = normalPlaneta;

            // Leer input
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 direccion = transform.forward * z + transform.right * x;

            // Proyectar movimiento sobre la superficie
            direccion = Vector3.ProjectOnPlane(direccion, normalPlaneta);

            movimiento.modificadorDireccion = direccion;
            movimiento.usarModificador = true;
        }
        else
        {
            movimiento.usarModificador = false;
        }
    }

    void OnDrawGizmos()
    {
        if (planeta == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(planeta.position, radioPlaneta + distanciaActivacion);
    }
}
