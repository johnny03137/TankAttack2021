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
    public GameObject roomPrefab;
    public Transform scrollContent;

    private readonly string gameVersion = "v1.0";
    private string userId = "Johnny";

    Dictionary<string, GameObject> roomDict = new Dictionary<string, GameObject>();
    

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

    // �� ��� ����
    public override void OnRoomListUpdate(List<RoomInfo> roomList)  // �� ����� ����� �� ���� ȣ��Ǵ� �ݹ�
    {
        GameObject tempRoom = null;
        foreach (var room in roomList)
        {
            if(room.RemovedFromList == true)
            {                
                if(roomDict.TryGetValue(room.Name, out tempRoom))
                {
                    Destroy(tempRoom);                                      // UI Prefab ����
                    roomDict.Remove(room.Name);
                }                
            }
            else
            {
                if(!roomDict.ContainsKey(room.Name))
                {
                    var obj = Instantiate(roomPrefab, scrollContent);   // ��ư ������ �������ְ�

                    obj.GetComponent<RoomData>().RoomInfo = room;
                    roomDict.Add(room.Name, obj);                       // �װ� Dictionary�� �߰�
                }
                else
                {
                    roomDict.TryGetValue(room.Name, out tempRoom);
                    tempRoom.GetComponent<RoomData>().RoomInfo = room;
                }
            }
        }

    }



    #region UI_BUTTON_CALLBACK

    public void OnLoginClick()
    {
        if( string.IsNullOrEmpty(userIdText.text))
        {
            userId = $"USER_{Random.Range(0, 100):00}";
            userIdText.text = userId;
        }

        PlayerPrefs.SetString("USER_ID", userIdText.text);

        PhotonNetwork.NickName = userIdText.text;

        if(string.IsNullOrEmpty(roomNameText.text))
        {
            RoomOptions ro = new RoomOptions();
            ro.IsOpen = true;
            ro.IsVisible = true;
            ro.MaxPlayers = 20;

            roomNameText.text = $"ROOM_{Random.Range(0, 100):000}";

            PhotonNetwork.CreateRoom(roomNameText.text, ro);
        }

        PhotonNetwork.JoinRoom(roomNameText.text);

    }

    public void OnMakeRoomClick()
    {
        // �� �ɼ� ����
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 20;

        if(string.IsNullOrEmpty(roomNameText.text))
        {
            roomNameText.text = $"ROOM_{Random.Range(0, 100):000}";
        }

        // �� ����
        PhotonNetwork.CreateRoom(roomNameText.text, ro);
    }

    #endregion
}
