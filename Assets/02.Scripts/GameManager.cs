using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        Vector3 pos = new Vector3(Random.Range(-200.0f, 200.0f), 5.0f, Random.Range(-200.0f, 200.0f));

        // ����� ������ ���ΰ� ĳ����(��ũ)
        PhotonNetwork.Instantiate("Tank", new Vector3(0f, 5.0f, 0f), Quaternion.identity,0 );
    }
}
