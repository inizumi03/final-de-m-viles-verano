using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoJugadorRB : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 7f;
    public float fuerzaSalto = 8f;

    private Rigidbody rb;
    private bool enSuelo;

    // Variables que usa el script del planeta
    [HideInInspector] public Vector3 modificadorDireccion = Vector3.zero;
    [HideInInspector] public bool usarModificador = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Movimiento fijo en el mundo (NO depende de rotación)
        Vector3 direccion = new Vector3(x, 0f, z).normalized;

        // Si el planeta modifica el movimiento
        if (usarModificador)
        {
            direccion = modificadorDireccion;
        }

        Vector3 nuevaVelocidad = new Vector3(
            direccion.x * velocidad,
            rb.velocity.y,
            direccion.z * velocidad
        );

        rb.velocity = nuevaVelocidad;
    }

    void OnCollisionStay(Collision collision)
    {
        enSuelo = true;
    }

    void OnCollisionExit(Collision collision)
    {
        enSuelo = false;
    }
}
