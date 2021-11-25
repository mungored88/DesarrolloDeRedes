using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

namespace redes.parcial_2
{
    public class FAServer : MonoBehaviourPun
    {
        public static FAServer Instance; //SINGLETON

        Photon.Realtime.Player _server; //Referencia del Host real (y no de los avatares)

        public Player characterPrefab; //Prefab del Model a instanciar cuando se conecte un jugador

        Dictionary<Photon.Realtime.Player, Player> _dicModels = new Dictionary<Photon.Realtime.Player, Player>();

        // Animations? lo necesitamos?
        // Dictionary<Photon.Realtime.Player, CharacterViewFA> _dicViews = new Dictionary<Photon.Realtime.Player, CharacterViewFA>();

        public int PackagePerSecond { get; private set; }

        void Start()
        {
            DontDestroyOnLoad(this.gameObject);

            if (Instance == null)
            {
                if (photonView.IsMine)
                {
                    photonView.RPC("SetServer", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer, 1);
                }
            }
        }

        public Photon.Realtime.Player getPlayerServer()
        {
            return _server;
        }

        [PunRPC]
        void SetServer(Photon.Realtime.Player serverPlayer, int sceneIndex = 1)
        {
            Debug.Log("--- [Client] SetServer: lo llama cada player");
            if (Instance)
            {
                Destroy(this.gameObject);
                return;
            }

            Instance = this;

            _server = serverPlayer;

            PackagePerSecond = 60;

            PhotonNetwork.LoadLevel(sceneIndex);

            var playerLocal = PhotonNetwork.LocalPlayer;

            if (playerLocal != _server)
            {
                photonView.RPC("AddPlayer", _server, playerLocal);
            }
        }

        [PunRPC]
        void AddPlayer(Photon.Realtime.Player player)
        {
            Debug.Log("--- [Server] Agrego al player a la lista de players");
            StartCoroutine(WaitForLevel(player));
        }

        IEnumerator WaitForLevel(Photon.Realtime.Player player)
        {
            // TODO: iniciar cuando hayan 3 players conectados
            Debug.Log("[Server] Waiting for level.... ");
            // while (PhotonNetwork.PlayerList.Length < 2)
            // {
            //     yield return new WaitForSeconds(0.15f);
            // }

            while (PhotonNetwork.LevelLoadingProgress > 0.9f)
            {
                yield return new WaitForEndOfFrame();
            }

            // TODO: una pos por cada player
            var spawnPos = FindObjectOfType<SpawnPlayer>();
            Transform p1Position = spawnPos.player1Position;

            // El nivel esta cargado en este punto
            // TODO: crear players de acuerdo a la cant de PlayerList
            Player newCharacter = PhotonNetwork
                .Instantiate(characterPrefab.name, p1Position.position, Quaternion.identity)
                .GetComponent<Player>()
                .SetInitialParameters(player);

            Debug.Log($"--- [Server] Player {newCharacter.name} instanciado");
            _dicModels.Add(player, newCharacter);

            // _dicViews.Add(player, newCharacter.GetComponent<CharacterViewFA>());
        }

        /* REQUESTS (SERVERS AVATARES)*/

        //Esto lo recibe del Controller y llama por RPC a la funcion MOVE del host real
        public void RequestMove(Photon.Realtime.Player player, float v, float h, Vector3 cameraForward,
            Vector3 cameraRight)
        {
            float camForwardX = cameraForward.x;
            float camForwardY = cameraForward.y;
            float camForwardZ = cameraForward.z;

            float camRightX = cameraRight.x;
            float camRightY = cameraRight.y;
            float camRightZ = cameraRight.z;

            photonView.RPC("ServerMovesPlayer", _server, player, v, h,
                camForwardX, camForwardY, camForwardZ,
                camRightX, camRightY, camRightZ
            );
        }

        public void RequestShoot(Photon.Realtime.Player player)
        {
            photonView.RPC("Shoot", _server, player);
        }


        /* Requests que recibe el SERVER para gestionar a los jugadores */
        [PunRPC]
        void ServerMovesPlayer(Photon.Realtime.Player player, float v, float h, 
            float camForwardX, float camForwardY, float camForwardZ,
            float camRightX, float camRightY, float camRightZ)
        {
            if (_dicModels.ContainsKey(player))
            {
                Debug.Log("[Server] Server moves player...");
                _dicModels[player].PlayerMovedByServer(v, h, 
                    camForwardX, camForwardY, camForwardZ,
                    camRightX, camRightY, camRightZ);
            }
        }

        [PunRPC]
        void Shoot(Photon.Realtime.Player player)
        {
            if (_dicModels.ContainsKey(player))
            {
                _dicModels[player].Shoot();
            }
        }


        public void PlayerDisconnect(Photon.Realtime.Player player)
        {
            PhotonNetwork.Destroy(_dicModels[player].gameObject);
            _dicModels.Remove(player);
            // _dicViews.Remove(player);
        }
    }
}