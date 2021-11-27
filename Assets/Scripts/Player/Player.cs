using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using redes;
using redes.parcial_2;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class Player : Entity , ICollector, IDamageable, IObservable
{
    private Photon.Realtime.Player _owner; // El "ID" para saber a quien representa este Model
    // private Photon.Realtime.Player _serverReference; // referencia al servidor (a quien enviarle requests)
    
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
    
    // Movement _movement;
    BattleMechanics _battleMechanics;
    SoundMananger _soundMananger;
    // AnimatorController _animatorController;
    public Animator _animator;
    public Camera cam;

    public Weapon ActiveWeapon{ get { return activeWeapon; } set { activeWeapon = value; } } 
    public GrenadeHolder ActiveGrenades{ get { return grenades; } set { grenades = value; } }

    public event Action<Weapon> weaponChanged;
    public event Action<float> onUpdateLife;
    public event Action OnGrab;

    public GameObject minimapGameObject;


    //Debug pourpuse
    public Transform CtSpawn;
    public Transform MafiaSpawn;
    
    
    // Delegate para el movimiento
    public delegate void ControlDelegate();
    private ControlDelegate _controlDelegate = () => { };

    private void Start()
    {
        // No va mas isMine, porque "son todos del server"
        if (!photonView.IsMine)
        {
            // Disable other audio listeners
            FindObjectOfType<Player>().gameObject.GetComponent<AudioListener>().enabled = false;
            // disable other player's minimap
            minimapGameObject.SetActive(false);
            return;
        }
    }
    
    private void Update()
    {
        // TODO: solo lo tiene que hacer cada cliente
        _controlDelegate.Invoke();
        //FORDEBUG
        //if (Input.GetKeyDown(KeyCode.F1)) { this.transform.position = CtSpawn.position; }
        //if (Input.GetKeyDown(KeyCode.F2)) { this.transform.position = MafiaSpawn.position; }  
        // if (Input.GetKeyDown(KeyCode.B)) { GetDamage(25); }
    }

    //public Photon.Realtime.Player GetServerReference()
    //{
    //    return _serverReference;
    //}

    public Photon.Realtime.Player GetOwner()
    {
        return _owner;
    }

    public Rigidbody GetRigidbody()
    {
        return _rb;
    }

    UIManager playerUiM;
    public Player SetInitialParameters(Photon.Realtime.Player localPlayer)
    {
        // owner es el current player
        _owner = localPlayer;
        // _movement = new Movement(this, cam);
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        
        _animator.SetBool("IsShooting", true);
        
        // le mando al player un RPC para que setee sus parametros
        photonView.RPC("SetPlayerLocalParams", localPlayer, _owner);
        
        return this;
    }

    [PunRPC]
    private void SetPlayerLocalParams(Photon.Realtime.Player localPlayer)
    {
        _owner = localPlayer;
        Debug.Log("--- [Client] player setea sus propios parametros");
        
        // Awake
        _control = new PlayerController(this);
        
        // seteado de nuevo, pero del lado del cliente
        _animator = GetComponent<Animator>();
        
        cam = Camera.main;
        _soundMananger = new SoundMananger();
        // _animatorController = new AnimatorController(_animator);
        _playerView = new PlayerView(this, _soundMananger);
        // _movement = new Movement(this, cam);;
        SetCam(cam);
        // ------------

        // Start
        var spawnPlayer = FindObjectOfType<SpawnPlayer>();
        transform.parent = spawnPlayer.parentTransform;

        var PlayerCam = FindObjectOfType<CinemachineFreeLook>();
        PlayerCam.Follow = transform;
        PlayerCam.LookAt = transform;
        
        // Setea la UI de los objetivos
        playerUiM = GetComponent<UIManager>();
        playerUiM.SetPlayer(this);

        // Hacemos lo que antes hacia el Start
        activeWeapon.BulletOrigin = bulletOrigin;
        activeWeapon.BulletOriginBucket = bulletOriginBucket;
        grenades = new GrenadeHolder().setSpawnPos(grenadeOrigin).setPlayerRb(this.GetComponent<Rigidbody>());
        weaponHolder = new WeaponHolder(this);
        weaponHolder.AddWeapon(activeWeapon);
        _battleMechanics = new BattleMechanics(this, activeWeapon, weaponHolder, grenades);
        weaponHolder.onUpdateWeapon += updateChangeWeapon;
        
        spawnPlayer.SetupUIForPlayer(playerUiM);
        playerUiM.SetUIManagerForClient(this);
        FindObjectOfType<ObjectiveBoxesSpawner>().ClientSetupObjectiveBoxes(playerUiM);
        // ------

        // hack para no llamar al OnUpdate en Update (y que se ejecute en todos los players)
        _controlDelegate += _control.OnUpdate;
    }

    public void MovementAim()
    {
        if (isRolling) return;
        _cameraForward = new Vector3(cam.transform.forward.x, transform.forward.y, cam.transform.forward.z);
        transform.forward = _cameraForward.normalized;
    }
    
    public void MovementRoll()
    {
        Vector3 direction = transform.forward;
        _rb.AddForce(direction * _jumpForce, ForceMode.Impulse);
        isRolling = false;
    }
    
    # region ANIMATOR
    public void AnimatorControllerRoll()
    {
        _animator.SetTrigger("Rolling");
    }

    public void AnimatorControllerCrouch(bool crouch)
    {
        _animator.SetBool("Crouched", crouch);
    }

    public void AnimatorControllerDie()
    { 
        _animator.SetTrigger("Death");
        _animator.SetLayerWeight(_animator.GetLayerIndex("Shoot"), 0);
    }
    
    # endregion

    # region MOVEMENT
    
    Vector3 _cameraForward;
    Vector3 _cameraRight;
    public void SetCam(Camera c)
    {
        _cameraForward = new Vector3(cam.transform.forward.x, transform.forward.y, cam.transform.forward.z);
        _cameraRight = new Vector3(cam.transform.right.x, transform.forward.y, cam.transform.right.z);
        
        SetMovementTypes();
    }
    
    IMovementMode myCurrentMovementMode;
    IMovementMode MMNormal;
    IMovementMode MMCrouch;
    IMovementMode MMPreroll;
    Rigidbody _rb;
    float _jumpForce = 2500f;
    //float _rotateSpeed = 500f;

    public float turnSmoothTime = 0.1f;
    public void SetMovementTypes()
    {
        MMNormal = new NormalMovement(cam.transform, this, _rb, speed);
        MMCrouch = new CrouchedMovement(cam.transform, this, _rb, speed);
        MMPreroll = new PrerollMovement(cam.transform, this, _rb, speed);

        myCurrentMovementMode = MMNormal;
    }
    
    public void MovementChangeMovementMode(MovementMode tipo)
    {
        if (tipo == MovementMode.NORMAL) { myCurrentMovementMode = MMNormal; }
        else if (tipo == MovementMode.CROUCHED) { myCurrentMovementMode = MMCrouch; }
        else if (tipo == MovementMode.PREROLL) { myCurrentMovementMode = MMPreroll; }
    }

    # endregion

    #region LIFE_STUFF

    //deberia estar en un game manager :)
    public void SendDeadStatusToServerAndchangeScene()
    {
        FAServer.Instance.SendServerPlayerDisconnect(GetOwner());
        UnityEngine.SceneManagement.SceneManager.LoadScene("Defeat");
    }
    #endregion

    #region SERVER METHODS (AND EXECUTED BY THE CLIENTS)
    // llamada por el cliente, se ejecuta en el server
    public void PlayerMovedByServer(float v, float h, 
        float camForwardX, float camForwardY, float camForwardZ,
        float camRightX, float camRightY, float camRightZ)
    {
        Vector3 _cameraForward = new Vector3(camForwardX, camForwardY, camForwardZ);
        Vector3 _cameraRight = new Vector3(camRightX, camRightY, camRightZ);
        
        Vector3 X = _cameraRight * h * speed;
        Vector3 Y = new Vector3(0f, _rb.velocity.y, 0f);
        Vector3 Z = _cameraForward * v * speed;
        GetRigidbody().velocity = (Z + Y + X);
    }
    
    public void PlayerDamagedByServer(Photon.Realtime.Player player, float dmg)
    {
        Debug.Log("--- [Server] Le saco vida al player");
        life -= dmg;
        
        if (life <= 0)
        {
            photonView.RPC("UpdatePlayerUIAfterDamageByServer", player, life);
            
            Debug.Log($"--- [Server] El player {player.ActorNumber} tiene que morir");
            // mandar mensaje de muerte
            photonView.RPC("PlayerDeadByServer", player);
        }
        else
        {
            // le mando al cliente el mensaje de que actualice su UI
            photonView.RPC("UpdatePlayerUIAfterDamageByServer", player, life);
        }
    }

    public void AnimatorUpdateByServer(float v, float h)
    {
        _animator.SetFloat("Speed_Forward", v);
        _animator.SetFloat("Speed_Right", h);
    }

    #endregion

    [PunRPC]
    public void UpdatePlayerUIAfterDamageByServer(float currentLife)
    {
        Debug.Log("--- [Cliente] Recibo mensaje de daño. Actualizo UI");
        life = currentLife;
        playerUiM.LifeUpdate(currentLife);
    }

    [PunRPC]
    public void PlayerDeadByServer()
    {
        Die();
        Invoke("SendDeadStatusToServerAndchangeScene",5);
    }
    
    // DISABLED
    public void GetDamage(float dmg)
    {
        // Llamado por el cliente
        life -= dmg;
        // playerUiM.LifeUpdate(life);
        onUpdateLife(life);
        if (life <= 0) Die();
    }
    
    
    // Disabled
    public void Die()
    {
        // TODO: Server
        if (PlayerLost != null) PlayerLost(this);

        AnimatorControllerDie();
        this.enabled = false;
    }
    public delegate void NotifyPlayerDie(Player T);
    public static NotifyPlayerDie PlayerLost;
    
    internal void Aim()
    {
        MovementAim();
    }
    
    internal void Roll()
    {
        // TODO: Server
        MovementRoll();
        AnimatorControllerRoll();
    }
    
    internal void ChangeMovementMode(MovementMode mm)
    {
        if (mm == MovementMode.CROUCHED) AnimatorControllerCrouch(true);
        else AnimatorControllerCrouch(false);
        MovementChangeMovementMode(mm);

    }

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
        // TODO: Server
        this.activeWeapon = wep;
        _battleMechanics.setWeapon = wep;
    }

    public void GrabWeapon(Weapon weapon)
    {
        // TODO: Server ?
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
        // TODO: Server
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

