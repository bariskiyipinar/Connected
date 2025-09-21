using System.Collections;
using TMPro;
using UnityEngine;

public class MenuAnimatoinEffect : MonoBehaviour
{
    public Animator TransationEffect;
    public GameObject CircleObject;
    public TextMeshProUGUI MessageText;
    public string[] messages;
    public float typingSpeed = 0.05f;      // Normal hýz
    public float fastTypingSpeed = 0.01f;  // Hýzlý yazma

    private bool isTyping = false;

    private void Start()
    {
        MessageText.text = " ";
    }

    private void Update()
    {
        // Sol týklama kontrolü
        if (Input.GetMouseButtonDown(0) && isTyping)
        {
            // Hýzlandýrmak için typingSpeed’i geçici deðiþtiriyoruz
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

        // Typing bittikten sonra hýz normal deðerine dönsün
        typingSpeed = 0.05f;
        isTyping = false;
    }
}
