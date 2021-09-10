using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GrenadeHolder
{
    //private float range = 30f;
    
    private Transform _spawnPosition;
    private Rigidbody _playerRb;

    private Grenade[] granadesHolder = new Grenade[4];
    private int _granadeSelected = 0;
    private Grenade _activeGranade;
    
    List<IObserver> _allObserver = new List<IObserver>();

    public event Action<int[]> onUpdateCount;
    public int[] grenadeHolder { get; set; } = new int[4] { 3, 3, 3, 3 };


    #region PROPERTIES
    public int activeGranade { get { return _granadeSelected; } set { _granadeSelected = value; } }

    public float setRange { get; set; }

    #endregion
    #region BUILDER
    public GrenadeHolder setSpawnPos(Transform t)
    {
        _spawnPosition = t;
        return this;
    }
    public GrenadeHolder setPlayerRb(Rigidbody rb)
    {
        _playerRb = rb;
        return this;
    }
    #endregion

    public void Launch()
    {
        if (grenadeHolder[_granadeSelected] == 0) return;                                              //TODO:
        grenadeHolder[_granadeSelected]--;                                                            //When more grenades added
        Grenade nadeInstance = GrenadeSpawner.Instance.fragPool.GetObject().setSpawnPosition(_spawnPosition);// Instantiate(granadesHolder[_granadeSelected], spawnPosition.position, spawnPosition.rotation);
        nadeInstance.Launch(_playerRb.velocity);
        onUpdateCount(grenadeHolder);
    }
    public void ChangeNadeType()
    {
        _granadeSelected = _granadeSelected++ % granadesHolder.Length; 
    }


}


