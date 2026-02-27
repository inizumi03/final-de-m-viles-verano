using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlColliderSalto : MonoBehaviour
{
    [Header("Collider que se activará durante el salto")]
    public Collider colliderSalto;

    // Se llama desde Animation Event al comenzar el salto
    public void ActivarCollider()
    {
        if (colliderSalto != null)
        {
            colliderSalto.enabled = true;
        }
    }

    // Se llama desde Animation Event al terminar el salto
    public void DesactivarCollider()
    {
        if (colliderSalto != null)
        {
            colliderSalto.enabled = false;
        }
    }
}
