using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootController : MonoBehaviour
{
    public Bullet bullet;
    public Transform bulletOrigin;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Bullet b = Instantiate(bullet);
            b.transform.position = bulletOrigin.position;
            b.transform.forward = bulletOrigin.transform.forward;
        }
    }
}
