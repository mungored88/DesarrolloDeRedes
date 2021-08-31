using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform bulletTransform;
    public float speed = 10f;

    public float damage = 10f;

    public float lifeTime = 3f;

    private void Awake()
    {
        bulletTransform = this.GetComponent<Transform>();
    }

void Update()
    {
        bulletTransform.position += bulletTransform.forward * speed * Time.deltaTime;

        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0) { BulletSpawner.Instance.ReturnBullet(this); lifeTime = 3f; }
     }

    public virtual void OnCollisionEnter(Collision collision)
    {
      if(collision.gameObject.GetComponent<IDamageable>() != null)
        {
            collision.gameObject.GetComponent<IDamageable>().GetDamage(damage);
        }
      BulletSpawner.Instance.ReturnBullet(this);
    }

    public Bullet SetPosition(Transform t)
    {
        bulletTransform.position = t.position;
        bulletTransform.forward = t.forward;
        return this;
    }

    public Bullet SetDamage(float dmg)
    {
        damage = dmg;
        return this;
    }

    //Funcion que va a tener guardada el object pool cuando se la paso desde el Factory al crearlo (turnOnCallback)
    public static void TurnOn(Bullet b)
    {
        //b.Reset();  //Antes de prenderlo ejecuto, si necesito, una funcion para devolver valores necesarios para simular que es la primera vez que la uso
        b.gameObject.SetActive(true);  //La activo
    }

    //Funcion que va a tener guardada el object pool cuando se la paso desde el Factory al crearlo  (turnOffCallback)
    public static void TurnOff(Bullet b)
    {
        b.gameObject.SetActive(false); //La deshabilito
        
    }
    
}