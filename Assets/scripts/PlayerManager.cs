using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    float fuel = 200;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GainFuel(float newFuel)
    {
        fuel += newFuel;
    }
}
