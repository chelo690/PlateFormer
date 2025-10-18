using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Tilemaps;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public Transform[] enemyMovementPoints;
    public GameObject enemyMovement;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform actualObjective;

    public float enemySpeed;
    public float detectionRadius = 0.5f;

    Vector2 movement;

    Animator enemyAnimator;

    public float enemyStrengthX;
    public float enemyStrengthY;
    public float enemyDamage;

    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        enemyAnimator=GetComponent<Animator>();
        actualObjective = enemyMovementPoints[0];
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToObjective = Vector2.Distance(transform.position, actualObjective.position);


        if (distanceToObjective < detectionRadius)
        {
            if (actualObjective == enemyMovementPoints[0])//Llegue al punto A
            {
                actualObjective = enemyMovementPoints[1];
                flip();
            }
            else if(actualObjective == enemyMovementPoints[1])//Llegue al Punto B
            {
                actualObjective = enemyMovementPoints[0];
                flip();
            }
        }
        Vector2 direction=(actualObjective.position - transform.position).normalized;

        int roundedDirection=Mathf.RoundToInt(direction.x);
        movement=new Vector2(roundedDirection, 0);

        rb.MovePosition(rb.position+movement*enemySpeed*Time.deltaTime);

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

        if (collision.gameObject.CompareTag("Player"))
        {
            Movement player=collision.gameObject.GetComponent<Movement>();

            player.TakeDamage(enemyDamage);
            player.hitTime = 0.5f;
            player.hitForceX=enemyStrengthX;
            player.hitForceY=enemyStrengthY;

            if(transform.position.x > player.transform.position.x)
            {
                player.hitFromRight = true;
            }
            else if(transform.position.x < player.transform.position.x)
            {
                player.hitFromRight = false;
            }

            enemyAnimator.SetTrigger("Hit");
        }
    }
}
