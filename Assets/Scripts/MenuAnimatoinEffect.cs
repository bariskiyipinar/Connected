using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Tilemaps;
using UnityEngine;

public class MenuAnimatoinEffect : MonoBehaviour
{
     public Animator TransationEffect;
     public GameObject CircleObject;
    public TextMeshProUGUI MessageText;
    public string[] messages;
    public float typingSpeed = 0.05f;

    private bool isTyping = false;

    private void Start()
    {
        MessageText.text = " ";
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

        isTyping = false;
    }

}
