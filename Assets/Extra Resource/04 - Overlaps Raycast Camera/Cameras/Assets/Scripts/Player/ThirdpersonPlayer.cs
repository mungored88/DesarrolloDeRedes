using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdpersonPlayer : MonoBehaviour
{
    private Animator _anim;
    public Bullet bullet;
    public Transform bulletOrigin;
    private float _isShooting;

    //protected override void Awake()
    //{
    //    base.Awake();

    //    _anim = GetComponent<Animator>(); //Guardo mi animator
    //    Cursor.visible = false;
    //    Cursor.lockState = CursorLockMode.Locked;
    //    Cursor.lockState = CursorLockMode.Confined;
    //}

    void Update()
    {
        if (_anim.GetBool("IsShooting")) { _isShooting = 0.5f; } else { _isShooting = 1f; }
        _anim.SetFloat("Speed_Forward", Input.GetAxis("Vertical")* _isShooting); //Seteo float en el animator    
        _anim.SetFloat("Speed_Right", Input.GetAxis("Horizontal")* _isShooting); //Seteo float en el animator                             

        if (Input.GetKeyDown(KeyCode.LeftControl))
            _anim.SetBool("Crouched", true); //Seteo bool en el animator
        if (Input.GetKeyUp(KeyCode.LeftControl))
            _anim.SetBool("Crouched", false);

        //if (canUse && Input.GetMouseButtonDown(0)) { 
        //    //_anim.SetTrigger("Shoot"); //Seteo un trigger en el animator
        //    Bullet b = Instantiate(bullet);
        //    b.transform.position = bulletOrigin.position;
        //    b.transform.forward = bulletOrigin.transform.forward;
        //}

        if (Input.GetMouseButtonDown(1))
        {
            _anim.SetBool("IsShooting",true); //Seteo un trigger en el animator
            //this.GetComponent<ThirdPersonMovement>().slowDownSpeed(true);
        }
        if (Input.GetMouseButtonUp(1))
        {
            _anim.SetBool("IsShooting", false); //Seteo un trigger en el animator
            //this.GetComponent<ThirdPersonMovement>().slowDownSpeed(false);

        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            _anim.SetTrigger("Death");
            _anim.SetLayerWeight(_anim.GetLayerIndex("Shoot"), 0); //Asigno peso 0 a al Layer "Shoot" asi no se puede usar mas
        }
    }

    //public void Shoot()
    //{
    //    gun.Shoot(); //La clase Gun como hereda de IShoot, puedo llamar la funcion Shoot
    //}
}
