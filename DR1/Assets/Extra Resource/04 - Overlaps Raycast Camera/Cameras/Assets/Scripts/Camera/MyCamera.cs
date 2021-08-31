using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCamera : MonoBehaviour
{
    public Transform myCamera;
    public Texture2D mousePointer;
    public float rotSpeed;

    protected virtual void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //Me permite hacer que el mouse no se mueva del centro de la pantalla
    }
}
