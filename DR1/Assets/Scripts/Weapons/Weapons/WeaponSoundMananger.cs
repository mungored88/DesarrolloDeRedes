using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSoundMananger 
{
    private Weapon _weapon;
    public WeaponSoundMananger(Weapon w) {
        _weapon = w;
    }

    public void Shoot()
    {
        _weapon.shootSound.Play();
    }

    public void NoAmmo()
    {
        _weapon.noAmmoSound.Play();
    }

    public void Reload()
    {
        _weapon.reloadSound.Play();
    }
    public void Grab()
    {
        _weapon.grabSound.Play();
    }
}
