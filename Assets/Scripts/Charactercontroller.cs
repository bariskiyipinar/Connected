using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charactercontroller : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Rigidbody2D rb;
    private bool isGrounded = true;

    public bool isFacingLeft;
    [SerializeField] private SpriteRenderer sr;


    void Update()
    {
      
        float move = Input.GetAxisRaw("Horizontal"); 
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);
        float moveX=rb.velocity.x;

        
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
        }


        if (moveX < 0 && !isFacingLeft)
        {
            // sola dön
            isFacingLeft = true;
            sr.flipX = true;
        }
        else if (moveX > 0 && isFacingLeft)
        {
            // saða dön
            isFacingLeft = false;
            sr.flipX = false;
       
        }

        GameObject Bocek = GameObject.FindGameObjectWithTag("Bocek");

        if(transform.position.x < Bocek.transform.position.x)
        {
            Bocek.GetComponentInChildren<SpriteRenderer>().flipX = true;

        }
        else
        {
            Bocek.GetComponentInChildren<SpriteRenderer>().flipX = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Basit yerde temas kontrolü
        if (collision.contacts[0].normal.y > 0.5f)
            isGrounded = true;
    }
}
