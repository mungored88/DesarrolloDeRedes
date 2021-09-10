using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity , IDamageable
{
    public float life = 100;

    public float damage = 25f;


    //TODO Deberian ir a script de View para corresponder al MVC
    public AudioSource attack;
    public AudioSource getHit;

    public void Die()
    {
        Destroy(this.gameObject);
    }

    public void GetDamage(float dmg)
    {
        getHit.Play();

        life -= dmg;
        if (life <= 0) Die();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Player>() != null)
        {
            attack.Play();
            other.GetComponent<IDamageable>().GetDamage(damage);
        }
    }
}
