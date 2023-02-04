using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Basic menu, build on it or implement your own

    public GameObject ScrollingBG1;
    public GameObject ScrollingBG2;
    public float BGScrollSpeed = -0.2f;
    // Start is called before the first frame update
    void Start()
    {
        ScrollingBG1.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -BGScrollSpeed);
        ScrollingBG2.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -BGScrollSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButton()
    {
        SceneManager.LoadScene("Demo");
    }
   
}
