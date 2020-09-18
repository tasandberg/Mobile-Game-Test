using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceHolder : MonoBehaviour
{
    public int fuel;
    public bool activated = false;

    private void OnTriggerEnter(Collider other)
    {

        if (activated)
        {
            Debug.Log("Island empty, resources have been collected");
            return;
        }
        Behaviour halo = (Behaviour)gameObject.GetComponent("Halo");
        halo.enabled = false;
        // PlayerManager pManager = GameObject.Find("GameManager").GetComponent<PlayerManager>();
        Debug.Log("I got bumped!! Give " + fuel + " fuel.");
        // pManager.GainFuel(fuel);
        activated = true;
    }
}
