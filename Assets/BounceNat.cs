using UnityEngine;
using System.Collections;

public class BounceNat : MonoBehaviour
{
    private Animator anim;
    private bool isBouncing = false;

    [SerializeField] private float bounceForce = 3f; // fuerza fija del rebote
    [SerializeField] private float bounceDuration = 0.2f; // tiempo que el jugador no puede moverse

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica si el objeto pertenece al grupo "Jugadores"
        if (collision.transform.parent != null && collision.transform.parent.name == "Jugadores")
        {
            if (collision.gameObject.name == "Jose Maria" || collision.gameObject.name == "Maria Jose")
            {
                Rigidbody2D rb = collision.rigidbody;
                if (rb != null)
                {
                    // Elimina cualquier velocidad vertical previa
                    rb.velocity = new Vector2(rb.velocity.x, 0f);

                    // Aplica una fuerza hacia arriba constante
                    rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);

                    // Desactiva movimiento temporalmente
                    Movement mv = rb.GetComponent<Movement>();
                    if (mv != null)
                        StartCoroutine(TemporarilyDisableMovement(mv));
                }

                // 🔹 Controla la animación solo una vez por rebote
                if (!isBouncing)
                    StartCoroutine(DoBounce());
            }
        }
    }

    private IEnumerator DoBounce()
    {
        isBouncing = true;
        anim.SetBool("isStep", true);
        yield return new WaitForSeconds(0.4f); // duración de la animación
        anim.SetBool("isStep", false);
        isBouncing = false;
    }

    private IEnumerator TemporarilyDisableMovement(Movement mv)
    {
        mv.isBounced = true;
        yield return new WaitForSeconds(bounceDuration);
        mv.isBounced = false;
    }
}
