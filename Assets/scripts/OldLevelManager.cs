using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OldLevelManager : MonoBehaviour
{
    /** Prefabs **/
    public GameObject planetLarge;
    public GameObject planetMedium;
    public GameObject planetSmall;
    public GameObject ship;
    public GameObject home;
    private GameObject homeInstance;



    /** Difficulty Specs **/
    public int planetFuel; // Total fuel across all planets
    public float distanceModifier; // Ratio of distance to fuel to plot for planet path

    // Terrain bounds
    public int boardX = 300;
    public int boardZ = 300;



    // Corners
    Vector3 topL;
    Vector3 topR;
    Vector3 botL;
    Vector3 botR;

    // Start is called before the first frame update
    void Start()
    {
        ship = GameObject.Find("Ship");
        // Initialize Board Dimensions
        topL = new Vector3(-boardX, 0, boardZ);
        topR = new Vector3(boardX, 0, boardZ);
        botL = new Vector3(-boardX, 0, -boardZ);
        botR = new Vector3(boardX, 0, boardZ);

        // GeneratePlanets();
        // PlotPlanetRec();
        CreateLevel();
    }


    void CreateLevel()
    {
        Vector3 origin = GameObject.Find("Ship").transform.position;
        Vector3 delta;
        Vector3 newPosition;
        float angle;
        float d;
        GameObject planet;
        float fuelScore;
        int randomIndex;

        List<GameObject> planets = new List<GameObject> { planetSmall, planetMedium, planetLarge };
        int remainingFuelToPlace = planetFuel;

        while (remainingFuelToPlace > 0)
        {
            randomIndex = Random.Range(0, planets.Count);
            Debug.Log("index " + randomIndex);
            Debug.Log("plants size " + planets.Count);
            planet = planets[randomIndex];
            fuelScore = planet.GetComponent<ResourceHolder>().fuel;
            // Remove planet if its too big for remaining fuel and continue
            if (fuelScore > remainingFuelToPlace)
            {
                planets.RemoveAt(randomIndex);
                continue;
            }
            // Random position of d distance from origin
            Debug.Log("origin: " + origin);
            angle = Random.Range(0f, 180f) * Mathf.Deg2Rad;
            d = distanceModifier * fuelScore;
            delta = new Vector3(Mathf.Cos(angle) * d, 0, Mathf.Sin(angle) * d);
            newPosition = origin + delta;

            GameObject newPlanet = Instantiate(planet, newPosition, Quaternion.identity);
            remainingFuelToPlace -= (int)fuelScore;

            origin = newPlanet.transform.position;
        }

        angle = Random.Range(0f, 180f) * Mathf.Deg2Rad;
        d = Random.Range(100, 200);
        delta = new Vector3(Mathf.Cos(angle) * d, 0, Mathf.Sin(angle) * d);
        newPosition = origin + delta;
        homeInstance = Instantiate(home, newPosition, Quaternion.identity);

    }

    void PlaceHome()
    {
        List<Vector3> corners = new List<Vector3> { topL, topR, botR };
        Vector3 homeCorner = corners[Random.Range(0, 3)];
        homeInstance = (GameObject)Instantiate(home, homeCorner, Quaternion.identity);
    }

    void Update()
    {

    }

}


