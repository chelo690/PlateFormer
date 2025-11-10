using System.Collections;
using UnityEngine;

public class Seguirjugador : MonoBehaviour
{
    [Header("Movimiento y detección")]
    public float velocidadMovimiento = 3f;
    public float radioBusqueda = 10f;
    public float distanciaMaxima = 30f;
    public LayerMask capaJugador;
    private Transform transformJugador;
    private Vector2 puntoInicial;
    private bool mirandoDerecha = true;

    [Header("Salto y terreno")]
    public float fuerzaSalto = 7f;
    public LayerMask capaSuelo;
    public Transform groundCheck;
    public Transform obstacleCheck;
    public float distanciaSuelo = 0.5f;
    public float distanciaObstaculo = 1f;

    [Header("Ataque")]
    public float fuerzaEmpujeX = 5f;
    public float fuerzaEmpujeY = 3f;
    public float daño = 20f;
    private float dañoOriginal;

    [Header("Referencias")]
    public Rigidbody2D rb2D;
    public Animator animator;

    public enum EstadoEnemy { Esperando, Siguiendo, Volviendo }
    public EstadoEnemy estadoActual = EstadoEnemy.Esperando;

    private void Start()
    {
        puntoInicial = transform.position;
        dañoOriginal = daño;
    }

    private void Update()
    {
        switch (estadoActual)
        {
            case EstadoEnemy.Esperando:
                EstadoEsperando();
                break;
            case EstadoEnemy.Siguiendo:
                EstadoSiguiendo();
                break;
            case EstadoEnemy.Volviendo:
                EstadoVolviendo();
                break;
        }

        // Raycasts para debug
        if (groundCheck != null)
            Debug.DrawRay(groundCheck.position, Vector2.down * distanciaSuelo, Color.yellow);
        if (obstacleCheck != null)
        {
            Vector2 dir = mirandoDerecha ? Vector2.right : Vector2.left;
            Debug.DrawRay(obstacleCheck.position, dir * distanciaObstaculo, Color.magenta);
        }
    }

    // ---------- ESTADOS PRINCIPALES ----------

    private void EstadoEsperando()
    {
        Collider2D jugador = Physics2D.OverlapCircle(transform.position, radioBusqueda, capaJugador);
        if (jugador)
        {
            transformJugador = jugador.transform;
            estadoActual = EstadoEnemy.Siguiendo;
        }
    }

    private void EstadoSiguiendo()
    {
        if (transformJugador == null)
        {
            estadoActual = EstadoEnemy.Volviendo;
            return;
        }

        animator.SetBool("Inmovement", true);
        GirarAObjetivo(transformJugador.position);
        transform.position = Vector2.MoveTowards(transform.position, transformJugador.position, velocidadMovimiento * Time.deltaTime);

        // Salto automático ante obstáculo
        if (HayObstaculo() && HaySuelo())
            rb2D.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);

        // Si se aleja demasiado, volver al punto inicial
        if (Vector2.Distance(transform.position, puntoInicial) > distanciaMaxima)
        {
            transformJugador = null;
            estadoActual = EstadoEnemy.Volviendo;
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
            estadoActual = EstadoEnemy.Esperando;
        }
    }

    // ---------- ATAQUE ----------
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        Movement player = collision.gameObject.GetComponent<Movement>();
        if (player == null || player.Health <= 0f) return;

        float dañoFinal = daño;
        player.TakeDamage(dañoFinal);

        // Retroceso hacia el jugador
        player.hitTime = 0.5f;
        player.hitForceX = fuerzaEmpujeX;
        player.hitForceY = fuerzaEmpujeY;
        player.hitFromRight = transform.position.x > player.transform.position.x;

        if (animator != null)
            animator.SetTrigger("Hit");
    }

    // ---------- FUNCIONES AUXILIARES ----------

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
        if (objetivo.x > transform.position.x && !mirandoDerecha)
            Girar();
        else if (objetivo.x < transform.position.x && mirandoDerecha)
            Girar();
    }

    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        transform.Rotate(0f, 180f, 0f);
    }

    // ---------- BUFF DE DEFENSA ----------
    public void AplicarBuffDefensa(float reduccion, float duracion)
    {
        StopAllCoroutines();
        StartCoroutine(ReducirDañoTemporal(reduccion, duracion));
    }

    private IEnumerator ReducirDañoTemporal(float reduccion, float duracion)
    {
        daño = Mathf.Max(0, dañoOriginal - reduccion);
        yield return new WaitForSeconds(duracion);
        daño = dañoOriginal;
    }

    // ---------- DEBUG VISUAL ----------
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioBusqueda);
        Gizmos.DrawWireSphere(puntoInicial, distanciaMaxima);
    }
}

