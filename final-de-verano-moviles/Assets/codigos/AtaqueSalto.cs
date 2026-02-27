using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaqueSalto : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemigo"))
        {
            EnemigoDaño enemigo = other.GetComponent<EnemigoDaño>();

            if (enemigo != null)
            {
                enemigo.Morir();
            }
        }
    }
}