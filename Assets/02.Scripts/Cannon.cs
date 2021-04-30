using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public float speed;
    public GameObject explosionVfx;

    public string shooter;

    Rigidbody m_rigid;


    // Start is called before the first frame update
    void Start()
    {
        m_rigid = GetComponent<Rigidbody>();
        m_rigid.AddRelativeForce(Vector3.forward * speed);
    }

    private void OnCollisionEnter(Collision other)
    {
        var obj = Instantiate(explosionVfx, transform.position, Quaternion.identity);

        Destroy(obj, 3f);

        Destroy(this.gameObject);
    }
}
