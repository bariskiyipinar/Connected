using System.Collections;
using TMPro;
using UnityEngine;

public class MenuAnimatoinEffect : MonoBehaviour
{
    public Animator TransationEffect;
    public GameObject CircleObject;
    public TextMeshProUGUI MessageText;
    public string[] messages;
    public float typingSpeed = 0.05f;      // Normal h�z
    public float fastTypingSpeed = 0.01f;  // H�zl� yazma

    private bool isTyping = false;

    private void Start()
    {
        MessageText.text = " ";
    }

    private void Update()
    {
        // Sol t�klama kontrol�
        if (Input.GetMouseButtonDown(0) && isTyping)
        {
            // H�zland�rmak i�in typingSpeed�i ge�ici de�i�tiriyoruz
            typingSpeed = fastTypingSpeed;
        }
    }

    public void StartTypingText()
    {
        if (!isTyping)
            StartCoroutine(PlayText());
    }

    public IEnumerator PlayText()
    {
        isTyping = true;

        foreach (string message in messages)
        {
            MessageText.text = "";

            foreach (char letter in message.ToCharArray())
            {
                MessageText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }

            yield return new WaitForSeconds(1f);
        }

        // Typing bittikten sonra h�z normal de�erine d�ns�n
        typingSpeed = 0.05f;
        isTyping = false;
    }
}
