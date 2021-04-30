using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityStandardAssets.Utility;

public class TankControl : MonoBehaviour, IPunObservable
{
    public float speed;

    public GameObject cannonPrefab;
    public Transform firePos;
    public Transform cannonMesh;
    public AudioClip fireSfx;

    public TMPro.TMP_Text userIdText;

    Transform tr;
    PhotonView pv;
    AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();        
        pv = GetComponent<PhotonView>();
        audio = GetComponent<AudioSource>();

        userIdText.text = pv.Owner.NickName;

        if (pv.IsMine)
        {
            Camera.main.GetComponent<UnityStandardAssets.Utility.SmoothFollow>().target = tr.Find("CamPivot").transform;
            GetComponent<Rigidbody>().centerOfMass = new Vector3(0f, -5.0f, 0f);

        }
        else
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine)
        {
            float v = Input.GetAxis("Vertical");
            float h = Input.GetAxis("Horizontal");
            float r = Input.GetAxis("Mouse ScrollWheel");

            tr.Translate(Vector3.forward * Time.deltaTime * speed * v);

            tr.Rotate(Vector3.up * Time.deltaTime * 120.0f * h);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                pv.RPC("Fire", RpcTarget.AllViaServer, pv.Owner.NickName);
            }

            cannonMesh.Rotate(Vector3.right * Time.deltaTime * -r * 200.0f);
        }
        else
        {
            if((tr.position - receivePos).sqrMagnitude > 3.0f * 3.0f)
            {
                tr.position = receivePos;
            }
            else
            {
                tr.position = Vector3.Lerp(tr.position, receivePos, Time.deltaTime * 10.0f);
            }            
            tr.rotation = Quaternion.Slerp(tr.rotation, receiveRot, Time.deltaTime * 10.0f);
        }
    }

    [PunRPC]
    void Fire(string shooterName)
    {
        if (audio == null)
        {
            audio = GetComponent<AudioSource>();
        }

        audio?.PlayOneShot(fireSfx);

        GameObject _cannon = Instantiate(cannonPrefab, firePos.position, firePos.rotation);
        _cannon.GetComponent<Cannon>().shooter = shooterName;
        
        //Destroy(obj.gameObject, 10f);
    }

    Vector3 receivePos = Vector3.zero;              // 네트워크를 통해서 수신 받을 변수
    Quaternion receiveRot = Quaternion.identity;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting) // 내 PhotonView == true 일 때,
        {
            stream.SendNext(tr.position);   // 위치 보내기
            stream.SendNext(tr.rotation);   // 회전값 보내기
        }
        else
        {
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
