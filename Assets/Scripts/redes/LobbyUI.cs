using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobbyUI : MonoBehaviourPunCallbacks
{
    public InputField createInputField, joinInputField;

    public void BtnCreateRoom()
    {
        RoomOptions option = new RoomOptions();

        option.MaxPlayers = 4;

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
        PhotonNetwork.LoadLevel("Level");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed join room " + returnCode + " Message: " + message);
    }
}
