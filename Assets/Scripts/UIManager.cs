using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Photon.Pun;
using redes.parcial_2;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviourPun
{

    public Image[] Bull;
    public Sprite FullBull;
    public Sprite EmptyBull;


    //IObservable _Weapon;
    //IObservable _Grenades;
    Player _player;
    Weapon ActiveWeapon;
    WeaponHolder wepHolder;
    GrenadeHolder ActiveGrenades;

    public Text life;
    public Text ammo;
    public Text MaxAmmo;
  
    public Text Granade;


    public TextMeshProUGUI medicina;
    public TextMeshProUGUI morfi;
    public Text volveBase;

    public Image[] Weapons;
    public Image[] Slots;
    public Image[] Selected;
    
    public ObjetiveBox boxmedicine;
    public ObjetiveBox boxfood;

    public wincond winner;

    public void SetUIManagerForClient(Player player)
    {
        ActiveWeapon = player.ActiveWeapon;
        wepHolder = player.weaponHolder;
        ActiveGrenades = player.ActiveGrenades;
        
        // TODO: Server should not set any of this
        UpdateAmmoCount(ActiveWeapon.GetAmmo);
        UpdateGranadeCount(ActiveGrenades.grenadeHolder);


        player.onUpdateLife += LifeUpdate;
        ActiveWeapon.onUpdateAmmo += UpdateAmmoCount;
        wepHolder.onUpdateWeapon += WeaponChanged;
        wepHolder.onUpdateWeapon += SelectWeaponSlot;
        ActiveGrenades.onUpdateCount += UpdateGranadeCount;


        boxmedicine.OnGrab += ObjetiveTextMedicine;
        boxfood.OnGrab += ObjetiveTextFood;
    }

    public void UpdateAmmoCount(Ammo ammo)
    {
        this.ammo.text = ammo.AMMO.ToString();  
        MaxAmmo.text = "/"+(ActiveWeapon.GetAmmo.MAX_LOADED_AMMO * ActiveWeapon.GetAmmo.CLIPS).ToString();


        if (ActiveWeapon.GetAmmo.AMMO == 0 && ActiveWeapon.GetAmmo.CLIPS == 0) 
        {  
            UpdateNoAmmo(true, ActiveWeapon.IsPrimary? 0 : 1); 
        }

    }
    public void UpdateNoAmmo(bool noAmmo, int slot) {
        if (noAmmo)
        {
            Slots[slot].color = Color.red;
        }
        else Slots[slot].color = Color.white;
    }
    public void WeaponChanged(Weapon wep)
    {
        ActiveWeapon = wep;
        UpdateAmmoCount(wep.GetAmmo);
        ActiveWeapon = _player.ActiveWeapon;
        ActiveWeapon.onUpdateAmmo += UpdateAmmoCount;

        if(wep.IsPrimary) ShowWeaponImage(wep.Name);

        if (ActiveWeapon.GetAmmo.AMMO != 0 || ActiveWeapon.GetAmmo.CLIPS != 0)
        {
            UpdateNoAmmo(false, ActiveWeapon.IsPrimary ? 0 : 1);
        }

    }
    void ShowWeaponImage(String wep)
    {
        if(wep == "AK47") { Weapons[0].enabled = true; Weapons[1].enabled = false; }
        else if(wep == "SPAS12") { Weapons[1].enabled = true; Weapons[0].enabled = false; }
    }
    void SelectWeaponSlot(Weapon wep)
    {
        if (wep.IsPrimary) { Selected[0].enabled = true; Selected[1].enabled = false; }
        else{ Selected[1].enabled = true; Selected[0].enabled = false; }
    }
    public void LifeUpdate(float life)
    {
        if (life < 0) return;
        this.life.text = life.ToString();
    }
    public void UpdateGranadeCount(int[] grenadeCount)
    {
      Granade.text = grenadeCount[0].ToString();
    }
    public void ObjetiveTextMedicine()
    {
        medicina.fontStyle = FontStyles.Strikethrough;
        ObjetiveCompleted();
    }
    public void ObjetiveTextFood()
    {
        //morfi.fontStyle = FontStyles.Strikethrough;
        
        photonView.RPC("ObjetiveCompleted", RpcTarget.AllViaServer);
        //ObjetiveCompleted();
    }
    
    [PunRPC]
    public void ObjetiveCompleted()
    {
        // Let's win here!
        SceneManager.LoadScene("win");
        
        //if(morfi.fontStyle == FontStyles.Strikethrough && medicina.fontStyle == FontStyles.Strikethrough)
        //{
        //    volveBase.enabled = true;
        //    winner.gameObject.SetActive(true);
        //}
    }
       
}
