
using UnityEngine;

public class AnimatorController
{
    Animator _anim;
    public AnimatorController(Animator a)
    {
        _anim = a;
        _anim.SetBool("IsShooting", true);

    }
    public void AnimatorControllerMove(float h, float v) {
        _anim.SetFloat("Speed_Forward", v);
        _anim.SetFloat("Speed_Right", h);
    }
    public void AnimatorControllerRoll()
    {
        _anim.SetTrigger("Rolling");
    }

    public void AnimatorControllerCrouch(bool crouch)
    {
        _anim.SetBool("Crouched", crouch);
    }

    public void AnimatorControllerDie()
    { 
        _anim.SetTrigger("Death");
        _anim.SetLayerWeight(_anim.GetLayerIndex("Shoot"), 0);
    }
}
