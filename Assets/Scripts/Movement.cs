using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;




public class Movement : MonoBehaviour
{

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }


    public float speed = 5f;
    public float Horizontal;
    public float JumpForce = 5f;
    public float Vertical;

    public Transform groundcheck;
    public LayerMask groundLayer;
    public float groundRadius;

    [SerializeField]public float MaxHealth = 100f;
    public float Health;

    public float hitTime;
    public float hitForceX;
    public float hitForceY;
    public bool hitFromRight;

    [SerializeField] public float playerHealth;

    [SerializeField] private Animator PlayerAnimator;

    public TextMeshProUGUI healthText;

    private bool isFacingRight;

    public Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        PlayerAnimator = GetComponent<Animator>();

        Health=MaxHealth;

        healthText.text = $"Health: {Health}/{MaxHealth}";
    }

    // Update is called once per frame
    void Update()
    {

        rb.AddForce(new Vector2(Horizontal * speed, 0));

        PlayerAnimator.SetFloat("Direction",Horizontal);
        Vertical = rb.velocity.y;
        PlayerAnimator.SetFloat("High", Vertical);

        if (isFacingRight == false && Horizontal < 0f)
        {
            flip();
        }
        else if(isFacingRight == true && Horizontal > 0f)
        {
            flip();
        }

        if(hitTime<=0f)
        {
            rb.velocity = new Vector2(Horizontal * speed, rb.velocity.y);
        }
        else
        {
           if(hitFromRight)
           {
                rb.AddForce(new Vector2(-hitForceX, hitForceY));
           }
           else if(!hitFromRight==false)
           {
                rb.AddForce(new Vector2(hitForceX, hitForceY));
           }
           hitTime-= Time.deltaTime;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        Horizontal = context.ReadValue<Vector2>().x;
    }
    public void Jump(InputAction.CallbackContext context)
    {

        if (context.performed == true && OnGrounded())
        {
            audioManager.PlaySFX(audioManager.Jump);
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);

            //PlayerAnimator.SetFloat("High", rb.);
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
        if (Health - damage > MaxHealth)
        {
            Health -= MaxHealth;
        }
        else
        {
            Health -= damage;
        }
        healthText.text = $"Health: {Health}/{MaxHealth}";
        
    }

    public void AddHealth(float _health)
    {
        if (Health + _health > MaxHealth)
        {
            Health = MaxHealth;
        }
        else
        {
            Health += _health;
        }
        healthText.text = $"Health: {Health}/{MaxHealth}";
    }
}

