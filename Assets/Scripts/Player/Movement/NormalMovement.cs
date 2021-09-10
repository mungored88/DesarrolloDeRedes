using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMovement : IMovementMode
{
    Player _player;
    Rigidbody _rb;
    Transform _cam;
    float _speed;
    Vector3 _cameraForward;
    Vector3 _cameraRight;

    public NormalMovement(Transform cam, Player player, Rigidbody rb, float speed)
    {
        _player = player;
        _speed = speed;
        _rb = rb;
        _cam = cam;
        _cameraForward = new Vector3(_cam.forward.x, _player.transform.forward.y, _cam.forward.z);
        _cameraRight = new Vector3(_cam.right.x, _player.transform.forward.y, _cam.right.z);

    }
    public void Move(float v, float h)
    {
        _cameraForward = new Vector3(_cam.forward.x, _player.transform.forward.y, _cam.forward.z);
        _cameraRight = new Vector3(_cam.right.x, _player.transform.forward.y, _cam.right.z);

        Vector3 X = _cameraRight * h * _speed;
        Vector3 Y = new Vector3(0f, _rb.velocity.y, 0f);
        Vector3 Z = _cameraForward * v * _speed;
        _rb.velocity = (Z + Y + X);
    }

}
