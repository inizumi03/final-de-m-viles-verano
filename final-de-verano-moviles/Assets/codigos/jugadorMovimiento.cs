using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jugadorMovimiento : MonoBehaviour
{
    public float velocidad = 7f;
    public float fuerzaSalto = 8f;

    public float distanciaSuelo = 1.2f;
    public LayerMask capaSuelo;

    private Rigidbody rb;
    private planetaSuperficie planetaActual;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        DetectarPlanetaMasCercano();

        if (Input.GetKeyDown(KeyCode.Space) && EstaEnSuelo())
        {
            Vector3 up = GetUpDirection();

            // Limpiar velocidad vertical antes de saltar
            rb.velocity = rb.velocity - Vector3.Project(rb.velocity, up);

            rb.AddForce(up * fuerzaSalto, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        float lateral = 0f;
        float adelante = 0f;

        if (Input.GetKey(KeyCode.A))
            lateral = -1f;

        if (Input.GetKey(KeyCode.D))
            lateral = 1f;

        if (Input.GetKey(KeyCode.W))
            adelante = 1f;

        Vector3 up = GetUpDirection();
        Vector3 forward = Vector3.Cross(transform.right, up).normalized;

        Vector3 direccion = (transform.right * lateral + forward * adelante).normalized;

        rb.velocity = direccion * velocidad + up * rb.velocity.y;
    }

    void DetectarPlanetaMasCercano()
    {
        planetaSuperficie[] planetas = FindObjectsOfType<planetaSuperficie>();

        planetaSuperficie masCercano = null;
        float menorDistancia = Mathf.Infinity;

        foreach (var planeta in planetas)
        {
            if (planeta.EstaEnRango(transform.position))
            {
                float distancia = Vector3.Distance(transform.position, planeta.transform.position);

                if (distancia < menorDistancia)
                {
                    menorDistancia = distancia;
                    masCercano = planeta;
                }
            }
        }

        planetaActual = masCercano;
    }

    Vector3 GetUpDirection()
    {
        if (planetaActual == null)
            return Vector3.up;

        return planetaActual.ObtenerUp(transform.position);
    }

    bool EstaEnSuelo()
    {
        Vector3 direccion = -GetUpDirection();
        return Physics.Raycast(transform.position, direccion, distanciaSuelo, capaSuelo);
    }
}
