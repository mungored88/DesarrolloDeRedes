using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour , IInteractable, ICollectable<Weapon>

{
    [Header("Sounds")]
    public AudioSource shootSound;
    public AudioSource reloadSound;
    public AudioSource noAmmoSound;
    public AudioSource grabSound;
    WeaponSoundMananger _weaponSoundMananger;

    
    protected Ammo ammo;
    protected Bullet bullet;
    protected float realoadTime = 2;
    private bool reloading = false;
    protected Transform bulletOrigin;
    protected Transform[] bulletOriginBucket;

    protected IFiringMode myCurrentFiringMode;
    protected IFiringMode FMSingleShoot;
    protected IFiringMode FMBurstShoot;
    protected IFiringMode FMAutomaticShoot;
    protected IFiringMode FMBuckShoot;

    protected IFiringMode[] availablesFiringModes;

    private int _currentMode = 0;

    private bool _isPrimary;
    private string _name;

    
    Coroutine shooting;
    Action shoot;

    //List<IObserver> _allObserver = new List<IObserver>(); 

    public event Action<Ammo> onUpdateAmmo;
    public event Action OnGrab;
    public event Action<Weapon> Collect;

    public Action changeFiringMode;

    public Ammo GetAmmo { get { return ammo; } }
    public int AddClips { set { ammo.CLIPS += value; } }
    public Transform BulletOrigin{ get { return bulletOrigin; } set { bulletOrigin = value; }    }
    public Transform[] BulletOriginBucket{ get { return bulletOriginBucket; } set { bulletOriginBucket = value; }    }

    public bool IsPrimary { get { return _isPrimary; } set { _isPrimary = value; } }
    public string Name { get { return _name; } set { _name = value; } }

    protected virtual void Awake()
    {
        FMSingleShoot = new SingleShoot();
        FMBurstShoot =  new BurstShoot();
        FMAutomaticShoot = new AutomaticShoot();
        FMBuckShoot = new BuckShoot();
        
        shoot += ShootOne;
        changeFiringMode += ChangeFiringMode;

        _weaponSoundMananger = new WeaponSoundMananger(this);
    }


    public void GrabThis()
    {
        Collect(this);
        HideWeapon();
        _weaponSoundMananger.Grab();
    }

    private void HideWeapon()
    {
        this.GetComponent<MeshRenderer>().enabled = false;
        this.GetComponent<SphereCollider>().enabled = false;
    }
  
    public void ChangeFiringMode()
    {
        _currentMode++;
        myCurrentFiringMode = availablesFiringModes[_currentMode%(availablesFiringModes.Length)];
    }
    public void ChangeFiringType(FiringType ft)
    {
        if (ft == FiringType.SINGLESHOOT) shoot = ShootOne;
        else if (ft == FiringType.BUCKETSHOOT) shoot = ShootBuck;
    }

    public void Shoot()
    {
        if (myCurrentFiringMode != null)
        {
            shooting = StartCoroutine(myCurrentFiringMode.Shoot(shoot));
        }
    }

    public void StopShoot()
    {
        if (shooting == null) return;
        StopCoroutine(shooting);
    }

    void ShootOne()
    {
        if (ammo.AMMO <= 0) { _weaponSoundMananger.NoAmmo(); return; }

        Bullet b = BulletSpawner.Instance.pool.GetObject().SetPosition(bulletOrigin);
        ammo.AMMO--;
        onUpdateAmmo(ammo);
        _weaponSoundMananger.Shoot();


    }
    void ShootBuck()
    {
        if (ammo.AMMO <= 0) { _weaponSoundMananger.NoAmmo(); return; }
        ammo.AMMO--;
        onUpdateAmmo(ammo);


        foreach (Transform t in BulletOriginBucket)
        {
            BulletSpawner.Instance.pool.GetObject().SetPosition(t);
        }

        _weaponSoundMananger.Shoot();
    }


    public void Reload()
    {
        if (reloading) return;
        if (ammo.CLIPS == 0) return;
        reloading = true;
        _weaponSoundMananger.Reload();
        StartCoroutine(ReloadWait());
    }
    IEnumerator ReloadWait() {
        yield return new WaitForSeconds(realoadTime);

        reloading = false;
        ammo.AMMO = ammo.MAX_LOADED_AMMO;
        ammo.CLIPS--;
        onUpdateAmmo(ammo);
        yield return null;
    }

}

public struct Ammo
{
    public int AMMO;
    public int MAX_LOADED_AMMO;
    public int CLIPS;
}

public enum FiringType
{
    SINGLESHOOT,
    BUCKETSHOOT,
}

