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

    [Header("UI")]
    public Image barraOxigeno;
    public GameObject panelDerrota;

    private float oxigenoActual;
    private bool derrotado = false;
    private bool enPlaneta = false;

    void Start()
    {
        oxigenoActual = oxigenoMaximo;
        panelDerrota.SetActive(false);
    }

    void Update()
    {
        if (derrotado) return;

        VerificarSiEstaEnPlaneta();

        if (enPlaneta)
        {
            RecargarOxigeno();
        }
        else
        {
            ConsumirOxigeno();
        }

        ActualizarUI();

        if (oxigenoActual <= 0)
        {
            Derrota();
        }
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
