using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JugadorAnimaciones : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;

    [Header("Detección suelo")]
    public float distanciaSuelo = 1.2f;
    public LayerMask capaSuelo;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponentInParent<Rigidbody>();
    }

    void Update()
    {
        ControlarMovimiento();
        ControlarSalto();
    }

    void ControlarMovimiento()
    {
        // Velocidad horizontal real
        Vector3 velocidadHorizontal = rb.velocity;
        velocidadHorizontal.y = 0f;

        float velocidad = velocidadHorizontal.magnitude;

        animator.SetFloat("Velocidad", velocidad);
    }

    void ControlarSalto()
    {
        bool enSuelo = Physics.Raycast(
            transform.position,
            -transform.up,
            distanciaSuelo,
            capaSuelo
        );

        animator.SetBool("EnSuelo", enSuelo);
    }
}
