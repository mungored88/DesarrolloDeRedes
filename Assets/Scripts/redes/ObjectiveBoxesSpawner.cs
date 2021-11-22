using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using redes.parcial_2;
using UnityEngine;

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
            // _uiManager es un componente de los jugadores. So no existe, no hacemos nada
            _uiManager = FindObjectOfType<UIManager>();
            // si no soy el primer cliente conectado, no quiero crear enemigos
            var playerLocal = PhotonNetwork.LocalPlayer;
            
            // No quiero spawnear los objetos?
            if (!Equals(FAServer.Instance.getPlayerServer(), playerLocal))
            {
                _uiManager.boxfood = FindObjectOfType<ObjetiveBox>();
                _uiManager.boxfood.OnGrab += _uiManager.ObjetiveTextFood;
                return;
            }
            else
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
        }
    }
}