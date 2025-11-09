using System.Collections;
using UnityEngine;

public class ColeccionableVelocidad : MonoBehaviour
{
    [Header("Configuración del Power-Up")]
    [Tooltip("Cuánto se multiplica la velocidad del jugador (1.5 = +50%)")]
    public float speedMultiplier = 1.5f;

    [Tooltip("Duración del aumento de velocidad en segundos")]
    public float duration = 5f;

    [Tooltip("Objeto que desaparecerá junto con el coleccionable (opcional)")]
    public GameObject objectDissapear;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Movement player = collision.GetComponent<Movement>();
        if (player != null)
        {
            // Ejecutamos la corutina desde el jugador (no desde este objeto)
            player.StartCoroutine(AumentarVelocidadTemporal(player));
            Desaparecer();
        }
    }

    private IEnumerator AumentarVelocidadTemporal(Movement player)
    {
        float velocidadOriginal = player.speed;
        player.speed *= speedMultiplier;

        yield return new WaitForSeconds(duration);

        player.speed = velocidadOriginal;
    }

    private void Desaparecer()
    {
        if (objectDissapear != null)
            objectDissapear.SetActive(false);

        gameObject.SetActive(false);
    }
}
