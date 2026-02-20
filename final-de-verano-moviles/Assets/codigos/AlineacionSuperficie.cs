using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlineacionSuperficie : MonoBehaviour
{
    public float velocidadAlineacion = 8f;
    public string tagPlataforma = "PlataformaEspecial";

    private Vector3 normalActual = Vector3.up;
    private bool alineando = false;

    void Update()
    {
        if (alineando)
        {
            Quaternion rotacionObjetivo =
                Quaternion.FromToRotation(transform.up, normalActual) * transform.rotation;

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                rotacionObjetivo,
                velocidadAlineacion * Time.deltaTime
            );
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag(tagPlataforma))
        {
            foreach (ContactPoint contacto in collision.contacts)
            {
                normalActual = contacto.normal;
                alineando = true;
                return;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(tagPlataforma))
        {
            alineando = false;
        }
    }
}
