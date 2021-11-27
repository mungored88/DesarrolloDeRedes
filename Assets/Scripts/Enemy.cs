using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using redes.parcial_2;
using UnityEngine;

public class Enemy : Entity , IDamageable
{
    public float life = 100;

    public float damage = 25f;


    //TODO Deberian ir a script de View para corresponder al MVC
    public AudioSource attack;
    public AudioSource getHit;

    [PunRPC]
    public void Die()
    {
        Destroy(this.gameObject);
    }

    public void GetDamage(float dmg)
    {
        getHit.Play();

        life -= dmg;
        if (life <= 0)
        {
            photonView.RPC("Die", RpcTarget.AllViaServer);
            //Die();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Esto lo llama al server, porque los enemies son RoomObjects creados por el server
        var player = other.GetComponent<Player>();
        if(player != null && photonView.IsMine)
        {
            Photon.Realtime.Player localPlayer = player.GetOwner();
            attack.Play();
            FAServer.Instance.RequestDamagePlayer(localPlayer, damage);
            // other.GetComponent<IDamageable>().GetDamage(damage);
        }
    }
}
