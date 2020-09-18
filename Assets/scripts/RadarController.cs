using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarController : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject[] targets;
    public Hashtable indicators = new Hashtable();
    public GameObject indicatorPrefab;
    private GameObject player;
    GameObject radar;

    Hashtable debugTimers = new Hashtable();

    void Start()
    {
        targets = GameObject.FindGameObjectsWithTag("Navigatable");
        foreach (GameObject g in targets)
        {
            debugTimers.Add(g.GetInstanceID(), 0f);
        }
        radar = GameObject.Find("Radar");
    }

    private GameObject FindOrInitializeIndicator(GameObject navigatable)
    {
        int targetId = navigatable.GetInstanceID();
        player = player ?? GameObject.FindGameObjectWithTag("Player");
        GameObject indicator;
        if (indicators.ContainsKey(targetId))
        {
            indicator = (GameObject)indicators[targetId];
        }
        else
        {
            indicator = Instantiate(indicatorPrefab, navigatable.transform.position, Quaternion.identity, radar.transform);
            PlanetIndicator pi = indicator.GetComponent<PlanetIndicator>();
            pi.planet = navigatable;
            pi.player = player;
            if (navigatable.name == "HomePlanet")
            {
                pi.SetHighlight();
            }
            indicators.Add(targetId, indicator);
        }

        return indicator;
    }

    // Note: FALSE POSITIVES WHEN SCENE VIEW OPEN
    bool TargetVisible(GameObject planet)
    {
        return planet.GetComponent<MeshRenderer>().isVisible;
    }

    void LateUpdate()
    {
        targets = GameObject.FindGameObjectsWithTag("Navigatable");
        foreach (GameObject t in targets)
        {
            PlaceIndicator(t);
        }
    }

    void TimerLog(GameObject target, Vector3 screenPos)
    {
        int id = target.GetInstanceID();
        if ((float)debugTimers[id] > 3f)
        {
            debugTimers[id] = 0f;
        }
        else
        {
            debugTimers[id] = (float)debugTimers[id] + Time.deltaTime;
        }

    }

    void PlaceIndicator(GameObject target)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.transform.position);
        GameObject indicator = FindOrInitializeIndicator(target);
        PlanetIndicator pi = indicator.GetComponent<PlanetIndicator>();

        // IF within screenbounds, set marker on planet
        if (screenPos.z > 0 &&
            screenPos.x >= 0 && screenPos.x <= Screen.width &&
            screenPos.y <= Screen.height && screenPos.y >= 0)
        {

            if (!pi.hidden)
            {
                pi.Hide();
            }
            return;
        }
        else
        {
            if (pi.hidden)
            {
                pi.Show();

            }
        }

        if (screenPos.z < 0)
            screenPos *= -1;

        Vector3 screenCenter = new Vector3(Screen.width, Screen.height, 0) / 2;

        // Change 0,0 to screen center
        screenPos -= screenCenter;

        // find angle from center of screen to object
        float angle = Mathf.Atan2(screenPos.y, screenPos.x);
        angle -= 90 * Mathf.Deg2Rad;

        float cos = Mathf.Cos(angle); // Run
        float sin = -Mathf.Sin(angle); // Rise

        // This moves position along rise over run. Why 150 though? Is that a rad thing?
        screenPos = screenCenter + new Vector3(sin * 150, cos * 150, 0);

        float m = cos / sin;

        Vector3 screenBounds = screenCenter * 1f; // add padding around edge of screen ?

        // Snapping point to edge of screen

        if (cos > 0) // if fore or aft
        {
            screenPos = new Vector3(screenBounds.y / m, screenBounds.y, 0);
        }
        else
        {
            screenPos = new Vector3(-screenBounds.y / m, -screenBounds.y, 0);
        }

        // if right or left
        if (screenPos.x > screenBounds.x)
        {
            screenPos = new Vector3(screenBounds.x, screenBounds.x * m, 0);
        }
        else if (screenPos.x < -screenBounds.x)
        {
            screenPos = new Vector3(-screenBounds.x, -screenBounds.x * m, 0);
        }
        // Remove coordinate translation
        screenPos += screenCenter;

        indicator.transform.position = screenPos;
    }

}
