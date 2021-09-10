using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MyCamera
{
    private float _rot = 0;

    void LateUpdate()
    {
        _rot -= Input.GetAxis("Mouse Y") * rotSpeed;
        //Me fijo que no se pase el giro de la camara
        //El clamp me permite evitar que un valor se pase del rango dado
        _rot = Mathf.Clamp(_rot, -40, 40);
        //Solo modifico X
        myCamera.localEulerAngles = new Vector3(_rot, myCamera.localEulerAngles.y, myCamera.localEulerAngles.z); //Asigno la rotacion fixeada
    }
}
