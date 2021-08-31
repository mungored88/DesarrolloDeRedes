using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeSpawner : MonoBehaviour
{
    //Singleton para tener solo una referencia global de este factory
    public static GrenadeSpawner Instance { get; private set; }

    public FragNade fragPrefab;     //Prefab del objeto que va a contener el pool
    //public Grenade flashFxPrefab;     //Prefab del objeto que va a contener el pool
    public int fxStock = 10;    //Cantidad de objetos que se van a crear al inicio


    public ObjectPool<Grenade> fragPool; //El pool de mis Grenade
    //public ObjectPool<Grenade> flashPool; //El pool de mis Grenade

    void Start()
    {
        Instance = this;

        fragPool = new ObjectPool<Grenade>(FragGrenadeFactory, Grenade.TurnOn, Grenade.TurnOff, fxStock, true);
    //    flashPool = new ObjectPool<Grenade>(FlashGrenadeFactory, Grenade.TurnOn, Grenade.TurnOff, fxStock, true);

    }
    public Grenade FragGrenadeFactory()
    {
        Grenade frag = Instantiate(fragPrefab);
        frag.transform.parent = this.transform;
        return frag;
    }
    public Grenade FlashGrenadeFactory()
    {
        return Instantiate(fragPrefab);
    }
    public void ReturnGrenade(Grenade nade)
    {
        switch (nade.grenadeType)
        {
            case Grenade.GrenadeType.FRAG_NADE:
                fragPool.ReturnObject(nade);
                break;
        }
    }
}
