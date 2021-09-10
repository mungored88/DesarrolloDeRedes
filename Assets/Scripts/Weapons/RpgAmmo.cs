using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RpgAmmo : Bullet
{
    public float force;
    public float explosionDistance;
    public float explotionTime = 3f;
    public LayerMask mask;

    public GameObject explotionEffect;
    //public override void OnCollisionEnter(Collision collision)
    //{
    //    Invoke("Explode", 0);
    //    GameObject.Destroy(this.gameObject);
    //}

    public void Explode()
    {
        //Sphere me permite generar un volumen de tipo esfera
        Collider[] collection = Physics.OverlapSphere(transform.position, explosionDistance, mask);

        foreach (var item in collection)
        {
            var rb = item.GetComponent<Rigidbody>();

            if (rb != null)
            {
                var distace = (item.transform.position - transform.position);
                rb.AddExplosionForce(force, transform.position, explosionDistance, 1f, ForceMode.Impulse);
            }

        }
        Destroy(Instantiate(explotionEffect, transform.position, transform.rotation),3f);
        Destroy(this.gameObject);
    }
}
