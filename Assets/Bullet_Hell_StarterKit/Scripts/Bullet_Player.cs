using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Player : MonoBehaviour
{
    // This script is attached to the player prefab object and handles all relevent function of the player

    [Tooltip("Current health of player")]
    public int Health;
    [Tooltip("Max Health of Player")]
    public int MaxHealth;
    [Tooltip("When the player is damaged, make him invincible for this many frames")]
    public int InvincibleFramesOnHit;
    [Tooltip("How many frames to wait until player shoots again")]
    public int ShootCooldown;
    [Tooltip("How many times has rate of fire been upgraded")]
    public int RateOfFireUpgrade;
    [Tooltip("How many frames does each rate of fire upgrade decrease the shooting cooldown by")]
    public int ROFIncrement = 2;
    [Tooltip("How many times has the players spread shot been upgraded, each level adds 2 bullets")]
    public int ShotSpreadUpgrade;
    [Tooltip("how far apart should the spread bullets be instantiated")]
    public float BulletXOffset;
    [Tooltip("bullet to instantiate on shoot")]
    public GameObject BulletPrefab;
    public int InvincibleFrames;
    //players rigidbody
    public Rigidbody2D RB;
    //players animator
    public Animator animator;
    //public float MoveSpeed;
    //public Vector2 inputAxis;
    //public Vector2 MoveV;

    float RBXVelocity;
    float RBYVelocity;
    GameObject SpawnedBullet;
    GameObject GameMasterObject;
    GameObject ObjectHolder;
    public int BaseCooldown;
    int CurrentShootCooldown=1;
    int scoretick;

    // Start is called before the first frame update
    void Start()
    {
        GameMasterObject = GameObject.Find("GameMaster");
        ObjectHolder = GameObject.Find("Objects");
        RB = GetComponent<Rigidbody2D>();
        BaseCooldown = ShootCooldown;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        
        scoretick++;
        if(scoretick==60)
        {
            //adds 1 score every second(60frames)
            GameMasterObject.GetComponent<GameMaster>().Score++;
            scoretick = 0;
        }
        
        //input move with arrow keys/joystick
        //inputAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //MoveV = inputAxis.normalized * MoveSpeed;
        //RB.velocity = MoveV;

        //mouse move, get mouse coordinates on screen
        Vector3 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //restrict mouse coordinates inside of screen
        MousePos.x = Mathf.Clamp(MousePos.x, -8.7f, 8.7f);
        MousePos.y = Mathf.Clamp(MousePos.y, -4.3f, 5f);
        MousePos.z = 0;
        //this.gameObject.transform.position = MousePos;
        //smoothes player movment
        transform.position = Vector3.Lerp(transform.position, MousePos, Time.deltaTime*4);

        //check players cooldown to shoot again
        CurrentShootCooldown--;
        if(CurrentShootCooldown == 0)
        {
            //instantiate bullet
            SpawnedBullet = Instantiate(BulletPrefab,this.gameObject.transform);
            SpawnedBullet.transform.parent = ObjectHolder.transform;
            CurrentShootCooldown = ShootCooldown;
            
            //spread shot calculations
            if(ShotSpreadUpgrade > 0)
            {
                for(int i = 0; i < ShotSpreadUpgrade; i++)
                {
                    SpawnedBullet = Instantiate(BulletPrefab, this.gameObject.transform);
                    SpawnedBullet.GetComponent<Bullet>().RBVelocity = new Vector2(BulletXOffset*(i+1), 5-((BulletXOffset/2)*(i+1)));
                    SpawnedBullet.transform.parent = ObjectHolder.transform;
                    SpawnedBullet = Instantiate(BulletPrefab, this.gameObject.transform);
                    SpawnedBullet.GetComponent<Bullet>().RBVelocity = new Vector2(-BulletXOffset * (i + 1), 5- ((BulletXOffset/2) * (i + 1)));
                    SpawnedBullet.transform.parent = ObjectHolder.transform;
                }
            }
        }
        //remove player invincibility when it expires
        if(InvincibleFrames>0)
        {
            InvincibleFrames--;
            if(InvincibleFrames == 0)
            {
                this.gameObject.layer = 8;
            }
        }
    }
    public void UpdateUI()
    {

    }

}
