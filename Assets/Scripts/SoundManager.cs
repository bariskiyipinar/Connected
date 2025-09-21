using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    private AudioSource backgroundSound;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject); 
            backgroundSound = GetComponent<AudioSource>();
            backgroundSound.Play();
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    public void RestartMusic()
    {
        if (backgroundSound != null)
        {
            backgroundSound.Stop();
            backgroundSound.time = 0f;  
            backgroundSound.Play();
        }
    }
}