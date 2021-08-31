
using UnityEngine;

public class AnimatorController
{
    Animator _anim;
    public AnimatorController(Animator a)
    {
        _anim = a;
        _anim.SetBool("IsShooting", true);

    }
    public void Move(float h, float v) {
        _anim.SetFloat("Speed_Forward", v);
        _anim.SetFloat("Speed_Right", h);
    }
    public void Roll()
    {
        _anim.SetTrigger("Rolling");
    }

    public void Crouch(bool crouch)
    {
        _anim.SetBool("Crouched", crouch);
    }

    public void Die()
    { 
        _anim.SetTrigger("Death");
        _anim.SetLayerWeight(_anim.GetLayerIndex("Shoot"), 0);
    }
}
