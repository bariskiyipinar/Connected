using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSwitch : MonoBehaviour
{
    public Charactercontroller character;   // Karakter script
    public InsectController bocek;          // Böcek script
    public float controlTime = 5f;          // Böceði kontrol süresi
    [SerializeField] private GameObject DespawnParticle;

    private bool controllingBocek = false;


    //UI Insect Time Out Show
    [SerializeField] private Image InsectTimeÝmage;
    int insectTime;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !controllingBocek)
        {
            StartCoroutine(ControlBocek());
        }
    }

    IEnumerator ControlBocek()
    {
        controllingBocek = true;

        // --- Karakter küçülüp kayboluyor ---
        yield return StartCoroutine(ScaleOverTime(character.transform, Vector3.one, Vector3.zero, 0.5f));

        // Particle efekti
        if (DespawnParticle != null)
        {
            GameObject p = Instantiate(DespawnParticle, character.transform.position, Quaternion.identity);
            ParticleSystem ps = p.GetComponent<ParticleSystem>();
            if (ps != null)
                ps.Play();  // Prefab Play on Awake kapalý olsa bile çalýþtýrýr
        }

        character.gameObject.SetActive(false);

        // Böcek aktif
        bocek.EnableControl(true);
        bocek.lineRenderer.enabled = false;

        // UI sýfýrla
        InsectTimeÝmage.fillAmount = 1f;

        float elapsed = 0f;
        while (elapsed < controlTime)
        {
            elapsed += Time.deltaTime;
            InsectTimeÝmage.fillAmount = Mathf.Lerp(1f, 0f, elapsed / controlTime);
            yield return null;
        }

        // Karakter tekrar aktif -> böceðin yanýna doðsun
        character.transform.position = bocek.transform.position;
        character.gameObject.SetActive(true);
        character.transform.localScale = Vector3.zero;

        // --- Karakter büyüyerek geri geliyor ---
        yield return StartCoroutine(ScaleOverTime(character.transform, Vector3.zero, Vector3.one, 0.5f));

        // Böcek kontrolü kapat
        bocek.EnableControl(false);

        // Ýpi tekrar baðla
        bocek.lineRenderer.enabled = true;
        bocek.lineRenderer.SetPosition(0, character.transform.position);
        bocek.lineRenderer.SetPosition(1, bocek.transform.position);

        controllingBocek = false;
    }

    IEnumerator ScaleOverTime(Transform target, Vector3 from, Vector3 to, float duration)
    {
        float t = 0;
        while (t < duration)
        {

            t += Time.deltaTime / duration;
            target.localScale = Vector3.Lerp(from, to, t);
            yield return null;
        }
        target.localScale = to;
    }


   
}
