using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    //Singleton para tener solo una referencia global de este factory
    public static BulletSpawner Instance { get; private set; }

    public Bullet bulletPrefab;     //Prefab del objeto que va a contener el pool
    public int bulletStock = 10;    //Cantidad de objetos que se van a crear al inicio


    public ObjectPool<Bullet> pool; //El pool de mis Bullet

    void Start()
    {
        Instance = this;

        pool = new ObjectPool<Bullet>(BulletFactory, Bullet.TurnOn, Bullet.TurnOff, bulletStock, true);

    }
    
    //Funcion que contiene la logica de la instanciacion de la bala
    public Bullet BulletFactory()
    {
        Bullet b = Instantiate(bulletPrefab);
        b.transform.parent = this.transform;
        return b;
    }

    //Funcion que en este caso se llama desde la bala para ejecutar la devolucion del objeto al pool
    public void ReturnBullet(Bullet b)
    {
        pool.ReturnObject(b);
    }
}
