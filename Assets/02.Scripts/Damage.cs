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

        // �߻���� ����
        // ������ ������Ʈ ��Ȱ��ȭ
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

        // 5�� Waiting
        yield return new WaitForSeconds(0.5f);

        // �ұ�Ģ�� ��ġ�� �̵�
        Vector3 pos = new Vector3(Random.Range(-150.0f, 150.0f), 15.0f, Random.Range(-150.0f, 150.0f));
        transform.position = pos;

        // ������ ������Ʈ Ȱ��ȭ
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
