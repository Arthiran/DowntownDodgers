using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CreateMatchScript : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject quickStartButton;
    //[SerializeField]
    //private GameObject quickCancelButton;
    [SerializeField]
    private int RoomSize;
    // Start is called before the first frame update
    void Start()
    {
        quickStartButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        quickStartButton.SetActive(true);
    }

    public void JoinRoom()
    {
        quickStartButton.SetActive(false);
        //quickCancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Quick Start");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join room");
        CreateRoom();
    }

    private void CreateRoom()
    {
        Debug.Log("Creating room");
        int randomRoomNum = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)RoomSize };
        PhotonNetwork.CreateRoom("Room" + randomRoomNum, roomOps);
        Debug.Log(randomRoomNum);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create room... trying again");
        CreateRoom();
    }

    public void QuickCancel()
    {
        //quickCancelButton.SetActive(false);
        quickStartButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }
}
