using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Terresquall;

public class JugadorPlaneta : MonoBehaviour
{
    public float velocidadAdelante = 8f;
    public float velocidadLateral = 6f;
    public float fuerzaSalto = 10f;
    public float distanciaSuelo = 1.2f;
    public LayerMask capaSuelo;
    private bool saltoMovil;
    public float velocidadAjusteRotacion = 5f;

    private Rigidbody rb;
    private Planeta planetaActual;
    
    Vector3 empujonDireccion;
    float empujonFuerza;
    float empujonTiempo;
    bool enEmpujon;

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

        if (Input.GetKeyDown(KeyCode.Space) || saltoMovil)
        {
            if (EstaEnSuelo())
            {
                Vector3 up = ObtenerUpActual();

                // Quitamos la velocidad vertical previa
                rb.velocity -= Vector3.Project(rb.velocity, up);

                // Movimiento actual del jugador sobre la superficie
                Vector3 movimientoPlano = Vector3.ProjectOnPlane(rb.velocity, up);

                // Salto base hacia arriba
                Vector3 fuerzaSaltoFinal = up * fuerzaSalto;

                // Si se está moviendo agregamos impulso hacia adelante
                if (movimientoPlano.magnitude > 0.1f)
                {
                    fuerzaSaltoFinal += movimientoPlano.normalized * (fuerzaSalto * 0.4f);
                }

                rb.AddForce(fuerzaSaltoFinal, ForceMode.Impulse);
            }

            saltoMovil = false; // se limpia siempre
        }

    }

    void FixedUpdate()
    {
        if (enEmpujon)
        {
            rb.velocity = empujonDireccion * empujonFuerza;

            empujonTiempo -= Time.fixedDeltaTime;

            if (empujonTiempo <= 0)
            {
                enEmpujon = false;
            }

            return;
        }
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
    void ObtenerInput(out float h, out float v)
    {
        float hTeclado = Input.GetAxis("Horizontal");
        float vTeclado = Input.GetAxis("Vertical");

        float hJoystick = VirtualJoystick.GetAxis("Horizontal");
        float vJoystick = VirtualJoystick.GetAxis("Vertical");

        h = Mathf.Clamp(hTeclado + hJoystick, -1f, 1f);
        v = Mathf.Clamp(vTeclado + vJoystick, -1f, 1f);
    }
    public void BotonSalto()
    {
        saltoMovil = true;
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

        float h, v;
        ObtenerInput(out h, out v);

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

        float h, v;
        ObtenerInput(out h, out v);

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

        float h, v;
        ObtenerInput(out h, out v);

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

    public void RecibirEmpujon(Vector3 direccion, float fuerza, float tiempo)
    {
        empujonDireccion = direccion;
        empujonFuerza = fuerza;
        empujonTiempo = tiempo;
        enEmpujon = true;
    }



}
