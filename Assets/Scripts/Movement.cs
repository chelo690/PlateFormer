using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Movement : MonoBehaviour
{
    AudioManager audioManager;
    private bool muerto = false;

    [Header("Movimiento")]
    public float speed = 5f;
    public float Horizontal;
    public float JumpForce = 5f;
    public float Vertical;
    public Rigidbody2D rb;

    [Header("Suelo")]
    public Transform groundcheck;
    public LayerMask groundLayer;
    public float groundRadius = 0.2f;

    [Header("Vida")]
    [SerializeField] public float MaxHealth = 100f;
    public float Health;
    [SerializeField] public float playerHealth;

    [Header("Golpes")]
    public float hitTime;
    public float hitForceX = 5f;
    public float hitForceY = 5f;
    public bool hitFromRight;

    [Header("Animador y UI")]
    [SerializeField] private Animator PlayerAnimator;
    public TextMeshProUGUI healthText;

    // Si tu sprite base mira a la DERECHA, deja esto en true.
    private bool isFacingRight = true;

    private void Awake()
    {
        var audioObj = GameObject.FindGameObjectWithTag("Audio");
        if (audioObj != null)
            audioManager = audioObj.GetComponent<AudioManager>();
    }

    void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
        Health = MaxHealth;
        playerHealth = Health;
        UpdateHealthText();
    }

    void Update()
    {
        // 🔒 Si está muerto, no se mueve ni salta.
        if (muerto)
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 1f;
            if (PlayerAnimator != null)
                PlayerAnimator.SetBool("IsDeath", true); // <-- nombre corregido
            PlayerAnimator.SetBool("IsDeath",muerto);
            return;
        }

        // Movimiento directo (sin AddForce -> evita deslizamiento)
        rb.velocity = new Vector2(Horizontal * speed, rb.velocity.y);

        // Actualizar animaciones
        if (PlayerAnimator != null)
        {
            PlayerAnimator.SetFloat("Direction", Mathf.Abs(Horizontal)); // evita moonwalk
            Vertical = rb.velocity.y;
            PlayerAnimator.SetFloat("High", Vertical);
        }

        // Flip corregido
        if (Horizontal > 0 && !isFacingRight)
            flip();
        else if (Horizontal < 0 && isFacingRight)
            flip();

        // Control de golpes
        if (hitTime > 0f)
            hitTime -= Time.deltaTime;
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (muerto) return;
        Horizontal = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (muerto) return;

        if (context.performed)
        {
            Debug.Log("Salto detectado, OnGrounded = " + OnGrounded());
        }

        if (context.performed && OnGrounded())
        {
            if (audioManager != null)
                audioManager.PlaySFX(audioManager.Jump);

            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
        }
    }

    public bool OnGrounded()
    {
        return Physics2D.OverlapCircle(groundcheck.position, groundRadius, groundLayer);
    }

    private void flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void TakeDamage(float damage)
    {
        // 🚫 Si ya está muerto, no se puede dañar ni empujar
        if (muerto) return;

        Health -= damage;
        Health = Mathf.Clamp(Health, 0f, MaxHealth);
        playerHealth = Health;
        UpdateHealthText();

        // 🎵 Sonido al recibir daño (si no muere todavía)
        if (audioManager != null && Health > 0f)
            audioManager.PlaySFX(audioManager.Damage);

        if (Health <= 0f && !muerto)
        {
            muerto = true;
            rb.velocity = Vector2.zero;
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            if (PlayerAnimator != null)
                PlayerAnimator.SetBool("IsDeath", true); // <-- nombre corregido

            // 🎵 Efecto de sonido de muerte
            if (audioManager != null)
                audioManager.PlaySFX(audioManager.DeathMasc);

            StartCoroutine(Muerte());

            if (GameManager.instance != null)
            {
                GameManager.instance.GameOver();
            }
        }
        else
        {
            // Solo aplicar retroceso si aún está vivo
            if (hitTime <= 0f)
            {
                hitTime = 0.2f; // duración breve del retroceso
                if (hitFromRight)
                    rb.AddForce(new Vector2(-hitForceX, hitForceY), ForceMode2D.Impulse);
                else
                    rb.AddForce(new Vector2(hitForceX, hitForceY), ForceMode2D.Impulse);
            }
        }
    }

    public void AddHealth(float _health)
    {
        if (muerto) return;

        Health = Mathf.Min(Health + _health, MaxHealth);
        playerHealth = Health;
        UpdateHealthText();
    }

    private void UpdateHealthText()
    {
        if (healthText != null)
            healthText.text = $"Health: {Health}/{MaxHealth}";
    }

    IEnumerator Muerte()
    {
        // Espera breve para permitir animación
        yield return new WaitForSeconds(1.2f);

        // Desactiva completamente el Rigidbody para evitar rebotes
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        // Llama al GameManager
        if (GameManager.instance != null)
            GameManager.instance.GameOver();
        else
            Debug.LogWarning("⚠️ GameManager no encontrado en la escena.");
    }
}