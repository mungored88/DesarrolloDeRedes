using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class SpawnPlayer : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform parentTransform;
    //public Vector3 spawnPosition;
    //public Transform PlayerPos;
    public Transform player1Position;
    public Transform player2Position;

    public Image[] Bull;
    public Sprite FullBull;
    public Sprite EmptyBull;
       
    Player _player;
    Weapon ActiveWeapon;
    WeaponHolder wepHolder;
    GrenadeHolder ActiveGrenades;
    public Text life;
    public Text ammo;
    public Text MaxAmmo;

    public Text Granade;
    

    public TMPro.TextMeshProUGUI medicina;
    public TextMeshProUGUI morfi;
    public Text volveBase;

    public Image[] Weapons;
    public Image[] Slots;
    public Image[] Selected;

    public ObjetiveBox boxmedicine;
    public ObjetiveBox boxfood;

    public wincond winner;

    CinemachineFreeLook PlayerCam;
    UIManager playerUiM;
    private void Awake()
    {
        Vector3 spawnPosition;
        if (PhotonNetwork.IsMasterClient)
        {
            spawnPosition = player1Position.position;
        }
        else
        {
            spawnPosition = player2Position.position;
        }
        GameObject playerGO = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity);
        playerGO.transform.parent = parentTransform;
        playerUiM = playerGO.GetComponent<UIManager>();
        PlayerCam = FindObjectOfType<CinemachineFreeLook>();
        PlayerCam.Follow = playerGO.transform;
        PlayerCam.LookAt = playerGO.transform;
    }

    void Start()
    {
        playerUiM.Bull= Bull;
        playerUiM.FullBull=FullBull;
        playerUiM.EmptyBull=EmptyBull;
        playerUiM.life=life;
        playerUiM.ammo=ammo;
        playerUiM.MaxAmmo=MaxAmmo;
        playerUiM.Granade=Granade;
        playerUiM.medicina=medicina;
        playerUiM.morfi=morfi;
        playerUiM.volveBase=volveBase;
        playerUiM.Weapons=Weapons;
        playerUiM.Slots=Slots;
        playerUiM.Selected=Selected;
        playerUiM.boxmedicine=boxmedicine;
        playerUiM.boxfood=boxfood;
        playerUiM.winner=winner;

        Debug.Log( PhotonNetwork.PlayerList.Length);
    }

}
