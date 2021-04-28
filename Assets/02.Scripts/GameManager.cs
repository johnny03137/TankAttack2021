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
    public TMP_Text roomNameText;
    public TMP_Text connectInfoText;
    public Button exitButton;


    private void Awake()
    {
        Vector3 pos = new Vector3(Random.Range(-150.0f, 150.0f), 5.0f, Random.Range(-150.0f, 150.0f));

        // ����� ������ ���ΰ� ĳ����(��ũ)
        PhotonNetwork.Instantiate("Tank", pos, Quaternion.identity, 0 );
    }

    private void Start()
    {
        SetRoomInfo();
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

    public override void OnLeftRoom()  // CleanUp�� ���� �Ŀ� ȣ��Ǵ� �ݹ�
    {
        // Lobby ������ �ǵ��ư�����.
        SceneManager.LoadScene("Lobby");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        SetRoomInfo();
        string msg = $"{newPlayer.NickName} is joined room";
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        SetRoomInfo();
        string msg = $"{otherPlayer.NickName} is lefted room";
    }
}
