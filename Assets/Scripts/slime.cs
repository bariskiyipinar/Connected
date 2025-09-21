using UnityEngine;

public class Slime : MonoBehaviour
{
    public float speed = 2f;
    public Transform pointA;
    public Transform pointB;
    private Transform target;
    public int damage = 1;
    [SerializeField] private GameObject SlimeDeadEffect;
    private bool isSwitching = false; // kilit
     private AudioSource SlimeDeadSound;
    void Start()
    {
        target = pointB;
        SlimeDeadSound=GetComponent<AudioSource>();
    }

    void Update()
    {
        // sadece X ekseninde hareket et
        Vector3 current = transform.position;
        Vector3 targetPos = new Vector3(target.position.x, current.y, current.z);

        transform.position = Vector3.MoveTowards(current, targetPos, speed * Time.deltaTime);

        // hedefe ulaþtý mý?
        if (!isSwitching && Mathf.Abs(transform.position.x - target.position.x) < 0.05f)
        {
            isSwitching = true;
            target = (target == pointB) ? pointA : pointB;
            Flip();
            Invoke(nameof(ResetSwitch), 0.1f); // 0.1 sn sonra kilidi aç
        }
    }

    void ResetSwitch()
    {
        isSwitching = false;
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Charactercontroller player = collision.gameObject.GetComponent<Charactercontroller>();
            if (player != null )
            {
                player.TakeDamage(damage);
             
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Bocek"))
        {
            Debug.Log("Oyuncu slime'ýn üstüne bastý!");
            SlimeDeadSound.Play();
          
            if (SlimeDeadEffect != null)
            {
                GameObject p = Instantiate(SlimeDeadEffect, transform.position, Quaternion.identity);
                ParticleSystem ps = p.GetComponent<ParticleSystem>();
                if (ps != null) ps.Play();
            }


            Destroy(gameObject, 0.1f);
        }
    }
}
