using System.Collections;
using UnityEngine;

public class ColeccionableSalto : MonoBehaviour
{
    [Header("Configuración del Power-Up")]
    [Tooltip("Cuánto se multiplica la fuerza de salto del jugador (1.5 = +50%)")]
    public float jumpMultiplier = 1.5f;

    [Tooltip("Duración del aumento de salto en segundos")]
    public float duration = 5f;

    [Tooltip("Objeto que desaparecerá junto con el coleccionable (opcional)")]
    public GameObject objectDissapear;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Movement player = collision.GetComponent<Movement>();
        if (player != null)
        {
            player.StartCoroutine(AumentarSaltoTemporal(player));
            Desaparecer();
        }
    }

    private IEnumerator AumentarSaltoTemporal(Movement player)
    {
        float saltoOriginal = player.JumpForce; // <- fijate que aquí usamos la J mayúscula
        player.JumpForce *= jumpMultiplier;

        yield return new WaitForSeconds(duration);

        player.JumpForce = saltoOriginal;
    }

    private void Desaparecer()
    {
        if (objectDissapear != null)
            objectDissapear.SetActive(false);

        gameObject.SetActive(false);
    }
}


