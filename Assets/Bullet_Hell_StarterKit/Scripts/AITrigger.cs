using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITrigger : MonoBehaviour
{
    // This trigger is for the boss, it changes the bosses speed when it touches the area, creating a strafing effect with variable speed
    // One object with this script on the left of screen and one on the right, boss will travel between both

    // randomly picks velocity between minimum and maximum
    Vector2 Velocity;
    [Tooltip("Minimum velocity value to apply")]
    public float Min;
    [Tooltip("Maximum velocity value to apply")]
    public float Max;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //only affects boss
        if(collision.gameObject.tag == "Boss")
        {
            //change velocity & direction when touched
            Velocity = new Vector2(Random.Range(Min,Max), 0);
            collision.gameObject.GetComponent<EnemyAI>().RB.velocity = Velocity;
        }
    }
}
