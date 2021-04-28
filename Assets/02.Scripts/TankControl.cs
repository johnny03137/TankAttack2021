using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TankControl : MonoBehaviour
{
    public float speed;

    public GameObject cannonPrefab;
    public Transform firePos;
    public Transform cannonMesh;
    public AudioClip fireSfx;

    Transform tr;
    PhotonView pv;
    AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();        
        pv = GetComponent<PhotonView>();
        audio = GetComponent<AudioSource>();

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
                pv.RPC("Fire", RpcTarget.AllViaServer, null);
            }

            cannonMesh.Rotate(Vector3.right * Time.deltaTime * -r * 200.0f);
        }
    }

    [PunRPC]
    void Fire()
    {
        var obj = Instantiate(cannonPrefab, firePos.position, firePos.rotation);

        audio.PlayOneShot(fireSfx);

        //Destroy(obj.gameObject, 10f);
    }
}
