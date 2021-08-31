using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectPool<T> //Cuando yo haga un new ObjectPool voy a tener que setear que tipo va a ser, en ESTE ejemplo va a ser tipo Bullet
{
    public delegate T FactoryMethod(); //Creo un delegado que va a devolver tipo T (bullet)
    FactoryMethod _factoryMethod; //Creo una variable de mi delegado. Voy a guardar la logica que tendra mi factory para instanciar el objecto

    List<T> _currentStock; //Lista donde voy a contener los objetos disponibles para usar
    bool _isDynamic; //Si es dinamico va a crear mas objetos cuando la lista este vacia

    Action<T> _turnOnCallback; //Usado para guardar la logica de la funcion que contiene Bullet para prenderse
    Action<T> _turnOffCallback; //Usado para guardar la logica de la funcion que contiene Bullet para apagarse

    // Cada vez que yo haga un new ObjectPool para crear un pool, voy a tener que pasarle estos parametros
    public ObjectPool(FactoryMethod factoryMethod, Action<T> turnOnCallback, Action<T> turnOffCallback, int initialStock = 0, bool isDynamic = true)
    {
        _factoryMethod = factoryMethod;
        _turnOnCallback = turnOnCallback;
        _turnOffCallback = turnOffCallback;

        _isDynamic = isDynamic;

        _currentStock = new List<T>(); //Inicializo la lista

        for (int i = 0; i < initialStock; i++)
        {
            var obj = _factoryMethod(); // Ejecuto la instanciacion del objeto
            _turnOffCallback(obj); //Lo apago con la logica del mismo objecto
            _currentStock.Add(obj); //Lo agrego a mi lista de objectos disponibles
        }

    }

    //Funcion que se va a llamar para dar un objecto de mi lista disponible
    public T GetObject()
    {
        var result = default(T); //Creo una referencia default del tipo de objeto

        if (_currentStock.Count > 0) //Si tengo stock en mi lista
        {
            result = _currentStock[0]; //Voy a devolver el primero de la lista
            _currentStock.RemoveAt(0); //Lo remuevo
        }
        else if (_isDynamic) //Sino, pero si es dinamico
        {
            result = _factoryMethod(); //Creo y devuelvo ese
        }

        _turnOnCallback(result); //Lo prendo con la logica del objecto

        return result;
    }

    //Funcion que se va a llamar cuando se quiere regresar el objecto a la lista de disponibles nuevamente
    public void ReturnObject(T obj)
    {
        _turnOffCallback(obj); //Lo apago con la logica del objeto
        _currentStock.Add(obj); //Lo guardo nuevamente en la lista
    }

}


