using UnityEngine;
using System;
using System.Collections;

internal class BurstShoot :  IFiringMode
{
    IEnumerator IFiringMode.Shoot(Action shoot)
    {
        shoot();
        yield return new WaitForSeconds(0.1f);
        shoot();
        yield return new WaitForSeconds(0.1f);
        shoot();
        yield return new WaitForSeconds(0.1f);
        yield return null;
  
    }
}