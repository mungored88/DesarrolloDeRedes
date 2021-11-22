using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using redes.parcial_2;
using UnityEngine;

public class EnemySceneSpawner : MonoBehaviourPun
{
    [SerializeField] private List<Transform> spawnerPositions;
    public GameObject enemyPrefab;
    public GameObject parentGameobject;

    private void Awake()
    {
        var playerLocal = PhotonNetwork.LocalPlayer;
        
        
        // si no soy el primer cliente conectado, no quiero crear enemigos
        if (!Equals(FAServer.Instance.getPlayerServer(), playerLocal)) return;
        
        spawnerPositions = GetComponentsInChildren<Transform>().ToList();
        spawnerPositions.RemoveAt(0);
        foreach (Transform spawnerTransform in spawnerPositions)
        {
            GameObject enemy = PhotonNetwork.InstantiateRoomObject(enemyPrefab.name, 
                spawnerTransform.position, Quaternion.identity);
            enemy.transform.parent = parentGameobject.transform;
        }
    }
}
