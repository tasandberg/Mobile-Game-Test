using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitController : MonoBehaviour
{
    private Vector3 parentPosition;
    private Vector3 ellipseCenter;
    public Transform planet;
    // Orbit Parameters
    [Range(1.0f, 2.0f)]
    public float eccentricity = 1.05f;
    public float size = 20f;
    public float speed = 0.01f;
    private float semiMajor;
    private float semiMinor;

    private float alpha = 0f;
    Vector3 axisDirection;
    Vector3[] directions = { Vector3.left, Vector3.right };

    // Start is called before the first frame update
    void Start()
    {
        axisDirection = directions[Random.Range(0, 2)];
        UpdateAxes();
        ellipseCenter = GetEllipseCenter();
        parentPosition = planet.position;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEllipseCenter();
        UpdateAxes();
        EllipticalOrbit();
    }

    void UpdateAxes()
    {
        semiMajor = size / 2f;
        semiMinor = (size * (1.0f / eccentricity)) / 2f;
    }

    void UpdateEllipseCenter()
    {
        Vector3 newEllipseCenter = GetEllipseCenter();
        if (ellipseCenter != newEllipseCenter) ellipseCenter = newEllipseCenter;
    }

    // void DrawEllipseCenterAndFoci()
    // {
    //     Vector3 gizmoScale = new Vector3(0.5f, 0.75f, 0.75f);
    //     // Center
    //     orbitCenter = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    //     orbitCenter.name = "Orbit center";
    //     orbitCenter.GetComponent<Renderer>().material.color = Color.yellow;
    //     orbitCenter.transform.localScale = gizmoScale;

    //     // Opposing Focus
    //     opposingFocus = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    //     opposingFocus.name = "Opposing Focus";
    //     opposingFocus.GetComponent<Renderer>().material.color = Color.blue;
    //     opposingFocus.transform.localScale = gizmoScale;
    // }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(ellipseCenter, 0.5f);

        Gizmos.color = Color.blue;
        Vector3 ellipseAxis = (ellipseCenter - parentPosition).normalized; // Direction from focus1 to center
        Gizmos.DrawSphere(ellipseCenter + ellipseAxis * FociDistanceToCenter(), 0.5f);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(parentPosition, 0.5f);
    }

    void EllipticalOrbit()
    {
        float xCoord = ellipseCenter.x + (semiMajor * Mathf.Sin(Mathf.Deg2Rad * alpha));
        float zCoord = ellipseCenter.z + (semiMinor * Mathf.Cos(Mathf.Deg2Rad * alpha));
        alpha += speed;//can be used as speed

        gameObject.transform.position = new Vector3(xCoord, 0, zCoord);
    }

    // Get an ellipse center from focus point PARENT at given distance in random direction
    Vector3 GetEllipseCenter()
    {
        return parentPosition + axisDirection * FociDistanceToCenter();
    }

    float FociDistanceToCenter()
    {
        // c = root (a^2 - b^2)
        return Mathf.Sqrt((Mathf.Pow(semiMajor, 2) - Mathf.Pow(semiMinor, 2)));
    }
}
