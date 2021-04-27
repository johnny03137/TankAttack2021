using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        Vector3 pos = new Vector3(Random.Range(-150.0f, 150.0f), 5.0f, Random.Range(-150.0f, 150.0f));

        // ����� ������ ���ΰ� ĳ����(��ũ)
        PhotonNetwork.Instantiate("Tank", pos, Quaternion.identity, 0 );
    }
}
