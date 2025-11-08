using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform[] enemyMovementPoints;
    public GameObject enemyMovement;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform actualObjective;

    public float enemySpeed = 2f;
    public float detectionRadius = 0.5f;

    Vector2 movement;
    Animator enemyAnimator;

    [Header("Ataque")]
    public float enemyStrengthX = 5f;
    public float enemyStrengthY = 3f;
    public float enemyDamage = 20f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();
        actualObjective = enemyMovementPoints[0];
    }

    void Update()
    {
        float distanceToObjective = Vector2.Distance(transform.position, actualObjective.position);

        if (distanceToObjective < detectionRadius)
        {
            if (actualObjective == enemyMovementPoints[0])
            {
                actualObjective = enemyMovementPoints[1];
                flip();
            }
            else if (actualObjective == enemyMovementPoints[1])
            {
                actualObjective = enemyMovementPoints[0];
                flip();
            }
        }

        Vector2 direction = (actualObjective.position - transform.position).normalized;
        int roundedDirection = Mathf.RoundToInt(direction.x);
        movement = new Vector2(roundedDirection, 0);

        rb.MovePosition(rb.position + movement * enemySpeed * Time.deltaTime);

        enemyAnimator.SetFloat("Direction", roundedDirection);
    }

    private void flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        Movement player = collision.gameObject.GetComponent<Movement>();
        if (player == null) return;

        // ⚠️ Si el jugador está muerto, no hacer nada
        if (playerHealthCero(player)) return;

        // Aplica daño
        player.TakeDamage(enemyDamage);

        // Aplica retroceso solo si sigue vivo
        player.hitTime = 0.5f;
        player.hitForceX = enemyStrengthX;
        player.hitForceY = enemyStrengthY;

        // Determinar dirección del golpe
        player.hitFromRight = transform.position.x > player.transform.position.x;

        // Animación del enemigo al atacar
        if (enemyAnimator != null)
            enemyAnimator.SetTrigger("Hit");
    }

    private bool playerHealthCero(Movement player)
    {
        // Retorna true si el jugador ya murió o su salud es 0
        return player == null || player.Health <= 0f;
    }
}

