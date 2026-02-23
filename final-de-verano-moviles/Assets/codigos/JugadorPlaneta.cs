using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JugadorPlaneta : MonoBehaviour
{
    public float velocidadAdelante = 8f;
    public float velocidadLateral = 6f;
    public float fuerzaSalto = 10f;
    public float distanciaSuelo = 1.2f;
    public LayerMask capaSuelo;

    public float velocidadAjusteRotacion = 5f;

    private Rigidbody rb;
    private Planeta planetaActual;

    private bool enPlaneta;
    private bool estabaEnPlaneta;

    private bool ajustandoRotacion;
    private Quaternion rotacionObjetivo;

    // Superficie especial
   
    public PlataformaEspecial superficieActual;
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Congelamos solo rotaciones físicas
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        DetectarPlaneta();

        if (Input.GetKeyDown(KeyCode.Space) && EstaEnSuelo())
        {
            Vector3 up = ObtenerUpActual();

            rb.velocity -= Vector3.Project(rb.velocity, up);
            rb.AddForce(up * fuerzaSalto, ForceMode.Impulse);
        }
       
    }

    void FixedUpdate()
    {
        if (superficieActual != null &&
            superficieActual.EstaEnRango(transform.position))
        {
            rb.useGravity = false;

            superficieActual.Atraer(rb);
            ModoSuperficie();
        }
        else if (enPlaneta && planetaActual != null)
        {
            ModoPlaneta();
        }
        else
        {
            ModoNormal();
        }

        AjustarRotacionSuave();

        
    }

    // =========================
    // MODO PLATAFORMA
    // =========================

    void ModoSuperficie()
    {
        Vector3 up = superficieActual.ObtenerNormal();

        // Rotación alineada a la superficie
        Quaternion rotacion =
            Quaternion.FromToRotation(transform.up, up) * rb.rotation;

        rb.MoveRotation(
            Quaternion.Slerp(rb.rotation, rotacion, 12f * Time.fixedDeltaTime)
        );

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 forward = Vector3.Cross(transform.right, up).normalized;

        Vector3 movimiento =
            transform.right * h * velocidadLateral +
            forward * v * velocidadAdelante;

        Vector3 velocidadVertical = Vector3.Project(rb.velocity, up);

        rb.velocity = movimiento + velocidadVertical;
    }

    // =========================
    //  MODO PLANETA
    // =========================

    void ModoPlaneta()
    {
        rb.useGravity = false;

        planetaActual.Atraer(rb);

        Vector3 up = planetaActual.ObtenerUp(transform.position);

        Quaternion rotacionPlaneta =
            Quaternion.FromToRotation(transform.up, up) * rb.rotation;

        rb.MoveRotation(
            Quaternion.Slerp(rb.rotation, rotacionPlaneta, 10f * Time.fixedDeltaTime)
        );

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 forward = Vector3.Cross(transform.right, up).normalized;

        Vector3 movimiento =
            transform.right * h * velocidadLateral +
            forward * v * velocidadAdelante;

        rb.velocity = movimiento + Vector3.Project(rb.velocity, up);
    }

    // =========================
    //  MODO NORMAL
    // =========================

    void ModoNormal()
    {
        rb.useGravity = true;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 movimiento =
            Vector3.right * h * velocidadLateral +
            Vector3.forward * v * velocidadAdelante;

        rb.velocity = new Vector3(movimiento.x, rb.velocity.y, movimiento.z);
    }

    // =========================
    // DETECCIÓN PLANETA
    // =========================

    void DetectarPlaneta()
    {
        Planeta[] planetas = FindObjectsOfType<Planeta>();

        float menorDistancia = Mathf.Infinity;
        Planeta masCercano = null;

        foreach (Planeta p in planetas)
        {
            if (p.EstaEnRango(transform.position))
            {
                float distancia = Vector3.Distance(transform.position, p.transform.position);

                if (distancia < menorDistancia)
                {
                    menorDistancia = distancia;
                    masCercano = p;
                }
            }
        }

        planetaActual = masCercano;

        bool enPlanetaAhora = planetaActual != null;

        if (estabaEnPlaneta && !enPlanetaAhora)
        {
            rotacionObjetivo = Quaternion.Euler(0f, 0f, 0f);
            ajustandoRotacion = true;
        }

        enPlaneta = enPlanetaAhora;
        estabaEnPlaneta = enPlanetaAhora;
    }

    void AjustarRotacionSuave()
    {
        if (!ajustandoRotacion) return;

        Quaternion nuevaRotacion = Quaternion.Slerp(
            rb.rotation,
            rotacionObjetivo,
            velocidadAjusteRotacion * Time.fixedDeltaTime
        );

        rb.MoveRotation(nuevaRotacion);

        if (Quaternion.Angle(rb.rotation, rotacionObjetivo) < 0.5f)
        {
            rb.MoveRotation(rotacionObjetivo);
            ajustandoRotacion = false;
        }
    }

    // =========================
    // UTILIDADES
    // =========================

    Vector3 ObtenerUpActual()
    {
        if (superficieActual != null &&
            superficieActual.EstaEnRango(transform.position))
            return superficieActual.ObtenerNormal();

        if (enPlaneta && planetaActual != null)
            return planetaActual.ObtenerUp(transform.position);

        return Vector3.up;
    }

    bool EstaEnSuelo()
    {
        Vector3 direccion = -ObtenerUpActual();
        return Physics.Raycast(transform.position, direccion, distanciaSuelo, capaSuelo);
    }



    

}
