using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaMovil : MonoBehaviour
{
    public Transform puntoA;
    public Transform puntoB;
    public float velocidad = 3f;

    private Vector3 objetivo;

    void Start()
    {
        objetivo = puntoB.position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            objetivo,
            velocidad * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, objetivo) < 0.05f)
        {
            objetivo = (objetivo == puntoA.position) ? puntoB.position : puntoA.position;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
