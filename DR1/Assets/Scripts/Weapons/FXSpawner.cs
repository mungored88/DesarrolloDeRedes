using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXSpawner : MonoBehaviour
{
    //Singleton para tener solo una referencia global de este factory
    public static FXSpawner Instance { get; private set; }

    public FX fragFxPrefab;     //Prefab del objeto que va a contener el pool
    public FX flashFxPrefab;     //Prefab del objeto que va a contener el pool
    public int fxStock = 10;    //Cantidad de objetos que se van a crear al inicio


    public ObjectPool<FX> fragPool; //El pool de mis FX
 //   public ObjectPool<FX> flashPool; //El pool de mis FX

    void Start()
    {
        Instance = this;

        fragPool = new ObjectPool<FX>(FragFXFactory, FX.TurnOn, FX.TurnOff, fxStock, true);
        //flashPool = new ObjectPool<FX>(FlashFXFactory, FX.TurnOn, FX.TurnOff, fxStock, true);

    }
    public FX FragFXFactory()
    {
        FX fragfx = Instantiate(fragFxPrefab);
        fragfx.transform.parent = this.transform;
        return fragfx;
    }
    public FX FlashFXFactory()
    {
        return Instantiate(fragFxPrefab);
    }
    public void ReturnFX(FX fx)
    {
        switch(fx.fxType){
            case FX.FXType.FRAG_EXPLOTION:
                fragPool.ReturnObject(fx);
                break;
            case FX.FXType.FLASH_EXPLOTION:
                //flashPool.ReturnObject(fx);
                break;
        }
    }


}
