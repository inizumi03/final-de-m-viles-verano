using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 puntoControl;

    void Start()
    {
        puntoControl = transform.position; // posición inicial
    }

    private void OnCollisionEnter(Collision collision)
    {
        //  Si toca un planeta nuevo checkpoint
        if (collision.gameObject.CompareTag("Planeta"))
        {
            puntoControl = collision.transform.position;
        }

        // Si toca zona de caída  respawn
        if (collision.gameObject.CompareTag("ZonaCaida"))
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        transform.position = puntoControl;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
