using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seguirjugador : MonoBehaviour
{
    [Header("Movimiento")]
    public float radioBusqueda = 5f;
    public LayerMask capaJugador;
    public Transform transformJugador;
    public float velocidadMovimiento = 3f;
    public float distanciaMaxima = 40f;
    private Vector2 puntoInicial;
    private bool mirandoDerecha = true;

    [Header("Salto")]
    public float fuerzaSalto = 7f;
    public LayerMask capaSuelo;
    public Transform groundCheck;
    public Transform obstacleCheck;
    public float distanciaSuelo = 0.5f;
    public float distanciaObstaculo = 1f;

    [Header("Referencias")]
    public Rigidbody2D rb2D;
    public Animator animator;

    public EstadosMovimiento estadoActual;

    public enum EstadosMovimiento
    {
        Esperando,
        Siguiendo,
        Volviendo
    }

    private void Start()
    {
        puntoInicial = transform.position;
    }

    private void Update()
    {
        // Dibuja rayos visibles
        if (groundCheck != null)
            Debug.DrawRay(groundCheck.position, Vector2.down * distanciaSuelo, Color.yellow);
        if (obstacleCheck != null)
        {
            Vector2 dir = mirandoDerecha ? Vector2.right : Vector2.left;
            Debug.DrawRay(obstacleCheck.position, dir * distanciaObstaculo, Color.magenta);
        }

        switch (estadoActual)
        {
            case EstadosMovimiento.Esperando:
                EstadoEsperando();
                break;
            case EstadosMovimiento.Siguiendo:
                EstadoSiguiendo();
                break;
            case EstadosMovimiento.Volviendo:
                EstadoVolviendo();
                break;
        }

        // Debug
        Debug.Log(HaySuelo() ? "✅ Tocando suelo" : "🟥 No está tocando el suelo");
        Debug.Log(HayObstaculo() ? "🧱 Obstáculo detectado" : "➡️ Sin obstáculo al frente");
    }

    private void EstadoEsperando()
    {
        Collider2D jugadorCollider = Physics2D.OverlapCircle(transform.position, radioBusqueda, capaJugador);
        if (jugadorCollider)
        {
            transformJugador = jugadorCollider.transform;
            estadoActual = EstadosMovimiento.Siguiendo;
        }
    }

    private void EstadoSiguiendo()
    {
        if (transformJugador == null)
        {
            estadoActual = EstadosMovimiento.Volviendo;
            return;
        }

        animator.SetBool("Inmovement", true);
        GirarAObjetivo(transformJugador.position);
        transform.position = Vector2.MoveTowards(transform.position, transformJugador.position, velocidadMovimiento * Time.deltaTime);

        // Salto ante obstáculo
        if (HayObstaculo() && HaySuelo())
        {
            rb2D.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
        }

        if (Vector2.Distance(transform.position, puntoInicial) > distanciaMaxima)
        {
            estadoActual = EstadosMovimiento.Volviendo;
            transformJugador = null;
        }
    }

    private void EstadoVolviendo()
    {
        transform.position = Vector2.MoveTowards(transform.position, puntoInicial, velocidadMovimiento * Time.deltaTime);
        GirarAObjetivo(puntoInicial);
        if (Vector2.Distance(transform.position, puntoInicial) < 0.1f)
        {
            rb2D.velocity = Vector2.zero;
            animator.SetBool("Inmovement", false);
            estadoActual = EstadosMovimiento.Esperando;
        }
    }

    private bool HaySuelo()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, distanciaSuelo, capaSuelo);
        return hit.collider != null;
    }

    private bool HayObstaculo()
    {
        Vector2 dir = mirandoDerecha ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(obstacleCheck.position, dir, distanciaObstaculo, capaSuelo);
        return hit.collider != null;
    }

    private void GirarAObjetivo(Vector2 objetivo)
    {
        if (objetivo.x > transform.position.x && !mirandoDerecha) Girar();
        else if (objetivo.x < transform.position.x && mirandoDerecha) Girar();
    }

    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        transform.Rotate(0f, 180f, 0f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioBusqueda);
        Gizmos.DrawWireSphere(puntoInicial, distanciaMaxima);
    }
}
