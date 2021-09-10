using System.Collections;
using System.Collections.Generic;
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

}
