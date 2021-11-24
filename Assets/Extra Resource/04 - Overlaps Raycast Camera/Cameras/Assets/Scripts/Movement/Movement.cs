using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement
{
    Player _player;
    Rigidbody _rb;
    float _jumpForce = 2500f;
    //float _rotateSpeed = 500f;

    float _speed;
    public float turnSmoothTime = 0.1f;
    //float turnSmoothVelocity;

    Transform _cam;
    Vector3 _cameraForward;
    Vector3 _cameraRight;

    IMovementMode myCurrentMovementMode;
    IMovementMode MMNormal;
    IMovementMode MMCrouch;
    IMovementMode MMPreroll;

    public void Awake()
    {
        MMNormal = new NormalMovement(_cam, _player, _rb, _speed);
        MMCrouch = new CrouchedMovement(_cam, _player, _rb, _speed);
        MMPreroll = new PrerollMovement(_cam, _player, _rb, _speed);

        myCurrentMovementMode = MMNormal;
    }
    public Movement(Player p, Camera c)
    {
        _player = p;
        _rb = _player.GetComponent<Rigidbody>();
        _speed = _player.speed;

        if (c != null)
        {
            SetCam(c);
        }
    }

    public void SetCam(Camera c)
    {
        _cam = c.transform;
        _cameraForward = new Vector3(_cam.forward.x, _player.transform.forward.y, _cam.forward.z);
        _cameraRight = new Vector3(_cam.right.x, _player.transform.forward.y, _cam.right.z);
        
        Awake();
    }

    public void ChangeMovementMode(MovementMode tipo)
    {
        if (tipo == MovementMode.NORMAL) { myCurrentMovementMode = MMNormal; }
        else if (tipo == MovementMode.CROUCHED) { myCurrentMovementMode = MMCrouch; }
        else if (tipo == MovementMode.PREROLL) { myCurrentMovementMode = MMPreroll; }
    }


    public void Move(float v, float h)
    {
        myCurrentMovementMode.Move(v, h);
    }

    public void Aim()
    {
        if (_player.isRolling) return;
        _cameraForward = new Vector3(_cam.forward.x, _player.transform.forward.y, _cam.forward.z);
        _player.transform.forward = _cameraForward.normalized;
    }



    public void Jump()  //NO SALTA
    {
        _player.isGrounded = true;
        if (!_player.isGrounded) return;
        float jumpForce = _jumpForce;
        Vector2 force = new Vector2(0, jumpForce);
        _rb.AddForce(force, ForceMode.Impulse);
    }


    public void Roll()
    {
        Vector3 direction = _player.transform.forward;
        _rb.AddForce(direction * _jumpForce, ForceMode.Impulse);
        _player.isRolling = false;
    }

}

public enum MovementMode
{
    NORMAL,
    CROUCHED,
    PREROLL,
}

                          

