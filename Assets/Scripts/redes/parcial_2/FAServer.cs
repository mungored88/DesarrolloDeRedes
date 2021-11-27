using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace redes.parcial_2
{
    public class FAServer : MonoBehaviourPun
    {
        public int maxPlayerCount = 4;
        
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

            // loads level for the server
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
            if (PhotonNetwork.PlayerList.Length == maxPlayerCount)
            {
                Debug.Log("--- [Server] Todos los players ONLINE. Creo enemigos");
                // Instantiate enemies
                FindObjectOfType<EnemySceneSpawner>().InstantiateEnemiesAfterAllPlayerConnect();
                // habilito a todos los otros players
            }
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
        }

        /* REQUESTS (SERVERS AVATARES)*/
        public void RequestDamagePlayer(Photon.Realtime.Player player, float dmg)
        {
            if (player != _server)
            {
                _dicModels[player].PlayerDamagedByServer(player, dmg);
                //Debug.Log("--- [Cliente] Daño al player. Le envio el mensaje");
                //photonView.RPC("ServerDamagesPlayer", _server, player, dmg);
            }
        }
        
        //[PunRPC]
        //private void ServerDamagesPlayer(Photon.Realtime.Player player, float dmg)
        //{
        //    
        //    _dicModels[player].PlayerDamagedByServer(player, dmg);
        //}

        //Esto lo recibe del Controller y llama por RPC a la funcion MOVE del host real
        public void RequestMove(Photon.Realtime.Player player, float v, float h, 
            Vector3 cameraForward, Vector3 cameraRight)
        {
            float camForwardX = cameraForward.x;
            float camForwardY = cameraForward.y;
            float camForwardZ = cameraForward.z;

            float camRightX = cameraRight.x;
            float camRightY = cameraRight.y;
            float camRightZ = cameraRight.z;

            // Dont send a request unless the player presses the input button
            if (v == 0 & h == 0) return;

            photonView.RPC("ServerMovesPlayer", _server, 
                player, v, h,
                camForwardX, camForwardY, camForwardZ,
                camRightX, camRightY, camRightZ
            );
        }


        /* Requests que recibe el SERVER para gestionar a los jugadores */
        [PunRPC]
        private void ServerMovesPlayer(Photon.Realtime.Player player, float v, float h, 
            float camForwardX, float camForwardY, float camForwardZ,
            float camRightX, float camRightY, float camRightZ)
        {
            if (_dicModels.ContainsKey(player))
            {
                Debug.Log("[Server] Server moves player...");
                
                _dicModels[player].PlayerMovedByServer(v, h, 
                    camForwardX, camForwardY, camForwardZ,
                    camRightX, camRightY, camRightZ);
                // Animator update
                _dicModels[player].AnimatorUpdateByServer(v, h);
            }
        }

        public void RequestShoot(Photon.Realtime.Player player)
        {
            photonView.RPC("Shoot", _server, player);
        }

        [PunRPC]
        void Shoot(Photon.Realtime.Player player)
        {
            if (_dicModels.ContainsKey(player))
            {
                _dicModels[player].Shoot();
            }
        }

        public void SendServerPlayerDisconnect(Photon.Realtime.Player player)
        {
            photonView.RPC("PlayerDisconnect", _server, player);
        }
        
        [PunRPC]
        public void PlayerDisconnect(Photon.Realtime.Player player)
        {
            // si soy server, hago esto
            PhotonNetwork.Destroy(_dicModels[player].gameObject);
            _dicModels.Remove(player);
            // _dicViews.Remove(player);
            
            photonView.RPC("ShowDefeatScreen", player);
            // Invoke("SendDeadStatusToServerAndchangeScene",5);
            
            // si no soy server, como destruyo a ese enemigo?
        }

        [PunRPC]
        public void ShowDefeatScreen()
        {
            Invoke("SendDeadStatusToServerAndchangeScene",5);
        }
        
        //deberia estar en un game manager :)
        public void SendDeadStatusToServerAndchangeScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Defeat");
        }
    }
}