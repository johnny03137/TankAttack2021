using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{


    private readonly string gameVersion = "v1.0";
    private string UserId = "Johnny";


    private void Awake()
    {
        // 게임버전 지정
        PhotonNetwork.GameVersion = gameVersion;
        // 게임 유저명 지정
        PhotonNetwork.NickName = UserId;

        // 서버접속
        PhotonNetwork.ConnectUsingSettings();

    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Server!!");
        PhotonNetwork.JoinRandomRoom();
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

        // 통신이 가능한 주인공 캐릭터(탱크)
        PhotonNetwork.Instantiate("Tank", new Vector3(0f, 5.0f, 0f), Quaternion.identity,0 );
    }
}
