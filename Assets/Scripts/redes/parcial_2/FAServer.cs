using System.Collections;
using System.Collections.Generic;
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

        [PunRPC]
        void SetServer(Photon.Realtime.Player serverPlayer, int sceneIndex = 1)
        {
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
            StartCoroutine(WaitForLevel(player));
        }

        IEnumerator WaitForLevel(Photon.Realtime.Player player)
        {
            Debug.Log("Waiting for level.... ");
            while (PhotonNetwork.PlayerList.Length < 2)
            {
                yield return new WaitForSeconds(0.15f);
            }
            
            while (PhotonNetwork.LevelLoadingProgress > 0.9f)
            {
                yield return new WaitForEndOfFrame();
            }

            Player newCharacter = PhotonNetwork
                .Instantiate(characterPrefab.name, Vector3.zero, Quaternion.identity)
                .GetComponent<Player>();
                //.SetInitialParameters(player);
            
            Debug.Log("--- WaitForLevel: Instantiated Player");
            _dicModels.Add(player, newCharacter);
            
            // _dicViews.Add(player, newCharacter.GetComponent<CharacterViewFA>());
        }
        
        public Photon.Realtime.Player getPlayerServer()
        {
            return _server;
        }

        /* REQUESTS (SERVERS AVATARES)*/

        //Esto lo recibe del Controller y llama por RPC a la funcion MOVE del host real
        public void RequestMove(Photon.Realtime.Player player, float v, float h)
        {
            photonView.RPC("Move", _server, player, v, h);
        }

        public void RequestShoot(Photon.Realtime.Player player)
        {
            photonView.RPC("Shoot", _server, player);
        }
        
        
        /* Requests que recibe el SERVER para gestionar a los jugadores */
        [PunRPC]
        void Move(Photon.Realtime.Player player, float v, float h)
        {
            if (_dicModels.ContainsKey(player))
            {
                Debug.Log("Server moves player...");
                _dicModels[player].Move(v, h);
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