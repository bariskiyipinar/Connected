using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class InsectController : MonoBehaviour
{
    [Header("Baðlantýlar")]
    public Rigidbody2D karakter;
    public LineRenderer lineRenderer;
    public float width = 0.2f;

    [Header("Ýp Fizik Ayarlarý")]
    public float maxDistance = 3f;
    public float minDistance = 1.5f;
    public float springForce = 50f;
    public float damping = 15f;

    [Header("Hareket Ayarlarý")]
    public float moveSpeed = 10f;
    public float jumpForce = 8f; // Zýplama kuvveti

    private Rigidbody2D rb;
    private bool canControl = false;
    private Animator animator;
    private SpriteRenderer sr;

    public void EnableControl(bool value)
    {
        canControl = value;

        if (!value)
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("Insect_Walking", false);
        }
    }

    private void Update()
    {
        if (canControl)
        {
            float move = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

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

          
            if (Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(rb.velocity.y) < 0.01f)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
             
            }
        }

      
        if (Input.GetKey(KeyCode.Q))
        {
            Vector2 targetPos = karakter.position;
            float step = 5f * Time.deltaTime;
            rb.position = Vector2.MoveTowards(rb.position, targetPos, step);
            rb.velocity = Vector2.zero;
        }
    }

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        rb.mass = 1.5f;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        if (lineRenderer == null)
            lineRenderer = GetComponentInChildren<LineRenderer>();

        lineRenderer.material = new Material(lineRenderer.material);
        lineRenderer.textureMode = LineTextureMode.Stretch;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
    }

    private void FixedUpdate()
    {
        ÝnsectMovementRope();
    }

    public void ÝnsectMovementRope()
    {
        if (!canControl)
        {
            Vector2 dir = karakter.position - rb.position;
            float dist = dir.magnitude;

            float stretch = 0f;
            if (dist > maxDistance) stretch = dist - maxDistance;
            else if (dist < minDistance) stretch = dist - minDistance;

            Vector2 force = dir.normalized * springForce * stretch;
            force -= rb.velocity * damping;

            rb.AddForce(force);

            lineRenderer.SetPosition(0, karakter.position);
            lineRenderer.SetPosition(1, rb.position);
        }
    }

  
}
