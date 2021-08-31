using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragNade : Grenade
{
    public float damage = 50f;

    private void Start()
    {
        grenadeType = GrenadeType.FRAG_NADE;
    }

    protected override void Explode()
    { 
        Collider[] collection = Physics.OverlapSphere(this.transform.position, explosionDistance, mask);

        foreach (var item in collection)
        {
            IDamageable enemy = item.GetComponent<IDamageable>();
            if (enemy != null) { enemy.GetDamage(damage);  }
            var rb = item.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(force, this.transform.position, explosionDistance, 1f, ForceMode.Impulse);
            }
            //var distance = (item.transform.position - transform.position);//for damage?

        }
        FXSpawner.Instance.fragPool.GetObject().SetPosition(this.transform);
        GrenadeSpawner.Instance.ReturnGrenade(this);
    }
}

