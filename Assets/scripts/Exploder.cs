using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploder : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody rb;
    public bool explode = false;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void OnMouseDown()
    {
        Debug.Log("wow");
        rb.AddExplosionForce(10f, transform.position, 10f, 10f);
    }
}
