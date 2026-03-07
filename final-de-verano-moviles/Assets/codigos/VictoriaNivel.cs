using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class VictoriaNivel : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject canvasVictoria;

    [Header("Textos")]
    public TextMeshProUGUI textoTiempo;
    public TextMeshProUGUI textoMonedas;

    [Header("Sistema monedas")]
    public ContadorPuntos contadorPuntos;

    private float tiempoInicio;

    void Start()
    {
        tiempoInicio = Time.time;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        StartCoroutine(SecuenciaVictoria());
    }

    IEnumerator SecuenciaVictoria()
    {
        float tiempoFinal = Time.time - tiempoInicio;
        int monedasFinal = contadorPuntos.ObtenerPuntos();

        canvasVictoria.SetActive(true);

        // animación de números
        yield return StartCoroutine(AnimarNumeroTiempo(tiempoFinal));
        yield return StartCoroutine(AnimarNumeroMonedas(monedasFinal));

        Time.timeScale = 0f;
    }

    IEnumerator AnimarNumeroTiempo(float tiempoReal)
    {
        float duracion = 1.5f;
        float tiempo = 0;

        while (tiempo < duracion)
        {
            float random = Random.Range(0f, 999f);
            textoTiempo.text = "Tiempo: " + random.ToString("F1") + " s";

            tiempo += 0.05f;
            yield return new WaitForSeconds(0.05f);
        }

        textoTiempo.text = "Tiempo: " + tiempoReal.ToString("F1") + " s";
    }

    IEnumerator AnimarNumeroMonedas(int monedasReal)
    {
        float duracion = 1.2f;
        float tiempo = 0;

        while (tiempo < duracion)
        {
            int random = Random.Range(0, 200);
            textoMonedas.text = "Estrellas: " + random;

            tiempo += 0.05f;
            yield return new WaitForSeconds(0.05f);
        }

        textoMonedas.text = "Estrellas: " + monedasReal;
    }

    // BOTONES

    public void Reintentar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SalirJuego()
    {
        Application.Quit();
    }
}
