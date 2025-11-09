using System.Collections;
using UnityEngine;

public class ColeccionableDefensa : MonoBehaviour
{
    [Header("Configuración del Power-Up")]
    [Tooltip("Cantidad de daño que se resta temporalmente a los enemigos")]
    public float damageReduction = 5f;

    [Tooltip("Duración del efecto en segundos")]
    public float duration = 5f;

    [Tooltip("Objeto que desaparecerá junto con el coleccionable (opcional)")]
    public GameObject objectDissapear;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Movement player = collision.GetComponent<Movement>();
        if (player != null)
        {
            // Aplicamos la reducción temporal a todos los enemigos actuales
            EnemyController[] enemigos = FindObjectsOfType<EnemyController>();
            foreach (var e in enemigos)
            {
                e.AplicarBuffDefensa(damageReduction, duration);
            }

            Desaparecer();
        }
    }

    private void Desaparecer()
    {
        if (objectDissapear != null)
            objectDissapear.SetActive(false);

        gameObject.SetActive(false);
    }
}




