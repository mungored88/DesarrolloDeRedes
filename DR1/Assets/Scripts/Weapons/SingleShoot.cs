using UnityEngine;
using System;
using System.Collections;

public class SingleShoot : IFiringMode
{
    IEnumerator IFiringMode.Shoot(Action shoot)
    {
        shoot();
        yield return null;
    }
}