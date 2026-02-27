using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoDaño : MonoBehaviour
{
    [Header("Sistema Derrota")]
    public int golpesParaPerder = 3;
    private int golpesActuales = 0;
    public GameObject canvasDerrota;

    [Header("Drop al morir")]
    public GameObject prefabDrop;

    // =========================
    // DAÑO AL JUGADOR
    // =========================

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        // Verificamos si el jugador está atacando
        Collider colliderAtaque = collision.gameObject
            .GetComponentInChildren<Collider>();

        if (colliderAtaque != null && colliderAtaque.enabled)
        {
            return; // Si está atacando, no recibe daño
        }

        golpesActuales++;

        if (golpesActuales >= golpesParaPerder)
        {
            if (canvasDerrota != null)
            {
                canvasDerrota.SetActive(true);
                Time.timeScale = 0f; // 🔥 pausa el juego
            }
        }
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
