using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AK47 : Weapon
{

    protected override void Awake()
    {
        base.Awake();

        Ammo am = new Ammo();
        am.MAX_LOADED_AMMO = 30;
        am.AMMO = 30;
        am.CLIPS = 3;

        ammo = am;

        availablesFiringModes = new IFiringMode[1];
        availablesFiringModes[0] = FMAutomaticShoot;
        myCurrentFiringMode = FMAutomaticShoot;

        ChangeFiringType(FiringType.SINGLESHOOT);

        this.Name = this.GetType().Name;
        this.IsPrimary = true;
        
    }

}
