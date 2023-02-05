using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource audioSource;
    public AudioClip StartScreen;
    public AudioClip LevelMusic;
    public AudioClip Victory;
    public AudioClip GameOver;

    void Start()
    {
        audioSource.clip = StartScreen;
        audioSource.Play();
    }

    void Update()
    {
        if (Input.GetMouseButton(1)) {
            audioSource.Stop();
            audioSource.clip = Victory;
            audioSource.Play();
        }
    }
}
