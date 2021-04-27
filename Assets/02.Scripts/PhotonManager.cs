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
        // ���ӹ��� ����
        PhotonNetwork.GameVersion = gameVersion;
        // ���� ������ ����
        PhotonNetwork.NickName = UserId;

        // ��������
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

        // �� �ɼ� ����
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 30;

        // �� ����
        PhotonNetwork.CreateRoom("My Room", ro);
    }
        
    public override void OnCreatedRoom()    // ������Ϸ� �ݹ�
    {
        Debug.Log("Create Room Complete");
    }

    public override void OnJoinedRoom()     // �뿡 �������� �� ȣ��Ǵ� �ݹ�
    {
        Debug.Log("Room Entered Complete");
        Debug.Log(PhotonNetwork.CurrentRoom.Name);

        // ����� ������ ���ΰ� ĳ����(��ũ)
        PhotonNetwork.Instantiate("Tank", new Vector3(0f, 5.0f, 0f), Quaternion.identity,0 );
    }
}
