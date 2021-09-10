using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponExpendMachine : MonoBehaviour
{
    public Bullet bullet;

    private void OnTriggerStay(Collider other)
    {
        if (!other.GetComponent<Entity>()) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            //Debug.Log("changeWeapon");
            //other.GetComponent<Player>().ChangeWeapon(bullet);
          //  this.GetComponent<AudioSource>().Play();
        }
    }
}
