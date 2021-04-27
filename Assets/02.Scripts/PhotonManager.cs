using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField userIdText;
    public TMP_InputField roomNameText;

    private readonly string gameVersion = "v1.0";
    private string userId = "Johnny";


    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        // 게임버전 지정
        PhotonNetwork.GameVersion = gameVersion;
        // 게임 유저명 지정
        // PhotonNetwork.NickName = userId;

        // 서버접속
        PhotonNetwork.ConnectUsingSettings();
    }

    private void Start()
    {
        userId = PlayerPrefs.GetString("USER_ID", $"USER_{Random.Range(0, 100):00}");
        userIdText.text = userId;
        PhotonNetwork.NickName = userId;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Server!!");

        // 랜덤 룸에 들어가는 메써드
        // PhotonNetwork.JoinRandomRoom();

        // 로비 입장
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Entered Lobby!!");
    }


    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"code = {returnCode}, msg = {message}");

        // 룸 옵션 설정
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 30;

        // 룸 생성
        PhotonNetwork.CreateRoom("My Room", ro);
    }
        
    public override void OnCreatedRoom()    // 룸생성완료 콜백
    {
        Debug.Log("Create Room Complete");
    }

    public override void OnJoinedRoom()     // 룸에 입장했을 때 호출되는 콜백
    {
        Debug.Log("Room Entered Complete");
        Debug.Log(PhotonNetwork.CurrentRoom.Name);

        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("BattleField");
        }        

        // 통신이 가능한 주인공 캐릭터(탱크)
        // PhotonNetwork.Instantiate("Tank", new Vector3(0f, 5.0f, 0f), Quaternion.identity,0 );
    }

    public void OnLoginClick()
    {
        if( string.IsNullOrEmpty(userIdText.text))
        {
            userId = $"USER_{Random.Range(0, 100):00}";
            userIdText.text = userId;
        }

        PlayerPrefs.SetString("USER_ID", userIdText.text);

        PhotonNetwork.NickName = userIdText.text;
        PhotonNetwork.JoinRandomRoom();
    }
}
