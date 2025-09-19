using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSwitch : MonoBehaviour
{
    public Charactercontroller character;   // Karakter script
    public InsectController bocek;          // B�cek script
    public float controlTime = 5f;          // B�ce�i kontrol s�resi
    [SerializeField] private GameObject DespawnParticle;

    private bool controllingBocek = false;


    //UI Insect Time Out Show
    [SerializeField] private Image InsectTime�mage;
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

        // --- Karakter k���l�p kayboluyor ---
        yield return StartCoroutine(ScaleOverTime(character.transform, Vector3.one, Vector3.zero, 0.5f));

        // Particle efekti
        if (DespawnParticle != null)
        {
            GameObject p = Instantiate(DespawnParticle, character.transform.position, Quaternion.identity);
            ParticleSystem ps = p.GetComponent<ParticleSystem>();
            if (ps != null)
                ps.Play();  // Prefab Play on Awake kapal� olsa bile �al��t�r�r
        }

        character.gameObject.SetActive(false);

        // B�cek aktif
        bocek.EnableControl(true);
        bocek.lineRenderer.enabled = false;

        // UI s�f�rla
        InsectTime�mage.fillAmount = 1f;

        float elapsed = 0f;
        while (elapsed < controlTime)
        {
            elapsed += Time.deltaTime;
            InsectTime�mage.fillAmount = Mathf.Lerp(1f, 0f, elapsed / controlTime);
            yield return null;
        }

        // Karakter tekrar aktif -> b�ce�in yan�na do�sun
        character.transform.position = bocek.transform.position;
        character.gameObject.SetActive(true);
        character.transform.localScale = Vector3.zero;

        // --- Karakter b�y�yerek geri geliyor ---
        yield return StartCoroutine(ScaleOverTime(character.transform, Vector3.zero, Vector3.one, 0.5f));

        // B�cek kontrol� kapat
        bocek.EnableControl(false);

        // �pi tekrar ba�la
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
