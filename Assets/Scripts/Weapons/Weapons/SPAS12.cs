using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SPAS12 : Weapon
{
    protected override void Awake()
    {
        base.Awake();

        Ammo am = new Ammo();
        am.MAX_LOADED_AMMO = 8;
        am.AMMO = 8;
        am.CLIPS = 1;

        ammo = am;

        availablesFiringModes = new IFiringMode[1];
        availablesFiringModes[0] = FMBuckShoot;
        myCurrentFiringMode = FMBuckShoot;

        ChangeFiringType(FiringType.BUCKETSHOOT);

        this.Name = this.GetType().Name;
        this.IsPrimary = true;
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
