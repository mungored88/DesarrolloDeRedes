using UnityEngine;
using System;
using System.Collections;

public class AutomaticShoot : IFiringMode
{
    IEnumerator IFiringMode.Shoot(Action shoot)
    {
        while (true) { 
            shoot();
            yield return new WaitForSeconds(0.1f);
        }

    }
}