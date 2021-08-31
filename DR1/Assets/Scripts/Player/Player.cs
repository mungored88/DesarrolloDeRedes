using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity , ICollector, IDamageable, IObservable
{
    [Header("Movement")]
    public float speed;
    public bool isGrounded;
    public bool isRolling = false;

    [Header("Battle")]
    public bool canAttack;
    public bool isReloading;
    public bool isShooting;
    public Bullet bullet;
    public Transform bulletOrigin;
    public Transform[] bulletOriginBucket;
    public Weapon activeWeapon;
    public WeaponHolder weaponHolder;


    public Transform grenadeOrigin;
    public GrenadeHolder grenades;

    [Header("HP/AR")]
    public float life = 100;
    public float armor;
    PlayerController _control;
    PlayerView _playerView;
    Movement _movement;
    BattleMechanics _battleMechanics;
    SoundMananger _soundMananger;
    AnimatorController _animatorController;
    public Animator _animator;
    public Camera cam;

    public Weapon ActiveWeapon{ get { return activeWeapon; } set { activeWeapon = value; } } 
    public GrenadeHolder ActiveGrenades{ get { return grenades; } set { grenades = value; } }

    public event Action<Weapon> weaponChanged;
    public event Action<float> onUpdateLife;
    public event Action OnGrab;


    //Debug pourpuse
    public Transform CtSpawn;
    public Transform MafiaSpawn;

    private void Start()
    {
        _control = new PlayerController(this);
        _animator = this.GetComponent<Animator>();

        cam = Camera.main;
        _soundMananger = new SoundMananger();
        _animatorController = new AnimatorController(_animator);
        _playerView = new PlayerView(this, _animatorController, _soundMananger);
        _movement = new Movement(this, cam);

        activeWeapon.BulletOrigin = bulletOrigin;
        activeWeapon.BulletOriginBucket = bulletOriginBucket;
        grenades = new GrenadeHolder().setSpawnPos(grenadeOrigin).setPlayerRb(this.GetComponent<Rigidbody>());

        weaponHolder = new WeaponHolder(this);

        weaponHolder.AddWeapon(activeWeapon);

        _battleMechanics = new BattleMechanics(this, activeWeapon, weaponHolder, grenades);

        weaponHolder.onUpdateWeapon += updateChangeWeapon;

    }


    void Update()
    {
        _control.OnUpdate();

        //FORDEBUG
        //if (Input.GetKeyDown(KeyCode.F1)) { this.transform.position = CtSpawn.position; }
        //if (Input.GetKeyDown(KeyCode.F2)) { this.transform.position = MafiaSpawn.position; }  
        if (Input.GetKeyDown(KeyCode.B)) { GetDamage(25); }
    }


    #region LIFE_STUFF
    public void GetDamage(float dmg)
    {
        life -= dmg;
        onUpdateLife(life);

        if (life <= 0) Die();
    }
    
    public void Die()
    {
        _playerView.animator.Die();
        this.enabled = false;
        Invoke("changeScene",5);
    }
    //deberia estar en un game manager :)
    public void changeScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Defeat");
    }
    #endregion

    #region MOVEMENT
    internal void Aim()
    {
        _movement.Aim();
    }
    internal void Move(float v, float h)
    {
        _movement.Move(v, h);
        _playerView.animator.Move(h,v);
    }
    internal void Roll()
    {
        _movement.Roll();
        _playerView.animator.Roll();
    }
    internal void ChangeMovementMode(MovementMode mm)
    {
        if (mm == MovementMode.CROUCHED) _playerView.animator.Crouch(true);
        else _playerView.animator.Crouch(false);
        _movement.ChangeMovementMode(mm);

    }

    #endregion

    #region BATTLE_MECHANICS
    public bool shooting
    {
        get { return isShooting; }
        set { isShooting = value; }
    }

    public Weapon SetActiveWeapon { get { return activeWeapon; } set { activeWeapon = value; } }

    internal void Shoot()
    {
        _battleMechanics.Shoot();
    }
    internal void StopShoot()
    {
        _battleMechanics.StopShoot();
    }
    internal void ReloadActiveWeapon()
    {
        _battleMechanics.ReloadActiveWeapon();
    }
    internal void ChangeFiringMode()
    {
        _battleMechanics.ChangeFiringMode();
    }   
    internal void ChangeWeapon(int slotPos)
    {
        _battleMechanics.ChangeWeapon(slotPos);
    }

    void updateChangeWeapon(Weapon wep)
    {
        this.activeWeapon = wep;
        _battleMechanics.setWeapon = wep;
    }

    public void GrabWeapon(Weapon weapon)
    {
        weaponHolder.AddWeapon(weapon);
        if (weapon.IsPrimary) ChangeWeapon(1);
        else if (!weapon.IsPrimary) ChangeWeapon(2);

    }

    //TODO: Generalizar esto para todo ICollectable
    internal void Interact()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 5);
        foreach(Collider c in hitColliders)
        {
            if(c.GetComponent<ICollectable<Weapon>>() != null)
            {
                c.GetComponent<ICollectable<Weapon>>().Collect += GrabWeapon;
                Weapon wep = c.GetComponent<Weapon>();
                wep.GrabThis();
                wep.BulletOrigin = this.bulletOrigin;
                wep.BulletOriginBucket = this.bulletOriginBucket;
            }
        }
    }

    internal void changeGranade()
    {
        _battleMechanics.changeGranade();        
    }
    internal void launchGranade()
    {
        _battleMechanics.launchGranade();
    }

    #endregion

    #region IOBSERVER
    public void Subscribe(IObserver obs)
    {
        throw new System.NotImplementedException();
    }

    public void Unsubscribe(IObserver obs)
    {
        throw new System.NotImplementedException();
    }

    public void NotifyToObservers(string action)
    {
        throw new System.NotImplementedException();
    }

    #endregion


}

