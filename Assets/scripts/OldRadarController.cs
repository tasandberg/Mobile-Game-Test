using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldRadarController : MonoBehaviour
{
    public GameObject planetIndicator;
    private GameObject[] planets;
    private Transform player;
    GameObject planet1;
    private float canvasWidth;
    private float canvasHeight;
    private Vector2 canvasCenter;
    Renderer planet1renderer;
    // Start is called before the first frame update
    void Start()
    {
        planets = GameObject.FindGameObjectsWithTag("Navigatable");
        planet1 = GameObject.Find("Planet1");
        planet1renderer = GameObject.Find("Planet1").GetComponent<Renderer>();
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        RectTransform canvasRect = FindObjectOfType<Canvas>().GetComponent<RectTransform>();
        canvasWidth = canvasRect.rect.width;
        canvasHeight = canvasRect.rect.height;
        canvasCenter = new Vector2(canvasWidth / 2, canvasHeight / 2);
        Debug.Log("w: " + canvasWidth + " h: " + canvasHeight);
        Debug.Log("Nav point count: " + planets.Length);
        // float radarDiameter = Mathf.Sqrt(Mathf.Pow(canvasWidth, 2) + Mathf.Pow(canvasHeight, 2));
        Vector3 dir = planet1.transform.position - player.position;
        Debug.Log(dir.normalized);
    }

    Vector2 FindIndicatorPosition(float angle)
    {
        Vector2 position = new Vector2(-1, -1);
        float adjustedAngle;

        if (angle > 0 && angle <= 45)
        {
            position.y = canvasHeight;
            adjustedAngle = angle;
        }
        else if (angle > 45 && angle <= 90)
        {
            position.x = canvasWidth;
            adjustedAngle = 90 - angle;
        }
        else if (angle > 90 && angle <= 135)
        {
            position.x = canvasWidth;
            adjustedAngle = (135 - angle) * -1;
        }
        else if (angle > 135 && angle <= 180)
        {
            position.y = 0;
            adjustedAngle = 180 - angle;
        }
        else if (angle > 180 && angle <= 225)
        {
            position.y = 0;
            adjustedAngle = (225 - angle) * -1;
        }
        else if (angle > 225 && angle <= 270)
        {
            position.x = 0;
            adjustedAngle = (270 - angle) * -1;
        }
        else if (angle > 270 && angle <= 315)
        {
            position.x = 0;
            adjustedAngle = 315 - angle;
        }
        else if (angle > 315 && angle >= 360)
        {
            position.y = canvasHeight;
            adjustedAngle = (360 - angle) * -1;
        }
        else
        {
            Debug.Log("error parsing angle");
            adjustedAngle = 0;
        }

        if (position.x == -1)
        {
            position.x = position.y / Mathf.Tan(adjustedAngle);
        }
        else
        {
            position.y = position.x * Mathf.Tan(adjustedAngle);
        }

        Debug.Log("Position " + position);
        return position;
    }

    float timer = 0f;

    // Forward is 0 and increases to 360 counter clockwise
    float Angle360(Transform start, Transform target)
    {
        Vector3 direction = target.position - start.position;

        // float angle = Vector3.Angle(direction, start.forward);
        // float angle2 = Vector3.Angle(direction, start.right);

        // return angle2 > 90 ? 360 - angle : angle;

        return Vector3.SignedAngle(direction, start.forward, Vector3.up) * -1;
    }

    bool TargetVisible(GameObject target)
    {
        return target.GetComponent<MeshRenderer>().isVisible;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 3f)
        {
            timer = 0f;
            Vector3 dir = planet1.transform.position - player.position;
            Debug.Log("Direction vector " + dir.normalized);
            Debug.Log("Angle: " + Angle360(player, planet1.transform));
        }
        // if (TargetVisible(planet1))
        // {
        //     float angle = Angle360(player, planet1.GetComponent<Transform>());
        //     timer += Time.deltaTime;
        //     if (timer > 2f)
        //     {
        //         timer = 0f;
        //         Debug.Log("angle: " + angle);
        //         FindIndicatorPosition(angle);
        //     }
        // }

        // for (int i = 0; i < planets.Length; i++)
        // {
        //     planet = planets[i].GetComponent<Transform>();
        //     // Get direction of planet with respect to current heading
        //     // Get distance of planet
        //     // Detect if planet is on the screen
        //     // place planetIndicator on a circle that contains the screen but constrain to screen x and y
        //     Vector3 direction = player.position - planet.position;
        //     float angle = Vector3.Angle(player.forward, direction);
        //     if (!done) Debug.Log("planet " + (i + 1) + " " + angle);
        // }
        // done = true;
    }
}