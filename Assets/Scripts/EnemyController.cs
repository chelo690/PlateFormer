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
    public float enemyDamage = 20f; // daño base
    private float originalDamage;    // para restaurar el daño después del buff

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();
        actualObjective = enemyMovementPoints[0];

        originalDamage = enemyDamage; // guardamos el valor original
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

        if (enemyAnimator != null)
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
        if (player == null || player.Health <= 0f) return;

        float dañoFinal = enemyDamage; // usamos el daño actual, que puede estar reducido

        player.TakeDamage(dañoFinal);

        // Aplicamos retroceso
        player.hitTime = 0.5f;
        player.hitForceX = enemyStrengthX;
        player.hitForceY = enemyStrengthY;
        player.hitFromRight = transform.position.x > player.transform.position.x;

        if (enemyAnimator != null)
            enemyAnimator.SetTrigger("Hit");
    }

    // Método público para aplicar reducción temporal de daño
    public void AplicarBuffDefensa(float reduction, float duration)
    {
        StopAllCoroutines(); // cancelamos buffs previos si hay
        StartCoroutine(ReducirDañoTemporal(reduction, duration));
    }

    private IEnumerator ReducirDañoTemporal(float reduction, float duration)
    {
        enemyDamage = Mathf.Max(0, originalDamage - reduction); // aplicamos reducción
        yield return new WaitForSeconds(duration);
        enemyDamage = originalDamage; // restauramos daño original
    }
}
