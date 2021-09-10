using UnityEngine;

public class BattleMechanics  //Revisar si esta clase es necesaria
{
    Player _player;
    Weapon _weapon;
    WeaponHolder _weaponHolder;
    GrenadeHolder _grenades;
    public BattleMechanics(Player p, Weapon w, WeaponHolder wh, GrenadeHolder g)
    {
        _player = p;
        _weapon = w;
        _weaponHolder = wh;
        _grenades = g;
    }

    public Weapon setWeapon { set { _weapon = value; } }

    public void Shoot()
    {
        _weapon.Shoot();
    }

    public void StopShoot()
    {
        _weapon.StopShoot();
    }

    public void ChangeWeapon(int slotPos)
    {
        _weaponHolder.ChangeWeapon(slotPos);
    }
    public void ReloadActiveWeapon()
    {
        _weapon.Reload();
    }

    public void ChangeFiringMode()
    {
        _weapon.ChangeFiringMode();
    }
    public void ChangeFiringType(FiringType ft)
    {
        _weapon.ChangeFiringType(ft);
    }

    public void launchGranade()
    {
        _grenades.Launch();
    }
    public void changeGranade()
    {
        _grenades.ChangeNadeType();
    }




}
