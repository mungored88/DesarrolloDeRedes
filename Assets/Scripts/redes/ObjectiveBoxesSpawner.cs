using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using redes.parcial_2;
using UnityEngine;
using Player = Photon.Realtime.Player;

namespace redes
{
    public class ObjectiveBoxesSpawner: MonoBehaviourPun
    {
        [SerializeField] private List<Transform> spawnerPositions;
        public List<GameObject> boxesPrefabs;
        public GameObject parentGameobject;

        private UIManager _uiManager;

        private void Start()
        {
            var localPlayer = PhotonNetwork.LocalPlayer;
            // Crear los objective boxes solo si soy el server
            if (FAServer.Instance != null && Equals(FAServer.Instance.getPlayerServer(), localPlayer))
            {
                  ServerSetupObjectiveBoxes();  
            }
        }

        public void ServerSetupObjectiveBoxes()
        {
            for (int i = 0; i < spawnerPositions.Count; i++)
            {
                GameObject newBoxesGO = PhotonNetwork.InstantiateRoomObject(
                    boxesPrefabs[i].name, 
                    spawnerPositions[i].position, Quaternion.identity);
                newBoxesGO.SetActive(true);
                newBoxesGO.transform.parent = parentGameobject.transform;
            }
        }

        public void ClientSetupObjectiveBoxes(UIManager uiManager)
        {
            uiManager.boxfood = FindObjectOfType<ObjetiveBox>();
            uiManager.boxfood.OnGrab += uiManager.ObjetiveTextFood;
        }
    }
}