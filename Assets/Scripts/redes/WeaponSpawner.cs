using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

namespace redes
{
    public class WeaponSpawner: MonoBehaviourPun
    {
        public List<Transform> weaponSpawnerTransforms;
        public List<GameObject> weaponPrefabs;
        public GameObject parentGameobject;

        private void Awake()
        {
            // si no soy el primer cliente conectado, no quiero crear enemigos
            if (!PhotonNetwork.IsMasterClient) return;
            
            //weaponSpawnerGameObjects = GetComponentsInChildren<GameObject>().ToList();
            //weaponSpawnerGameObjects.RemoveAt(0);

            for (int i = 0; i < weaponSpawnerTransforms.Count; i++)
            {
                GameObject newWeaponGO = PhotonNetwork.InstantiateRoomObject(
                    weaponPrefabs[i].name, 
                    weaponSpawnerTransforms[i].position, Quaternion.identity);
                newWeaponGO.SetActive(true);
                newWeaponGO.transform.parent = parentGameobject.transform;
            }
            
            //foreach (Transform weaponTransform in weaponSpawnerTransforms)
            //{
            //    GameObject newWeaponGO = PhotonNetwork.InstantiateRoomObject(go.name, 
            //        weaponTransform.position, Quaternion.identity);
            //    newWeaponGO.SetActive(true);
            //    newWeaponGO.transform.parent = parentGameobject.transform;
            //}
        }
    }
}