using UnityEngine;
using System;
using System.Collections;
public class BuckShoot : IFiringMode
{
    IEnumerator IFiringMode.Shoot(Action shoot)
    {
        shoot();
        yield return null;
    }
}