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

        // ���ӹ��� ����
        PhotonNetwork.GameVersion = gameVersion;
        // ���� ������ ����
        // PhotonNetwork.NickName = userId;

        // ��������
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

        // ���� �뿡 ���� �޽��
        // PhotonNetwork.JoinRandomRoom();

        // �κ� ����
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Entered Lobby!!");
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

        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("BattleField");
        }        

        // ����� ������ ���ΰ� ĳ����(��ũ)
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
