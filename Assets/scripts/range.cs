using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class range : MonoBehaviour
{
    private float nextFireTime = 0.0f;
    private float nextRotateTime = 0.0f;
    public float firePeriod = 0.2f;
    public float rotatePeriod = 0.5f;
    public GameObject BulletPrefab;
    GameObject SpawnedBullet;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextFireTime)
        {
            nextFireTime += firePeriod;
            // execute block of code here
            SpawnedBullet = Instantiate(BulletPrefab, transform.position, transform.rotation);
            SpawnedBullet.GetComponent<Bullet>().RBVelocity = transform.up * 100;
        }
        if (Time.time > nextRotateTime)
        {
            nextRotateTime += rotatePeriod;
            // execute block of code here
            Quaternion random_rotation = Random.rotation;
            transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(transform.rotation.x, transform.rotation.y, random_rotation.z, 1.0f), 1.0f);
        }
    }
}
