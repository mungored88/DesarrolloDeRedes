using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MyCamera
{
    public Transform pivot;
    public float lerpSpeed;

    private float RotY = 0;
    private float RotX = 0;

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, pivot.position, Time.deltaTime * lerpSpeed);

        RotY += Input.GetAxis("Mouse X") * rotSpeed;
        //RotX -= Input.GetAxis("Mouse Y") * rotSpeed;
        RotX = Mathf.Clamp(RotX, -20, 20);

        transform.localEulerAngles = new Vector3(RotX, RotY, transform.localEulerAngles.z);

        myCamera.LookAt(pivot); //Hace que el forward del objeto apunte hacia el transform asignado
    }
}
