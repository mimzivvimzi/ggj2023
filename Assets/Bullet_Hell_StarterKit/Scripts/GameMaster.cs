using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    // This script handles many of the games settings and functions, be sure to set variables in inspector appropriately

    public static GameMaster Instance;
    [Tooltip("Array of Sprites corresponding to health value of player(element 2 is shown when player health = 2)")]
    public List<Sprite> HealthBarSprites;
    public GameObject PlayerObject;
    [Tooltip("Background Sprite 1")]
    public GameObject ScrollingBG1;
    [Tooltip("Background Sprite 2")]
    public GameObject ScrollingBG2;
    [Tooltip("Score value text")]
    public Text ScoreText;
    [Tooltip("Player spread upgrade level")]
    public Text SpreadText;
    [Tooltip("Player rate of fire upgrade level")]
    public Text RateText;
    [Tooltip("Background scrolling speed")]
    public float BGScrollSpeed;
    [Tooltip("Players score")]
    public int Score;
    [Tooltip("Image to Display player health bar sprite")]
    public Image HealthBarImage;
    [Tooltip("Active part of progress bar image object")]
    public RectTransform ProgressHP;
    [Tooltip("Active part of boss health bar image")]
    public RectTransform BossHP;
    [Tooltip("This is the boss prefab object in the game")]
    public GameObject BossObject;
    [Tooltip("Menu to show when you lose")]
    public GameObject GameOverObject;
    [Tooltip("menu to show when you lose")]
    public GameObject WinMenuObject;

    void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        //set to 60 frames per second
        Application.targetFrameRate = 60;
        PlayerObject = GameObject.Find("Bullet_Player");
        ScrollingBG1.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -BGScrollSpeed);
        ScrollingBG2.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -BGScrollSpeed);
        //hide cursor
        //Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    public void UpdateUI()
    {
        //gradually restore timescale to normal after a slow motion powerup;
        if(Time.timeScale < 1)
        {
            //can tweak the restoration rate below
            Time.timeScale = Time.timeScale + 0.001f;
        }
        //set ui text
        ScoreText.text = Score.ToString();
        RateText.text = "Rate: " + PlayerObject.GetComponent<Bullet_Player>().RateOfFireUpgrade.ToString();
        SpreadText.text = "Spread: " + PlayerObject.GetComponent<Bullet_Player>().ShotSpreadUpgrade.ToString();
        //update sprite on players hp bar depending on health remaining
        if (PlayerObject.GetComponent<Bullet_Player>().Health > -1)
        {
            HealthBarImage.sprite = HealthBarSprites[PlayerObject.GetComponent<Bullet_Player>().Health];
        }
        //update boss hp bar
        if (BossObject != null)
        {
            BossHP.sizeDelta = new Vector2((BossObject.GetComponent<EnemyAI>().Health / BossObject.GetComponent<EnemyAI>().MaxHealth) * 500, 24);
        }
        //update progress hp bar
        if (GetComponent<LevelSettings>().BossFightActive == false)
        {
            ProgressHP.sizeDelta = new Vector2(((GetComponent<LevelSettings>().TotalFramesUntilBoss- GetComponent<LevelSettings>().FramesUntilBoss) / GetComponent<LevelSettings>().TotalFramesUntilBoss) * 500, 24);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Restart()
    {
        SceneManager.LoadScene("Demo");
        Time.timeScale = 1;
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }

    private void FixedUpdate()
    {
        //update ui once per frame
        UpdateUI();
    }
}
