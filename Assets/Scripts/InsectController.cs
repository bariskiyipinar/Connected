using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class InsectController : MonoBehaviour
{
    [Header("Ba�lant�lar")]
    public Rigidbody2D karakter;        // Karakter Rigidbody2D
    public LineRenderer lineRenderer;   // B�cek child�� LineRenderer
    public float spriteWidth = 0.2f;    // Tileable sprite geni�li�i
    public float width = 0.2f;          // LineRenderer kal�nl���

    [Header("�p Fizik Ayarlar�")]
    public float maxDistance = 3f;      // �pin maksimum uzunlu�u
    public float minDistance = 1.5f;    // �pin minimum uzunlu�u
    public float springForce = 30f;     // �pin yay kuvveti
    public float damping = 10f;         // S�n�mleme

    private Rigidbody2D rb;

    public float moveSpeed = 10f;
    private bool canControl = false;
    private Animator animator;

    SpriteRenderer sr;


    public void EnableControl(bool value)
    {
        canControl = value;


        if (!value)
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("Insect_Walking",false); 
        }

    }

    private void Update()
    {
        if (canControl)
        {
            float move = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);
            float moveX = rb.velocity.x;

            if (move > 0)
            {
                sr.flipX = false;
                animator.SetBool("Insect_Walking", true);
            }
            else if (move < 0)
            {
                sr.flipX = true;
                animator.SetBool("Insect_Walking", true);
            }
            else
            {
                animator.SetBool("Insect_Walking", false);
            }
             



        }
    }

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        // E�er lineRenderer referans verilmemi�se, b�ce�in child��nda arayal�m
        if (lineRenderer == null)
            lineRenderer = GetComponentInChildren<LineRenderer>();

        // Material instance olu�tur
        lineRenderer.material = new Material(lineRenderer.material);
    }

    private void FixedUpdate()
    {
        �nsectMovementRope();
    }

    public void �nsectMovementRope()
    {
        if (!canControl) { 
        Vector2 dir = karakter.position - rb.position;
        float dist = dir.magnitude;

        // Mesafeyi clamp et
        if (dist > maxDistance)
        {
            Vector2 targetPos = karakter.position - dir.normalized * maxDistance;
            rb.position = targetPos;
            rb.velocity = Vector2.zero;
        }
        else if (dist < minDistance)
        {
            Vector2 targetPos = karakter.position - dir.normalized * minDistance;
            rb.position = targetPos;
            rb.velocity = Vector2.zero;
        }

        // LineRenderer g�ncelle
        lineRenderer.SetPosition(0, karakter.position);
        lineRenderer.SetPosition(1, rb.position);

        // Sprite�� mesafeye g�re uzat
        lineRenderer.material.mainTextureScale = new Vector2(dist / spriteWidth, 1);
    }
    }
}
