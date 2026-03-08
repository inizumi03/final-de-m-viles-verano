using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menudeljuego : MonoBehaviour
{
    public GameObject imagenTutorial; // Panel o imagen del tutorial

    // Mostrar tutorial
    public void AbrirTutorial()
    {
        imagenTutorial.SetActive(true);
    }

    // Cerrar tutorial
    public void CerrarTutorial()
    {
        imagenTutorial.SetActive(false);
    }

    // Cambiar de escena
    public void CargarEscena(string nombreEscena)
    {
        SceneManager.LoadScene(nombreEscena);
    }

    // Salir del juego
    public void SalirJuego()
    {
        Application.Quit();
        Debug.Log("El juego se cerró");
    }
}
