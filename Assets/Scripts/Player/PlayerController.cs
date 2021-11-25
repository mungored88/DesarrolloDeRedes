using redes.parcial_2;
using UnityEngine;

public class PlayerController //ALL THE INPUT HERE
{
    Player _player;

    public PlayerController(Player p) //, BattleMechanics b, PlayerView pv)
    {
        _player = p;
        //_movement = m;
        //_battle = b;
        //_playerView = pv;

        //_playerView.Start();
    }

    public void OnUpdate()
    {
        Debug.Log($"OnUpdate called by {_player.name}");
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        
        Vector3 cameraForward = new Vector3(_player.cam.transform.forward.x, _player.transform.forward.y, _player.cam.transform.forward.z);
        Vector3 cameraRight = new Vector3(_player.cam.transform.right.x, _player.transform.forward.y, _player.cam.transform.right.z);

        // al server le tengo que pasar el forward de la cam, y me tiene que configurar la velocity
        if ((v != 0 || h != 0))
        {
            FAServer.Instance.RequestMove(_player.GetOwner(), v, h, cameraForward, cameraRight);
            //_player.Move(v, h);
        } //Move
        else FAServer.Instance.RequestMove(_player.GetOwner(), 0, 0,  cameraForward, cameraRight);

        if (Input.GetKey(KeyCode.Space))
        {
            _player.isRolling = true;
            _player.ChangeMovementMode(MovementMode.PREROLL);
        } //PreRoll

        if (Input.GetKeyUp(KeyCode.Space))
        {
            _player.Roll();
            _player.ChangeMovementMode(MovementMode.NORMAL);
        } //Roll

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            _player.ChangeMovementMode(MovementMode.CROUCHED);
        } //Crouched

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            _player.ChangeMovementMode(MovementMode.NORMAL);
        } //Crouched

        if (!Input.GetKeyDown(KeyCode.Space)) _player.Aim();


        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _player.Shoot();
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            _player.StopShoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            _player.ReloadActiveWeapon();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            _player.Interact();
        }


        //UsarEstoParaCambiarDeArma
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _player.StopShoot();
            _player.ChangeWeapon(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _player.StopShoot();
            _player.ChangeWeapon(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            _player.ChangeFiringMode();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            _player.launchGranade();
        }

        if (Input.GetKeyDown(KeyCode.T)) //SOLO HAY UNA DE MOMENTO
        {
            _player.changeGranade();
        }
    }
}