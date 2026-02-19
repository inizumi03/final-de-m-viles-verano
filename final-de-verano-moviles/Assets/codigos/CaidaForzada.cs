using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaidaForzada : MonoBehaviour
{
    public float tiempoEnAirePermitido = 1.5f;
    public float fuerzaExtraCaida = 20f;
    public float distanciaSuelo = 1.2f;
    public LayerMask capaSuelo;

    private Rigidbody rb;
    private float contadorAire;
    private bool enSuelo;
    private bool dentroDePlaneta;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        VerificarSuelo();
        VerificarRangoPlaneta();

        if (!enSuelo)
            contadorAire += Time.deltaTime;
        else
            contadorAire = 0f;
    }

    void FixedUpdate()
    {
        //  No aplicar caída si está dentro del rango de un planeta
        if (!enSuelo && contadorAire >= tiempoEnAirePermitido && !dentroDePlaneta)
        {
            rb.AddForce(Vector3.down * fuerzaExtraCaida, ForceMode.Acceleration);
        }
    }

    void VerificarSuelo()
    {
        enSuelo = Physics.Raycast(
            transform.position,
            Vector3.down,
            distanciaSuelo,
            capaSuelo
        );
    }

    void VerificarRangoPlaneta()
    {
        dentroDePlaneta = false;

        Planeta[] planetas = FindObjectsOfType<Planeta>();

        foreach (Planeta p in planetas)
        {
            if (p.EstaEnRango(transform.position))
            {
                dentroDePlaneta = true;
                return;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        Vector3 origen = transform.position;
        Vector3 destino = origen + Vector3.down * distanciaSuelo;

        Gizmos.DrawLine(origen, destino);
        Gizmos.DrawSphere(destino, 0.1f);
    }
}
