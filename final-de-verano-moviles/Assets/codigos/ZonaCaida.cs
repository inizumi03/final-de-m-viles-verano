using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZonaCaida : MonoBehaviour
{
    [Header("Objetos que reaparecen al respawn")]
    public List<GameObject> objetosRespawn = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerRespawn respawn = other.GetComponent<PlayerRespawn>();

        if (respawn != null)
        {
            respawn.Respawn();
        }

        // Reactivar objetos
        foreach (GameObject obj in objetosRespawn)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
    }
}
