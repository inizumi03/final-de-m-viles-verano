using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ContadorPuntos : MonoBehaviour
{
    public TextMeshProUGUI textoPuntos;

    private int puntos = 0;

    public void SumarPunto()
    {
        puntos++;
        textoPuntos.text = puntos.ToString();
    }

}
