using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class Turret : MonoBehaviour
{

    //PhotonView pv;

    // Start is called before the first frame update
    void Start()
    {
        //pv = GetComponent<PhotonView>();

        //this.enabled = pv.IsMine;
    }

    // Update is called once per frame
    void Update()
    {
        float r = Input.GetAxisRaw("Mouse X");
        transform.Rotate(Vector3.up * Time.deltaTime * 200.0f * r);
    }
}
