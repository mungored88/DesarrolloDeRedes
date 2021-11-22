using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using redes.parcial_2;

public class LobbyUI : MonoBehaviourPunCallbacks
{
    // Parcial 2: Esta clase ya no se llama
    public InputField createInputField, joinInputField;

    public void BtnCreateRoom()
    {
        RoomOptions option = new RoomOptions();

        option.MaxPlayers = 2;
        // so enemies are not destroyed when the master disconnects
        // option.CleanupCacheOnLeave = false;

        PhotonNetwork.CreateRoom(createInputField.text, option);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room created");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed create room " + returnCode + " Message: " + message);
    }

    public void BtnJoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInputField.text);
    }

    public override void OnJoinedRoom()
    {
        // show message
        StartCoroutine(StartGameAfterTwoPlayersConnect());
        // wait for other players...
        //PhotonNetwork.LoadLevel("Level");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed join room " + returnCode + " Message: " + message);
    }

    private IEnumerator StartGameAfterTwoPlayersConnect()
    {
        // TODO: change to 2
        while (PhotonNetwork.PlayerList.Length < 2)
        {
            yield return new WaitForSeconds(0.15f);
        }
        PhotonNetwork.LoadLevel("Level");
    }
}
