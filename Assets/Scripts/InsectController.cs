using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class InsectController : MonoBehaviour
{
    [Header("Baðlantýlar")]
    public Rigidbody2D karakter;        // Karakter Rigidbody2D
    public LineRenderer lineRenderer;   // Böcek child’ý LineRenderer
    public float spriteWidth = 0.2f;    // Tileable sprite geniþliði
    public float width = 0.2f;          // LineRenderer kalýnlýðý

    [Header("Ýp Fizik Ayarlarý")]
    public float maxDistance = 3f;      // Ýpin maksimum uzunluðu
    public float minDistance = 1.5f;    // Ýpin minimum uzunluðu
    public float springForce = 30f;     // Ýpin yay kuvveti
    public float damping = 10f;         // Sönümleme

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
        // Eðer lineRenderer referans verilmemiþse, böceðin child’ýnda arayalým
        if (lineRenderer == null)
            lineRenderer = GetComponentInChildren<LineRenderer>();

        // Material instance oluþtur
        lineRenderer.material = new Material(lineRenderer.material);
    }

    private void FixedUpdate()
    {
        ÝnsectMovementRope();
    }

    public void ÝnsectMovementRope()
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

        // LineRenderer güncelle
        lineRenderer.SetPosition(0, karakter.position);
        lineRenderer.SetPosition(1, rb.position);

        // Sprite’ý mesafeye göre uzat
        lineRenderer.material.mainTextureScale = new Vector2(dist / spriteWidth, 1);
    }
    }
}
