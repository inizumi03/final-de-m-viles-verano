using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraTerceraPersona : MonoBehaviour
{
    [Header("Objetivo")]
    public Transform jugador;

    [Header("Distancia")]
    public float distancia = 6f;
    public float altura = 2f;

    [Header("Suavizado")]
    public float suavizadoMovimiento = 5f;
    public float suavizadoRotacion = 5f;

    [Header("Inclinación en salto")]
    public float inclinacionSalto = 15f;
    public float distanciaRaycastSuelo = 1.3f;

    void LateUpdate()
    {
        if (jugador == null) return;

        // =============================
        // POSICIÓN DETRÁS DEL JUGADOR
        // =============================

        Vector3 direccion = -jugador.forward;

        Vector3 posicionDeseada =
            jugador.position +
            jugador.up * altura +
            direccion * distancia;

        transform.position = Vector3.Lerp(
            transform.position,
            posicionDeseada,
            suavizadoMovimiento * Time.deltaTime
        );

        // =============================
        // DETECTAR SI ESTÁ EN EL SUELO
        // =============================

        bool enSuelo = Physics.Raycast(
            jugador.position,
            -jugador.up,
            distanciaRaycastSuelo
        );

        float anguloExtra = 0f;

        if (!enSuelo)
        {
            anguloExtra = inclinacionSalto;
        }

        // =============================
        // ROTACIÓN MIRANDO AL JUGADOR
        // =============================

        Quaternion rotacionBase =
            Quaternion.LookRotation(
                jugador.position + jugador.up * altura - transform.position,
                jugador.up
            );

        Quaternion inclinacion =
            Quaternion.AngleAxis(
                anguloExtra,
                transform.right
            );

        Quaternion rotacionFinal = rotacionBase * inclinacion;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            rotacionFinal,
            suavizadoRotacion * Time.deltaTime
        );
    }
}
