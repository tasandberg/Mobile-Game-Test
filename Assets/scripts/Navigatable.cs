using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigatable : MonoBehaviour
{
    void OnMouseDown()
    {
        ShipController sc = GameObject.Find("Ship").GetComponent<ShipController>();
        sc.EnableAutoPilot(gameObject);
    }
}