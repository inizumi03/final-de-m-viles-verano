using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaEspecial : MonoBehaviour
{

    [Header("Gravedad Personalizada")]
    public float fuerzaGravedad = 25f;

    [Header("Rango de Activación")]
    public float rangoAltura = 5f;

    [Header("Velocidad alineación al salir")]
    public float velocidadAlineacionSalida = 5f;

    private BoxCollider boxCollider;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();

        if (boxCollider == null)
        {
            Debug.LogError("PlataformaEspecial necesita un BoxCollider.");
        }
    }

    // =========================
    //  ATRACCIÓN
    // =========================

    public void Atraer(Rigidbody rb)
    {
        Vector3 normal = ObtenerNormal();

        rb.AddForce(-normal * fuerzaGravedad, ForceMode.Acceleration);
    }

    // =========================
    // OBTENER NORMAL (ARRIBA)
    // =========================

    public Vector3 ObtenerNormal()
    {
        // El "arriba" real de la plataforma
        return transform.up;
    }

    // =========================
    //  DETECTAR SI ESTÁ EN RANGO
    // =========================

    public bool EstaEnRango(Vector3 posicion)
    {
        if (boxCollider == null) return false;

        Vector3 puntoMasCercano = boxCollider.ClosestPoint(posicion);

        float distancia = Vector3.Distance(posicion, puntoMasCercano);

        return distancia <= rangoAltura;
    }

    // =========================
    // TRIGGER PARA INFORMAR AL JUGADOR
    // =========================

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        JugadorPlaneta jugador = other.GetComponent<JugadorPlaneta>();
        if (jugador == null) return;

        jugador.superficieActual = this;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        JugadorPlaneta jugador = other.GetComponent<JugadorPlaneta>();
        if (jugador == null) return;

        jugador.superficieActual = null;

        StartCoroutine(SuavizarAlineacion(jugador));
    
}
    private IEnumerator SuavizarAlineacion(JugadorPlaneta jugador)
    {
        Rigidbody rb = jugador.GetComponent<Rigidbody>();

        while (Vector3.Angle(jugador.transform.up, Vector3.up) > 1f)
        {
            Quaternion rotacionObjetivo =
                Quaternion.FromToRotation(jugador.transform.up, Vector3.up) * rb.rotation;

            rb.MoveRotation(
                Quaternion.Slerp(
                    rb.rotation,
                    rotacionObjetivo,
                    velocidadAlineacionSalida * Time.fixedDeltaTime
                )
            );

            yield return new WaitForFixedUpdate();
        }

    }
}