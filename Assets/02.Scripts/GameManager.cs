using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;



public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance = null;

    [Header("Chatting UI")]
    public TMP_Text chatListText;
    public TMP_InputField msgIF;

    [Header("Game UI")]
    public TMP_Text roomNameText;
    public TMP_Text connectInfoText;
    public Button exitButton;
    public TMP_Text messageText;

    private PhotonView pv;



    private void Awake()
    {
        instance = this;

        Vector3 pos = new Vector3(Random.Range(-150.0f, 150.0f), 15.0f, Random.Range(-150.0f, 150.0f));

        // 통신이 가능한 주인공 캐릭터(탱크)
        PhotonNetwork.Instantiate("Tank", pos, Quaternion.identity, 0 );
    }

    private void Start()
    {
        SetRoomInfo();

        pv = GetComponent<PhotonView>();
    }

    void SetRoomInfo()
    {
        Room currentRoom = PhotonNetwork.CurrentRoom;
        roomNameText.text = currentRoom.Name;
        connectInfoText.text = $"{currentRoom.PlayerCount}/{currentRoom.MaxPlayers}";
    }

    public void OnExitClick()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()  // CleanUp이 끝난 후에 호출되는 콜백
    {
        // Lobby 씬으로 되돌아가야함.
        SceneManager.LoadScene("Lobby");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        SetRoomInfo();
        string msg = $"\n<color=#00ff00>{newPlayer.NickName}</color> is joined room";
        messageText.text += msg;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        SetRoomInfo();
        string msg = $"\n<color=#ff0000>{otherPlayer.NickName}</color> is lefted room";
        messageText.text += msg;
    }

    public void OnSendClick()
    {
        string _msg = $"<color=#00ff00>[{PhotonNetwork.NickName}]</color> {msgIF.text}";
        pv.RPC("SendChatMessage", RpcTarget.AllBufferedViaServer, _msg);
    }

    [PunRPC]
    void SendChatMessage(string msg)
    {
        chatListText.text += $"{msg}\n";
    }
}
