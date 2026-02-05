using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Ayarlar")]
    [SerializeField] private Transform noktaA; // Baþlangýç noktasý
    [SerializeField] private Transform noktaB; // Bitiþ noktasý
    [SerializeField] private float hiz = 3.0f; // Hareket hýzý

    private Vector3 hedefNokta;

    private void Start()
    {
        // Oyuna baþladýðýnda ilk hedef B noktasý olsun
        hedefNokta = noktaB.position;
        // Platformun pozisyonunu A noktasýna eþitleyelim (Garanti olsun)
        transform.position = noktaA.position;
    }

    private void Update()
    {
        // Platformu hedef noktaya doðru hareket ettir
        transform.position = Vector3.MoveTowards(transform.position, hedefNokta, hiz * Time.deltaTime);

        // Platform hedefe çok yaklaþtýysa (0.05 birim), hedefi deðiþtir
        if (Vector2.Distance(transform.position, noktaA.position) < 0.05f)
        {
            hedefNokta = noktaB.position;
        }
        else if (Vector2.Distance(transform.position, noktaB.position) < 0.05f)
        {
            hedefNokta = noktaA.position;
        }
    }

    // Editörde A ve B noktalarý arasýndaki yolu çizgi olarak gösterir
    private void OnDrawGizmos()
    {
        if (noktaA != null && noktaB != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(noktaA.position, noktaB.position);
            Gizmos.DrawWireSphere(noktaA.position, 0.3f);
            Gizmos.DrawWireSphere(noktaB.position, 0.3f);
        }
    }

    // Karakter platformun üstündeyken kaymamasý için
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(this.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}