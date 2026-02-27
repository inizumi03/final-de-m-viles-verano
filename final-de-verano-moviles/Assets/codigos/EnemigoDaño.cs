using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoDaño : MonoBehaviour
{
    [Header("Sistema de Vidas Visuales")]
    public GameObject[] imagenesDaño; // 3 imágenes
    private int golpesActuales = 0;

    [Header("Derrota")]
    public GameObject canvasDerrota;

    [Header("Drop al morir")]
    public GameObject prefabDrop;

    // =========================
    // DAÑO AL JUGADOR
    // =========================

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        golpesActuales++;

        // Si todavía está dentro del rango de imágenes
        if (golpesActuales <= imagenesDaño.Length)
        {
            imagenesDaño[golpesActuales - 1].SetActive(true);
        }

        // Si supera las imágenes → pierde
        if (golpesActuales > imagenesDaño.Length)
        {
            if (canvasDerrota != null)
            {
                canvasDerrota.SetActive(true);
                Time.timeScale = 0f;
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
