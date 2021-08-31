using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player2 : MonoBehaviour
{
    public Gun gun;

    protected bool canUse = true;

    private IMove _movement;

    protected virtual void Awake()
    {
        gun.canUse += CanUse; //me subscribo al delegado para saber si puedo usar el armar o no
        _movement = GetComponent<IMove>(); //Busco el componente que me permite moverme
    }

    private void FixedUpdate()
    {
        _movement.Move();
    }

    public void CanUse(bool canUse)
    {
        this.canUse = canUse;
    }
}
