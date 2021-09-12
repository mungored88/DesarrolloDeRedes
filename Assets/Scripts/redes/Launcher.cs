using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    public GameObject mainScreen, connectedScreen;

    public void BtnConnect()
    {
        PhotonNetwork.ConnectUsingSettings(); //Conecta a Photon como esta configurado en PhotonServerSettings
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(); //Estando en el Master Server, nos conectamos al lobby donde estan las rooms
    }

    public override void OnJoinedLobby()
    {
        mainScreen.SetActive(false);
        connectedScreen.SetActive(true);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Connection failed: " + cause.ToString());
    }
}
