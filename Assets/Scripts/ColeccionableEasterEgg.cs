using UnityEngine;
using UnityEngine.UI;
using TMPro; // Si usas TextMeshPro (recomendado)

public class ColeccionableEasterEgg : MonoBehaviour
{
    [Header("UI a mostrar")]
    public GameObject imagenUI;
    public TMP_Text textoUI; // Si prefieres usar un Text normal, cámbialo a 'public Text textoUI;'

    [Header("Configuración opcional")]
    public string mensaje = "¡Has encontrado un Easter Egg!";
    public bool destruirAlRecoger = true;
    public float tiempoVisible = 2.5f; // Segundos visibles

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Mostrar imagen y texto
            if (imagenUI != null)
                imagenUI.SetActive(true);

            if (textoUI != null)
            {
                textoUI.gameObject.SetActive(true);
                textoUI.text = mensaje;
            }

            // Ocultar después del tiempo establecido
            Invoke(nameof(OcultarUI), tiempoVisible);

            // Destruir o desactivar el coleccionable
            if (destruirAlRecoger)
                Destroy(gameObject);
        }
    }

    private void OcultarUI()
    {
        if (imagenUI != null)
            imagenUI.SetActive(false);

        if (textoUI != null)
            textoUI.gameObject.SetActive(false);
    }
}
