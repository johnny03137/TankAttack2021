using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Damage : MonoBehaviour
{
    public int hp = 100;

    private PhotonView pv;
    private List<MeshRenderer> renderers = new List<MeshRenderer>();
    private List<BoxCollider> colliders = new List<BoxCollider>();

    private void Start()
    {
        pv = GetComponent<PhotonView>();
        GetComponentsInChildren<MeshRenderer>(renderers);
        GetComponentsInChildren<BoxCollider>(colliders);
    }

    private void OnCollisionEnter(Collision collision)
    {        
        if( collision.collider.CompareTag("CANNON"))
        {
            string shooter = collision.gameObject.GetComponent<Cannon>().shooter;
            hp -= 10;

            if( hp <= 0)
            {
                StartCoroutine(TankDestroy(shooter));
            }
        }
    }

    IEnumerator TankDestroy(string shooter)
    {
        string msg = $"<color=#00ff00>{pv.Owner.NickName}</color> is killed by <color=#ff0000>{shooter}</color>";
        GameManager.instance.messageText.text += msg;

        // 발사로직 정지
        // 렌더러 컴포넌트 비활성화
        if(!pv.IsMine)
        {
            GetComponent<Rigidbody>().isKinematic = false;
        }
        
        foreach(var col in colliders)
        {
            col.enabled = false;
        }
        foreach(var mesh in renderers)
        {
            mesh.enabled = false;
        }

        // 5초 Waiting
        yield return new WaitForSeconds(0.5f);

        // 불규칙한 위치로 이동
        Vector3 pos = new Vector3(Random.Range(-150.0f, 150.0f), 15.0f, Random.Range(-150.0f, 150.0f));
        transform.position = pos;

        // 렌더러 컴포넌트 활성화
        if (!pv.IsMine)
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
        foreach (var col in colliders)
        {
            col.enabled = true;
        }
        foreach (var mesh in renderers)
        {
            mesh.enabled = true;
        }
    }
}
