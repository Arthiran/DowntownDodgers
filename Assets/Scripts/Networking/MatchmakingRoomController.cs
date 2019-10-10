using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MatchmakingRoomController : MonoBehaviourPunCallbacks
{
    public string lobbyRoomScene;

    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        PhotonNetwork.LoadLevel(lobbyRoomScene);
    }
}
