using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
   

public class Movement1 : MonoBehaviour
{
    public float speed = 5f;
    public float Horizontal;
    public float JumpForce = 5f;

    public Transform groundcheck;
    public LayerMask groundLayer;
    public float groundRadius;

    public float Coins;

    public Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(Horizontal * speed, rb.velocity.y);
        rb.AddForce(new Vector2(Horizontal * speed, 0));
    }

    public void Move(InputAction.CallbackContext context)
    {
        Horizontal = context.ReadValue<Vector2>().x;
    }
    public void Jump(InputAction.CallbackContext context)
    {

        if (context.performed == true && OnGrounded())
        {

            rb.velocity = new Vector2(rb.velocity.x, JumpForce);

        }   
    }

    public bool OnGrounded()
    {
        return Physics2D.OverlapCircle(groundcheck.position, groundRadius, groundLayer);
    }

}
