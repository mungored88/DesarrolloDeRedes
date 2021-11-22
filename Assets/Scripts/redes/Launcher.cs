using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using redes.parcial_2;

public class Launcher : MonoBehaviourPunCallbacks
{
    public FAServer serverPrefab;
    
    // ControllerFA == PlayerController (NO es un monobehavior)
    // public ControllerFA controllerPrefab;
    
    public GameObject mainScreen, connectedScreen;

    private void Start()
    {
        // Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void BtnConnect()
    {
        PhotonNetwork.ConnectUsingSettings(); //Conecta a Photon como esta configurado en PhotonServerSettings
    }

    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;

        PhotonNetwork.JoinOrCreateRoom("MMsalaFullAuth", options, TypedLobby.Default);
    }

    //public override void OnJoinedLobby()
    //{
    //    mainScreen.SetActive(false);
    //    connectedScreen.SetActive(true);
    //}
    
    public override void OnCreatedRoom()
    {
        Debug.Log("--- OnCreatedRoom: Instantiate Server");
        PhotonNetwork.Instantiate(serverPrefab.name, Vector3.zero, Quaternion.identity);
    }
    
    public override void OnJoinedRoom()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            // no hace falta, porque lo controla PlayerController
            //PhotonNetwork.Instantiate(controllerPrefab.name, Vector3.zero, Quaternion.identity);
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Connection failed: " + cause.ToString());
    }
}
