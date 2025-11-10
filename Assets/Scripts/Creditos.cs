using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CreditsScroll : MonoBehaviour
{
    [Header("Configuración del scroll")]
    public float scrollSpeed = 50f; // Velocidad del movimiento
    public float endYPosition = 1000f; // Hasta dónde sube
    public bool startAutomatically = true;
    float timer = 0;    
    private RectTransform rectTransform;
    private bool isScrolling = false;
    [SerializeField] float EndTimer = 0;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        //if (startAutomatically)
        //    StartScrolling();
    }

    void Update()
    {
        
       rectTransform.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
        timer += Time.deltaTime;

        Debug.Log(timer);

        if(timer > EndTimer) 
        {

            SceneManager.LoadScene("Final");

        
        }
    }
}

