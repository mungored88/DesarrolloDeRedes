using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    //private Transform _cam;
    //private Vector3 _cameraForward;
    //private Vector3 _cameraRight;

    //protected override void Start()
    //{
    //    base.Start();

    //    _cam = Camera.main.transform;
    //    _cameraForward = new Vector3(_cam.forward.x, transform.forward.y, _cam.forward.z);
    //    _cameraRight = new Vector3(_cam.right.x, transform.forward.y, _cam.right.z);
    //}

    //public void slowDownSpeed(bool estaApundando)
    //{
    //    if (estaApundando) {
    //        speedForward = speedForward / 2;
    //        speedRight = speedRight / 2;
    //    }
    //    else if (!estaApundando)
    //    {
    //        speedForward = speedForward * 2;
    //        speedRight = speedRight * 2;
    //    }
    //}

    //public override void Move()
    //{
    //    //Obtengo el forward y el right de la camara y le quito su inclinacion en Y
    //    _cameraForward = new Vector3(_cam.forward.x, transform.forward.y, _cam.forward.z);
    //    _cameraRight = new Vector3(_cam.right.x, transform.forward.y, _cam.right.z);

    //    transform.forward = Vector3.Slerp(transform.forward, _cameraForward, Time.deltaTime * rotSpeed);   

    //    rb.velocity = (_cameraForward * Input.GetAxis("Vertical") * speedForward +
    //                    _cameraRight * Input.GetAxis("Horizontal") * speedRight);
    //   // Debug.Log(rb.velocity);
    //}
}
