using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoDaño : MonoBehaviour
{
    [Header("Sistema de Vidas Visuales")]
    public GameObject[] imagenesDaño; // imágenes que se activan al recibir daño
    private int golpesActuales = 0;

    [Header("Derrota")]
    public GameObject canvasDerrota;

    [Header("Drop al morir")]
    public GameObject prefabDrop;

    [Header("Combate")]
    public float fuerzaEmpujon = 8f;
    public float tiempoEmpujon = 0.25f;

    private bool jugadorInvulnerable = false;

    // =========================
    // DAÑO AL JUGADOR
    // =========================

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        if (jugadorInvulnerable) return;

        JugadorPlaneta jugador = collision.gameObject.GetComponent<JugadorPlaneta>();
        if (jugador == null) return;

        // Dirección para empujar al jugador lejos del enemigo
        Vector3 direccion = (collision.transform.position - transform.position).normalized;

        jugador.RecibirEmpujon(direccion, fuerzaEmpujon, tiempoEmpujon);

        golpesActuales++;

        // Activar imágenes de daño
        if (golpesActuales <= imagenesDaño.Length)
        {
            imagenesDaño[golpesActuales - 1].SetActive(true);
        }

        // Si supera las imágenes → derrota
        if (golpesActuales > imagenesDaño.Length)
        {
            if (canvasDerrota != null)
            {
                canvasDerrota.SetActive(true);
                Time.timeScale = 0f;
            }
        }

        StartCoroutine(InvulnerabilidadJugador());
    }

    // =========================
    // INVULNERABILIDAD
    // =========================

    IEnumerator InvulnerabilidadJugador()
    {
        jugadorInvulnerable = true;
        yield return new WaitForSeconds(2f);
        jugadorInvulnerable = false;
    }

    // =========================
    // MUERTE DEL ENEMIGO
    // =========================

    public void Morir()
    {
        if (prefabDrop != null)
        {
            Instantiate(prefabDrop, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
