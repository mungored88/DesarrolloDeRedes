using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

namespace redes
{
    public class ObjectiveBoxesSpawner: MonoBehaviourPun
    {
        [SerializeField] private List<Transform> spawnerPositions;
        public List<GameObject> boxesPrefabs;
        public GameObject parentGameobject;

        private UIManager _uiManager;
        
        private void Awake()
        {
            _uiManager = FindObjectOfType<UIManager>();
            // si no soy el primer cliente conectado, no quiero crear enemigos
            if (!PhotonNetwork.IsMasterClient) return;

            for (int i = 0; i < spawnerPositions.Count; i++)
            {
                GameObject newBoxesGO = PhotonNetwork.InstantiateRoomObject(
                    boxesPrefabs[i].name, 
                    spawnerPositions[i].position, Quaternion.identity);
                newBoxesGO.SetActive(true);
                newBoxesGO.transform.parent = parentGameobject.transform;
                // Set the medicine and food gameobjects in the UI manager
                if (i == 0)
                {
                    // seteo food
                    _uiManager.boxfood = newBoxesGO.GetComponent<ObjetiveBox>();
                    _uiManager.boxfood.OnGrab += _uiManager.ObjetiveTextFood;
                }
                //if (i == 1)
                //{
                //    // seteo medicine
                //    _uiManager.boxmedicine = newBoxesGO.GetComponent<ObjetiveBox>();
                //    _uiManager.boxmedicine.OnGrab += _uiManager.ObjetiveTextMedicine;
                //}
            }
        }
    }
}