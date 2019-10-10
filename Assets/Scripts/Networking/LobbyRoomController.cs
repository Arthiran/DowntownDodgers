using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class LobbyRoomController : MonoBehaviourPunCallbacks
{
    private PhotonView myPhotonView;

    [SerializeField]
    private string multiplayerScene;
    [SerializeField]
    private string menuScene;
    private int playerCount;
    private int roomSize;
    [SerializeField]
    private int minPlayersToStart;

    [SerializeField]
    private Text playerCountDisplay;
    [SerializeField]
    private Text timerToStartDisplay;

    private bool readyToCountdown;
    private bool readyToStart;
    private bool StartingGame;

    private float timerToStartGame;
    private float notFullGameTimer;
    private float fullGameTimer;

    [SerializeField]
    private float maxWaitTime;
    [SerializeField]
    private float maxFullGameWaitTime;

    // Start is called before the first frame update
    void Start()
    {
        myPhotonView = GetComponent<PhotonView>();
        fullGameTimer = maxFullGameWaitTime;
        notFullGameTimer = maxWaitTime;
        timerToStartGame = maxWaitTime;

        PlayerCountUpdate();
    }

    // Update is called once per frame
    private void Update()
    {
        WaitingForMorePlayers();
    }

    private void PlayerCountUpdate()
    {
        playerCount = PhotonNetwork.PlayerList.Length;
        roomSize = PhotonNetwork.CurrentRoom.MaxPlayers;
        playerCountDisplay.text = playerCount + ":" + roomSize;

        if (playerCount == roomSize)
        {
            readyToStart = true;
        }
        else if (playerCount >= minPlayersToStart)
        {
            readyToCountdown = true;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PlayerCountUpdate();
        if (PhotonNetwork.IsMasterClient)
        {
            myPhotonView.RPC("RPC_SendTimer", RpcTarget.Others, timerToStartGame);
        }
    }

    [PunRPC]
    private void RPC_SendTimer(float timeIn)
    {
        timerToStartGame = timeIn;
        notFullGameTimer = timeIn;

        if(timeIn < fullGameTimer)
        {
            fullGameTimer = timeIn;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PlayerCountUpdate();
    }

    private void WaitingForMorePlayers()
    {
        if (playerCount<= 1)
        {
            ResetTimer();
        }

        if (readyToStart)
        {
            fullGameTimer -= Time.deltaTime;
            timerToStartGame = fullGameTimer;
        }
        else if (readyToCountdown)
        {
            notFullGameTimer -= Time.deltaTime;
            timerToStartGame = notFullGameTimer;
        }

        string tempTimer = string.Format("{0:00}", timerToStartGame);
        timerToStartDisplay.text = tempTimer;
        if (timerToStartGame <= 0f)
        {
            if (StartingGame)
            {
                return;
            }
            StartGame();
        }
    }

    private void ResetTimer()
    {
        timerToStartGame = maxWaitTime;
        notFullGameTimer = maxWaitTime;
        fullGameTimer = maxFullGameWaitTime;
    }

    private void StartGame()
    {
        StartingGame = true;
        if(!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(multiplayerScene);
     }

    public void LeaveMatchmaking()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(menuScene);
    }
}
