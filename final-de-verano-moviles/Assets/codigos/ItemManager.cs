using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject prefabItem;
    public Transform[] puntosSpawn;

    void Start()
    {
        foreach (Transform punto in puntosSpawn)
        {
            if (punto != null)
            {
                Instantiate(prefabItem, punto.position, Quaternion.identity);
            }
        }
    }
}
