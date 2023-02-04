using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //This script handles all bullet/projectile logic
    //Attach this script to any bullet prefab you want to use

    [Tooltip("How many frames does the bullet last")]
    public int Duration;
    [Tooltip("Check this for Bullet_Player bullet, automatically moves the bullet with next field velocity value")]
    public bool Bullet_PlayerBullet;
    [Tooltip("Velocity of Bullet_Player Bullet")]
    public Vector2 RBVelocity = new Vector2(0f,5f);
    [Tooltip("Is this bullet a beam")]
    public bool IsBeam;
    GameObject GM;
    public Rigidbody2D RB;
    private void Awake()
    {
        GM = GameObject.Find("GameMaster");
    }
    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //Bullet_Playersbullet always moves straight up
        if(Bullet_PlayerBullet==true)
        {
            RB.velocity = RBVelocity;
        }
        Duration--;
        //remove bullet if its duration expires
        if(Duration==0)
        {
            if (IsBeam == false)
            {
                Destroy(this.gameObject);
            }
            //boss uses this to animate while he is shooting beam
            if (IsBeam == true)
            {
                this.gameObject.SetActive(false);
                if (this.gameObject.GetComponentInParent<EnemyAI>().AnimateOnShoot == true)
                {
                    this.gameObject.GetComponentInParent<SpriteRenderer>().sprite = this.gameObject.GetComponentInParent<EnemyAI>().NormalSprite;
                    this.gameObject.GetComponentInParent<EnemyAI>().animator.enabled = false;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Bullet_Playerbullet
        if(this.gameObject.layer==10)
        {
            //check if bullet hits enemy
            if(collision.gameObject.layer==9)
            {
                //subtract enemies health
                collision.gameObject.GetComponent<EnemyAI>().Health--;
                //check if it was the boss
                if(collision.gameObject.GetComponent<EnemyAI>().IsBoss==true)
                {
                    //check if boss health is low enough to enter stage 2 of boss fight
                    if(collision.gameObject.GetComponent<EnemyAI>().Health < collision.gameObject.GetComponent<EnemyAI>().BossStage2HealthThreshold && collision.gameObject.GetComponent<EnemyAI>().InStage2 == false)
                    {
                        //activate stage2 boss fight object and disable stage1
                        collision.gameObject.GetComponent<EnemyAI>().InStage2 = true;
                        collision.gameObject.GetComponent<EnemyAI>().BossStage2ObjectActivate.SetActive(true);
                        collision.gameObject.GetComponent<EnemyAI>().BossStage2ObjectDeactivate.SetActive(false);
                    }

                }
                //check if enemy needs to die
                if(collision.gameObject.GetComponent<EnemyAI>().Health<1)
                {
                    //check if it was boss
                    if (collision.gameObject.GetComponent<EnemyAI>().IsBoss == true)
                    {
                        //end the level here when boss dies
                        Destroy(collision.gameObject);
                        Time.timeScale = 0;
                        GM.GetComponent<GameMaster>().WinMenuObject.SetActive(true);
                        Cursor.visible = true;
                    }
                    //remove enemy
                    Destroy(collision.gameObject);
                }
                // add +1 score for each bullet that hits an enemy
                GM.GetComponent<GameMaster>().Score++;
                Destroy(this.gameObject);
                
                
                
            }
        }
        //enemybullet
        if (this.gameObject.layer == 11)
        {
            //check if it hits Bullet_Player and Bullet_Player is not invincible , Bullet_Playeris layer 8
            if (collision.gameObject.layer == 8 && collision.gameObject.GetComponent<Bullet_Player>().InvincibleFrames < 1)
            {
                collision.gameObject.GetComponent<Bullet_Player>().animator.Play("Bullet_Player_Damaged",0);
                //subtract Bullet_Players health
                collision.gameObject.GetComponent<Bullet_Player>().Health--;
                //this stuff happens when Bullet_Player dies
                if (collision.gameObject.GetComponent<Bullet_Player>().Health == 0)
                {
                    collision.gameObject.GetComponent<Bullet_Player>().animator.Play("Bullet_Player Death", 0);
                    Time.timeScale = 0;
                    GM.GetComponent<GameMaster>().UpdateUI();
                    GM.GetComponent<GameMaster>().GameOverObject.SetActive(true);
                    Cursor.visible = true;
                }
                //make Bullet_Player invincible for short time
                collision.gameObject.GetComponent<Bullet_Player>().InvincibleFrames = collision.gameObject.GetComponent<Bullet_Player>().InvincibleFramesOnHit;
                //Bullet_Player becomes layer 31, layer 31 ignores collisions with enemy projectiles
                collision.gameObject.layer = 31;
                //remove bullet from game
                if (IsBeam == false) //dont destroy a beam
                {
                    Destroy(this.gameObject);
                }

                ////disable beam on hit
                //if (IsBeam == true)
                //{
                //    //this.gameObject.SetActive(false);
                //}
            }
        }
        //destroy bullet on boundary
        if(collision.gameObject.layer == 30)
        {
            Destroy(this.gameObject);
        }

    }

}
