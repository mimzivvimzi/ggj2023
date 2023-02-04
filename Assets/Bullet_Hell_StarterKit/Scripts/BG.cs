using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //resets BG sprite position when it falls too low giving appearance of infinite scrolling background
        if (this.gameObject.transform.position.y < -10f)
        {
            this.gameObject.transform.position = new Vector3(0,9.9f,0);
        }
    }
}
