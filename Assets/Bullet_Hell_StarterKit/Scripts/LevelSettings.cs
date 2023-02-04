using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSettings : MonoBehaviour
{
    // This script handles the transition into the boss fight, you can build on it to implement other level features as well

    [Tooltip("Frames until Boss Fight")]
    public float FramesUntilBoss;
    [Tooltip("This object is enabled when boss fight starts, this should be the parent of every object you want to enable for the boss fight")]
    public GameObject BossObject;
    [Tooltip("Play this track when boss fight begins")]
    public AudioClip BossMusic;
    [Tooltip("Disables this object when boss fight begins, put the parent of all normal spawners here to stop normal enemy spawning from continuing during the boss fight")]
    public GameObject Spawners;
    [Tooltip("Your main audiosource")]
    public AudioSource MainAudioSource;
    [Tooltip("Active portion of boss hp bar")]
    public GameObject BossHPBar;
    [Tooltip("active portion of progress bar")]
    public GameObject ProgressBar;
    [Tooltip("Ignore")]
    public bool BossFightActive;
    [Tooltip("Ignore")]
    public float TotalFramesUntilBoss;

    // Start is called before the first frame update
    void Start()
    {
        // this is for the progress bar on the top of screen
        TotalFramesUntilBoss = FramesUntilBoss;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        FramesUntilBoss--;
        // this is what happens when the boss arrives
        if(FramesUntilBoss==0)
        {
            BossFightActive = true;
            BossObject.SetActive(true);
            //disables normal spawners for boss fight
            Spawners.SetActive(false);
            BossHPBar.SetActive(true);
            ProgressBar.SetActive(false);
            if(BossMusic!=null)
            {
                MainAudioSource.clip = BossMusic;
                MainAudioSource.Play();
                MainAudioSource.loop = true;
            }
        }
    }
}
