using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingScreenManager : MonoBehaviour
{
    [Header("UI & Prefabs")]
    public Image explosionImage;
    public string textToShow = "CONNECTED";
    public GameObject letterPrefab;
    public Transform lettersParent;

    [Header("Credits UI")]
    public TMP_Text creditsHeaderText;  // "Hazýrlayanlar:" metni
    public TMP_Text creditsNamesText;   // Ýsimler
    public string creditsHeaderMessage = "Hazýrlayanlar:";
    public string creditsNamesMessage = "Barýþ KIYIPINAR\nEmrehan SEVÝMLÝ";

    public float creditsAppearDuration = 1f;
    public Vector3 creditsMoveOffset = new Vector3(0, 50f, 0); // Yukarýdan gelmesi için
    public float creditsLetterDelay = 0.05f;

    [Header("Animation Settings")]
    public float letterSpacing = 60f;
    public float dropDistance = 200f;
    public float appearDuration = 0.4f;
    public float delayBetweenLetters = 0.08f;
    public float jumpDuration = 0.5f;
    public float dropDelayBetweenLetters = 0.2f;

    private bool triggered = false;

    void Start()
    {
        if (explosionImage != null)
            explosionImage.gameObject.SetActive(false);

        if (creditsHeaderText != null)
            creditsHeaderText.gameObject.SetActive(false);

        if (creditsNamesText != null)
            creditsNamesText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered) return;

        if (collision.CompareTag("Player"))
        {
            triggered = true;

            Charactercontroller playerMovement = collision.GetComponent<Charactercontroller>();
            GameObject characterswitch = GameObject.Find("CharacterSwitcher");

            if (playerMovement != null)
            {
                playerMovement.moveSpeed = 0f;
                characterswitch.SetActive(false);
            }

            TriggerExplosion();
        }
    }

    void TriggerExplosion()
    {
        explosionImage.gameObject.SetActive(true);
        explosionImage.transform.localScale = Vector3.zero;
        explosionImage.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack)
            .OnComplete(() => SpawnLetters());
    }

    void SpawnLetters()
    {
        for (int i = 0; i < textToShow.Length; i++)
        {
            GameObject letterObj = Instantiate(letterPrefab, lettersParent);
            TMP_Text letterTMP = letterObj.GetComponent<TMP_Text>();
            letterTMP.text = textToShow[i].ToString();

            letterObj.transform.localPosition = new Vector3(i * letterSpacing, 0, 0);

            Color c = letterTMP.color;
            c.a = 0;
            letterTMP.color = c;

            int index = i;

            DOTween.To(() => letterTMP.color.a, x =>
            {
                Color col = letterTMP.color;
                col.a = x;
                letterTMP.color = col;
            }, 1f, appearDuration)
            .SetDelay(index * delayBetweenLetters)
            .OnComplete(() =>
            {
                letterObj.transform.DOPunchPosition(
                    new Vector3(Random.Range(-15f, 15f), Random.Range(20f, 40f), 0),
                    jumpDuration,
                    1,
                    1f
                );

                int lastThreeStartIndex = textToShow.Length - 3;
                if (index >= lastThreeStartIndex)
                {
                    float dropDelay = (index - lastThreeStartIndex) * dropDelayBetweenLetters;
                    letterObj.transform.DOLocalMoveY(
                        letterObj.transform.localPosition.y - dropDistance,
                        jumpDuration
                    ).SetEase(Ease.InBounce)
                     .SetDelay(dropDelay)
                     .OnComplete(() => { if (index == textToShow.Length - 1) ShowCredits(); });
                }
            });
        }
    }

    void ShowCredits()
    {
        // Baþlýk sabit, sadece yukarýdan aþaðý kayma ve fade
        if (creditsHeaderText != null)
        {
            creditsHeaderText.gameObject.SetActive(true);
            creditsHeaderText.text = creditsHeaderMessage;
            Vector3 originalPos = creditsHeaderText.transform.localPosition;
            creditsHeaderText.transform.localPosition = originalPos + creditsMoveOffset;

            Color c = creditsHeaderText.color;
            c.a = 0;
            creditsHeaderText.color = c;

            DOTween.To(() => creditsHeaderText.color.a, x =>
            {
                Color col = creditsHeaderText.color;
                col.a = x;
                creditsHeaderText.color = col;
            }, 1f, creditsAppearDuration);

            creditsHeaderText.transform.DOLocalMove(originalPos, creditsAppearDuration).SetEase(Ease.OutBack);
        }

        // Ýsimler harf harf çýkacak
        if (creditsNamesText != null)
        {
            creditsNamesText.gameObject.SetActive(true);
            creditsNamesText.text = "";

            Vector3 namesOriginalPos = creditsNamesText.transform.localPosition;
            creditsNamesText.transform.localPosition = namesOriginalPos + creditsMoveOffset;

            Color c = creditsNamesText.color;
            c.a = 1f;
            creditsNamesText.color = c;

            StartCoroutine(AnimateNamesText(namesOriginalPos));
        }
    }

    System.Collections.IEnumerator AnimateNamesText(Vector3 finalPos)
    {
        foreach (char letter in creditsNamesMessage)
        {
            creditsNamesText.text += letter;
            creditsNamesText.transform.DOPunchPosition(new Vector3(0, 5f, 0), 0.2f, 1, 1f);
            yield return new WaitForSeconds(creditsLetterDelay);
        }

        creditsNamesText.transform.DOLocalMove(finalPos, creditsAppearDuration).SetEase(Ease.OutBack);

        yield return new WaitForSeconds(1f); 
        GoToMainMenu();
    }


    void GoToMainMenu()
    {
        if (SoundManager.instance != null)
            SoundManager.instance.RestartMusic();

        SceneManager.LoadScene("MainMenu"); 
    }

}
