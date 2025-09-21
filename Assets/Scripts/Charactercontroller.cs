using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Charactercontroller : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Rigidbody2D rb;
    private bool isGrounded = true;

    [Header("Health Settings")]
    public int maxHealth = 3;
    private int currentHealth;
    public Slider healthSlider;

    [Header("Sprite Settings")]
    public bool isFacingLeft;
    [SerializeField] private SpriteRenderer sr;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip damageClip;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    void Update()
    {
        // Hareket
        float move = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        // Z�plama
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;

            PlaySound(jumpClip,0.1f);
        }

        // Y�z�n� �evirme
        if (move < 0 && !isFacingLeft)
        {
            isFacingLeft = true;
            sr.flipX = true;
        }
        else if (move > 0 && isFacingLeft)
        {
            isFacingLeft = false;
            sr.flipX = false;
        }

        // B�cek y�n�n� ayarla
        GameObject Bocek = GameObject.FindGameObjectWithTag("Bocek");
        if (Bocek != null)
        {
            if (transform.position.x < Bocek.transform.position.x)
                Bocek.GetComponentInChildren<SpriteRenderer>().flipX = true;
            else
                Bocek.GetComponentInChildren<SpriteRenderer>().flipX = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Yere temas kontrol�
        if (collision.contacts[0].normal.y > 0.5f)
            isGrounded = true;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        PlaySound(damageClip,0.2f);

        Debug.Log("Oyuncu damage ald�! Kalan can: " + currentHealth);

        if (healthSlider != null)
            healthSlider.value = currentHealth;

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Debug.Log("Oyuncu �ld�!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // Oyuncu yeniden do�du�unda can� resetle
        currentHealth = maxHealth;
        if (healthSlider != null)
            healthSlider.value = currentHealth;
    }

    private void PlaySound(AudioClip clip, float volume)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.volume = volume;
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
