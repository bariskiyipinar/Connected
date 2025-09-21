using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSwitch : MonoBehaviour
{
    public Charactercontroller character;
    public InsectController bocek;
    public float controlTime = 5f;
    [SerializeField] private GameObject DespawnParticle;
    [SerializeField] private Image InsectTimeImage;

    private bool controllingBocek = false;
    private CameraController cam;

     private AudioSource CharacterSwitcherSound;

    void Start()
    {
        CharacterSwitcherSound = GetComponent<AudioSource>();
        if (Camera.main != null)
            cam = Camera.main.GetComponent<CameraController>();
    }

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

        if (CharacterSwitcherSound != null)
            CharacterSwitcherSound.Play();

        // --- Karakter küçülüp kayboluyor ---
        yield return StartCoroutine(ScaleOverTime(character.transform, Vector3.one, Vector3.zero, 0.5f));

        if (DespawnParticle != null)
        {
            GameObject p = Instantiate(DespawnParticle, character.transform.position, Quaternion.identity);
            ParticleSystem ps = p.GetComponent<ParticleSystem>();
            if (ps != null) ps.Play();
        }

        character.gameObject.SetActive(false);

        // Böcek aktif
        bocek.EnableControl(true);
        bocek.lineRenderer.enabled = false;

        // Kamera böceðe geçsin
        if (cam != null)
            cam.SetTarget(bocek.transform);

      
        float elapsed = 0f;
        while (elapsed < controlTime)
        {
            elapsed += Time.deltaTime;
            if (InsectTimeImage != null)
                InsectTimeImage.fillAmount = Mathf.Lerp(1f, 0f, elapsed / controlTime);
            yield return null;
        }

        // UI sýfýrla
        if (InsectTimeImage != null)
            InsectTimeImage.fillAmount = 1f;

        // Süre dolunca karakter tekrar aktif
        character.transform.position = bocek.transform.position;
        character.gameObject.SetActive(true);
        character.transform.localScale = Vector3.zero;

        // Kamera tekrar karaktere geçsin
        if (cam != null)
            cam.ResetToDefaultTarget();


        // --- Karakter büyüyerek geri geliyor ---
        yield return StartCoroutine(ScaleOverTime(character.transform, Vector3.zero, Vector3.one, 0.5f));

        // Böcek kontrolü kapat
        bocek.EnableControl(false);
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
