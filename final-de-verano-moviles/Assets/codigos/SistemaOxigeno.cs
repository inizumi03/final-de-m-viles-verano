using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SistemaOxigeno : MonoBehaviour
{
    [Header("Oxigeno")]
    public float oxigenoMaximo = 100f;
    public float velocidadConsumo = 5f;
    public float velocidadRecarga = 15f;

    [Header("Alerta")]
    public float porcentajeAlerta = 0.25f; // 25%
    public float velocidadParpadeo = 5f;

    [Header("UI")]
    public Image barraOxigeno;
    public GameObject panelDerrota;

    private float oxigenoActual;
    private bool derrotado = false;
    private bool enPlaneta = false;
    private Color colorOriginal;

    void Start()
    {
        oxigenoActual = oxigenoMaximo;
        panelDerrota.SetActive(false);
        colorOriginal = barraOxigeno.color;
    }

    void Update()
    {
        if (derrotado) return;

        VerificarSiEstaEnPlaneta();

        if (enPlaneta)
            RecargarOxigeno();
        else
            ConsumirOxigeno();

        ActualizarUI();
        ManejarAlertaVisual();

        if (oxigenoActual <= 0)
            Derrota();
    }

    void ConsumirOxigeno()
    {
        oxigenoActual -= velocidadConsumo * Time.deltaTime;
        oxigenoActual = Mathf.Clamp(oxigenoActual, 0, oxigenoMaximo);
    }

    void RecargarOxigeno()
    {
        oxigenoActual += velocidadRecarga * Time.deltaTime;
        oxigenoActual = Mathf.Clamp(oxigenoActual, 0, oxigenoMaximo);
    }

    void ActualizarUI()
    {
        barraOxigeno.fillAmount = oxigenoActual / oxigenoMaximo;
    }

    void ManejarAlertaVisual()
    {
        float porcentajeActual = oxigenoActual / oxigenoMaximo;

        if (porcentajeActual <= porcentajeAlerta)
        {
            float t = Mathf.PingPong(Time.time * velocidadParpadeo, 1f);
            barraOxigeno.color = Color.Lerp(colorOriginal, Color.red, t);
        }
        else
        {
            barraOxigeno.color = colorOriginal;
        }
    }
    public void AgregarOxigeno(float cantidad)
    {
        oxigenoActual += cantidad;
        oxigenoActual = Mathf.Clamp(oxigenoActual, 0, oxigenoMaximo);
    }
    void VerificarSiEstaEnPlaneta()
    {
        enPlaneta = false;

        Planeta[] planetas = FindObjectsOfType<Planeta>();

        foreach (Planeta p in planetas)
        {
            if (p.EstaEnRango(transform.position))
            {
                enPlaneta = true;
                return;
            }
        }
    }

    void Derrota()
    {
        derrotado = true;
        panelDerrota.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ReiniciarNivel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
