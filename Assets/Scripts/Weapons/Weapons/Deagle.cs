using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Deagle : Weapon
{
    protected override void Awake()
    {
        base.Awake();

        Ammo am = new Ammo();
        am.MAX_LOADED_AMMO = 7;
        am.AMMO = 7;
        am.CLIPS = 3;

        ammo = am;

        availablesFiringModes = new IFiringMode[1];
        availablesFiringModes[0] = FMSingleShoot;
        myCurrentFiringMode = FMSingleShoot;

        ChangeFiringType(FiringType.SINGLESHOOT);

        this.Name = this.GetType().Name;
        this.IsPrimary = false;
    }
    
    public override void GrabThis()
    {
        base.GrabThis();
        photonView.RPC("HideWeapon", RpcTarget.AllViaServer);
        //Collect(this);
        //HideWeapon();
        //_weaponSoundMananger.Grab();
    }

    [PunRPC]
    protected override void HideWeapon()
    {
        this.GetComponent<MeshRenderer>().enabled = false;
        this.GetComponent<SphereCollider>().enabled = false;
    }
}
