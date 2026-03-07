using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsulaOxigeno : MonoBehaviour
{
    public float cantidadOxigeno = 20f;

    private void OnTriggerEnter(Collider other)
    {
        SistemaOxigeno sistema = other.GetComponent<SistemaOxigeno>();

        if (sistema != null)
        {
            sistema.AgregarOxigeno(cantidadOxigeno);
            gameObject.SetActive(false);
        }
    }
}
