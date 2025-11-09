using UnityEngine;

public class Colleccionables : MonoBehaviour
{
    [Header("Configuración del coleccionable")]
    [Tooltip("Cantidad de vida que se recupera al recoger este objeto.")]
    public float healAmount = 5f; // configurable desde el inspector

    [Tooltip("Objeto que desaparecerá junto con el coleccionable (opcional).")]
    public GameObject objectDissapear;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica si el objeto que entra al trigger tiene el script Movement
        Movement player = collision.GetComponent<Movement>();
        if (player != null)
        {
            // Añade vida al jugador usando el método oficial
            player.AddHealth(healAmount);

            // Ejecuta animación o sonido aquí si quieres
            // AudioManager.PlaySFX(pickupSound);

            Desaparecer();
        }
    }

    private void Desaparecer()
    {
        // Desactiva el objeto adicional si existe
        if (objectDissapear != null)
        {
            objectDissapear.SetActive(false);
        }

        // Desactiva este coleccionable
        gameObject.SetActive(false);
    }
}

