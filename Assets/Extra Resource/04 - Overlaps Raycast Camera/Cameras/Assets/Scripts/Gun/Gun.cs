using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour, IShoot
{
    public int ammo = 5;
    public float maxDistance;
    public LayerMask mask;

    public ParticleSystem shootParticle;

    public delegate void CanUse(bool outOfAmmo);
    public CanUse canUse = delegate { };

    private Camera _cam;

    private void Start()
    {
        _cam = Camera.main;
    }

    public void Shoot()
    {
        --ammo;
        shootParticle.Play();

        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, mask))
        {
            DestroyableObject dsObject = hit.transform.GetComponent<DestroyableObject>();
            dsObject?.DestroyObject();
        }

        canUse(ammo > 0);
    }
}
