using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    // This script is attached to the powerup prefab
    public enum PowerupType
    {
        Health = 1,
        FireRate = 2,
        ShotSpread = 3,
        TimeSlow = 4,
    }
    [Tooltip("what type of powerup is this")]
    public PowerupType PowerUp;
    [Tooltip("rigidbody velocity of powerup, negative y value makes it fall down")]
    public Vector2 Velocity;
    Rigidbody2D RB;
    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        RB.velocity = Velocity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //only allow collision with Bullet_Player or screen boundary
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 31)
        {
            // Gives +1 HP
            if ((int)PowerUp == 1)
            {
                    collision.gameObject.GetComponent<Bullet_Player>().Health++;
                    //dont let HP go higher than Max HP
                    if (collision.gameObject.GetComponent<Bullet_Player>().Health > collision.gameObject.GetComponent<Bullet_Player>().MaxHealth)
                    {
                        collision.gameObject.GetComponent<Bullet_Player>().Health = collision.gameObject.GetComponent<Bullet_Player>().MaxHealth;
                    }
                    Destroy(this.gameObject);
            }

            //Increase Rate of Fire
            if ((int)PowerUp == 2)
            {
                collision.gameObject.GetComponent<Bullet_Player>().RateOfFireUpgrade++;
                //reduce Bullet_Players cooldown time
                collision.gameObject.GetComponent<Bullet_Player>().ShootCooldown = collision.gameObject.GetComponent<Bullet_Player>().BaseCooldown - collision.gameObject.GetComponent<Bullet_Player>().RateOfFireUpgrade * collision.gameObject.GetComponent<Bullet_Player>().ROFIncrement;
                //dont allow shoot cooldown to be 0
                if(collision.gameObject.GetComponent<Bullet_Player>().ShootCooldown<1)
                {
                    collision.gameObject.GetComponent<Bullet_Player>().ShootCooldown = 1;
                }
                Destroy(this.gameObject);
            }

            //Increases the spread of Bullet_Players shooting
            if ((int)PowerUp == 3)
            {
                collision.gameObject.GetComponent<Bullet_Player>().ShotSpreadUpgrade++;
                Destroy(this.gameObject);
            }

            //slows down time
            if ((int)PowerUp == 4)
            {
                Time.timeScale = 0.25f;
                Destroy(this.gameObject);
            }
        }

        //remove powerup from game when it falls below edge of screen
        if (collision.gameObject.layer == 29)
        {
            if (collision.gameObject.tag == "KillEnemy")
            {
                Destroy(this.gameObject);
            }
        }

    }

}
