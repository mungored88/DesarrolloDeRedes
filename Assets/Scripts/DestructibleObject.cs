using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour, IDamageable
{
    public float life;

    void Update()
    {
        if(life <= 0)
        {
            GameObject.Destroy(this.gameObject);
        }
    }


    public void GetDamage(float dmg)
    {
        life -= dmg;
    }
}
