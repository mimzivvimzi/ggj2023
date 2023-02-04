using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum ShootMode
    {
        BasicAim = 1,
        SpreadAim = 2,
        Rotating = 3,
        Beam = 4,
    }
    [Tooltip("Type of enemy shooting, beam is experimental and is for the demo boss")]
    public ShootMode EnemyShootMode;

    [Tooltip("Enemy health")]
    public float Health;
    [Tooltip("Enemy max health")]
    public float MaxHealth;
    [Tooltip("Enemy shooting cooldown/ frames in between shots")]
    public int ShootCooldown;
    [Tooltip("Bullet object the enemy shoots")]
    public GameObject BulletPrefab;
    [Tooltip("Speed of enemy bullets")]
    public float BulletSpeed;
    [Tooltip("The incrementing angle applied to the enemies shooting pattern - this effects shootmode Spread & Rotating")]
    public float BulletSpreadAngle;
    [Tooltip("This value determines how many extra projectiles are fired in each direction when the enemy is using spread shot")]
    public int BulletSpreadQty;
    [Tooltip("This value is how many shots can be fired in a quick succession or 'burst', before the shooting cooldown will apply. For example the enemy can fire 10 shots in succession and then have to wait the cooldown period")]
    public int BurstQty = 1;
    [Tooltip("This is the delay in frames, between each burst shot, after all burst shots, the enemy waits the cooldown to fire again")]
    public int BurstIntervalFrames;
    [Tooltip("velocity of enemy rigidbody")]
    public Vector2 Velocity;
    [Tooltip("Only boss should have this checked")]
    public bool IsBoss;
    [Tooltip("this is true when boss is in stage 2")]
    public bool InStage2;
    [Tooltip("when boss falls below this much health, stage 2 is activated - use this to make a boss fight become harder when you damage the boss enough")]
    public int BossStage2HealthThreshold;
    [Tooltip("when boss fight stage 2 begins, enable this object(should be innactive by default) - this allows you to activate extra enemies/enemy spawners when the boss stage 2 begins")]
    public GameObject BossStage2ObjectActivate;
    [Tooltip("when boss fight stage 2 begins, disable this object - put stage 1 boss spawners etc in here")]
    public GameObject BossStage2ObjectDeactivate;
    [Tooltip("ignore this")]
    GameObject SpawnedBullet;

    public int CurrentBurstQty;
    public int CurrentBurstTick;
    public int CurrentShootCooldown=1;
    public float AngleInc;
    public Rigidbody2D RB;
    public GameObject PlayerObject;
    GameObject ObjectHolder;
    public string Description;
    public bool AnimateOnShoot;
    public Animator animator;
    public Sprite NormalSprite;
    public Sprite ShootingSprite;
    public GameObject GM;

    public Sprite Stage_2_Sprite;
    public Sprite Stage_3_Sprite;
    public float Stage_2_Health;
    public float Stage_3_Health;

    SpriteRenderer playerSprite;
    Color defaultColor = new Color(1, 1, 1, 1);

    // Start is called before the first frame update
    void Start()
    {
        GM = GameObject.Find("GameMaster");
        PlayerObject = GameObject.Find("Bullet_Player");
        ObjectHolder = GameObject.Find("Objects");
        RB.velocity = Velocity;
        playerSprite = GetComponent<SpriteRenderer>();
        //Debug.Log((int)EnemyShootMode);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //check if enemy is ready to shoot
        CurrentShootCooldown--;
        if (CurrentShootCooldown == 0)
        {
            //call the corresponding function depending on the style of the enemies shooting
            CurrentShootCooldown = ShootCooldown;
            if ((int)EnemyShootMode == 1)
            {
                ShootMode1();
            }
            if ((int)EnemyShootMode == 2)
            {
                ShootMode2();
            }
            if ((int)EnemyShootMode == 3)
            {
                ShootMode3();
            }
            if ((int)EnemyShootMode == 4)
            {
                ShootMode4();
            }

            
        }
    }

    public void ShootMode1()
    {
        //Basic Aim, 1 projectile aiming at the player

        SpawnedBullet = Instantiate(BulletPrefab, this.gameObject.transform);
        Rigidbody2D BRB;
        BRB = SpawnedBullet.GetComponent<Rigidbody2D>();
        //calculate angle to shoot
        Vector3 dir = this.gameObject.transform.position - PlayerObject.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle = angle + 90;
        SpawnedBullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        BRB.AddForce(SpawnedBullet.transform.up * BulletSpeed * 5, ForceMode2D.Force);
        SpawnedBullet.transform.SetParent(ObjectHolder.transform);
        if(BurstQty>1)
        {
            CurrentShootCooldown = BurstIntervalFrames;
            CurrentBurstQty++;
            if (CurrentBurstQty == BurstQty)
            {
                CurrentShootCooldown = ShootCooldown;
                CurrentBurstQty = 0;
            }
        }

    }

    public void ShootMode2()
    {
        //Basic Aim + Spread, 1 projectile aiming at the player with additional projectiles at increasing incremental angles;

        SpawnedBullet = Instantiate(BulletPrefab, this.gameObject.transform);
        Rigidbody2D BRB;
        BRB = SpawnedBullet.GetComponent<Rigidbody2D>();
        //calculate angle to shoot
        Vector3 dir = this.gameObject.transform.position - PlayerObject.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle = angle + 90;
        SpawnedBullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        BRB.AddForce(SpawnedBullet.transform.up * BulletSpeed * 5, ForceMode2D.Force);
        SpawnedBullet.transform.SetParent(ObjectHolder.transform);


        float AngleIncrement = BulletSpreadAngle;
        
        //for each spreadqty, create 2 additional projectiles
        for (int i = 0; i < BulletSpreadQty; i++)
        {
            float RandomAccuracyAngle = 0;
            SpawnedBullet = Instantiate(BulletPrefab, this.gameObject.transform);
            SpawnedBullet.transform.SetParent(ObjectHolder.transform);
            SpawnedBullet.transform.rotation = this.gameObject.transform.rotation;
            Vector3 dir1 = this.gameObject.transform.position - PlayerObject.transform.position;
            float angle1 = Mathf.Atan2(dir1.y, dir1.x) * Mathf.Rad2Deg;
            angle1 = angle1 + 90;
            SpawnedBullet.transform.rotation = Quaternion.AngleAxis(angle1, Vector3.forward);
            SpawnedBullet.transform.Rotate(
                SpawnedBullet.transform.eulerAngles.x,
                SpawnedBullet.transform.eulerAngles.y,
                AngleIncrement + Random.Range(RandomAccuracyAngle * -1, RandomAccuracyAngle),
                Space.Self
                );
            SpawnedBullet.GetComponent<Rigidbody2D>().AddForce(SpawnedBullet.transform.up * BulletSpeed * 5, ForceMode2D.Force);


            SpawnedBullet = Instantiate(BulletPrefab, this.gameObject.transform);
            SpawnedBullet.transform.SetParent(ObjectHolder.transform);
            SpawnedBullet.transform.rotation = this.gameObject.transform.rotation;
            dir1 = this.gameObject.transform.position - PlayerObject.transform.position;
            angle1 = Mathf.Atan2(dir1.y, dir1.x) * Mathf.Rad2Deg;
            angle1 = angle1 + 90;
            SpawnedBullet.transform.rotation = Quaternion.AngleAxis(angle1, Vector3.forward);
            SpawnedBullet.transform.Rotate(
                SpawnedBullet.transform.eulerAngles.x,
                SpawnedBullet.transform.eulerAngles.y,
                -AngleIncrement + Random.Range(RandomAccuracyAngle * -1, RandomAccuracyAngle),
                Space.Self
                );
            SpawnedBullet.GetComponent<Rigidbody2D>().AddForce(SpawnedBullet.transform.up * BulletSpeed * 5, ForceMode2D.Force);

            AngleIncrement = AngleIncrement + BulletSpreadAngle;
        }

    }

    public void ShootMode3()
    {
        //incrementing angle spiral effect

        SpawnedBullet = Instantiate(BulletPrefab, this.gameObject.transform);
        Rigidbody2D BRB;
        BRB = SpawnedBullet.GetComponent<Rigidbody2D>();
        SpawnedBullet.transform.Rotate(
                SpawnedBullet.transform.eulerAngles.x,
                SpawnedBullet.transform.eulerAngles.y,
                AngleInc,
                Space.Self
                );
        AngleInc = AngleInc + BulletSpreadAngle;
        BRB.AddForce(SpawnedBullet.transform.up * BulletSpeed * 5, ForceMode2D.Force);
        SpawnedBullet.transform.SetParent(ObjectHolder.transform);
    }

    //this one was created for the demo boss
    public void ShootMode4()
    {
        if(BulletPrefab.activeSelf==false)
        {
            BulletPrefab.SetActive(true);
            BulletPrefab.GetComponent<Bullet>().Duration = 120;
            if(AnimateOnShoot==true)
            {
                animator.enabled = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //remove the enemy when it travels off the bounds of the screen
        if(collision.gameObject.layer == 29)
        {
            if(collision.gameObject.tag == "KillEnemy")
            {
                Destroy(this.gameObject);
            }
        }
        //damage the player when colliding with it
        if (collision.gameObject.layer == 8 && collision.gameObject.GetComponent<Bullet_Player>().InvincibleFrames < 1)
        {
            collision.gameObject.GetComponent<Bullet_Player>().animator.Play("Player_Damaged", 0);
            //subtract players health
            collision.gameObject.GetComponent<Bullet_Player>().Health--;
            if (collision.gameObject.GetComponent<Bullet_Player>().Health == 0)
            {
                collision.gameObject.GetComponent<Bullet_Player>().animator.Play("Player Death", 0);
                Time.timeScale = 0;
                GM.GetComponent<GameMaster>().UpdateUI();
                GM.GetComponent<GameMaster>().GameOverObject.SetActive(true);
                Cursor.visible = true;
            }
            //make player invincible for short time
            collision.gameObject.GetComponent<Bullet_Player>().InvincibleFrames = collision.gameObject.GetComponent<Bullet_Player>().InvincibleFramesOnHit;
            //player becomes layer 31, layer 31 ignores collisions with enemy projectiles
            collision.gameObject.layer = 31;
        }

    }

    public void Blink()
    {
        StartCoroutine(Change_Color());
    }

    public IEnumerator Change_Color()
    {
        playerSprite.color = new Color(1, 0, 0, 1);

        yield return new WaitForSeconds(0.1f);

        playerSprite.color = defaultColor;
    }

    public void Check_Stage()
    {
        if (Health < Stage_3_Health)
        {
            if (Stage_3_Sprite)
            {
                playerSprite.sprite = Stage_3_Sprite;
            }
        }
        else if (Health < Stage_2_Health)
        {
            if (Stage_2_Sprite)
            {
                playerSprite.sprite = Stage_2_Sprite;
            }
        }
    }
}
