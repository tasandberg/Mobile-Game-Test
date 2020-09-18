using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelManager : MonoBehaviour
{
    /** Prefabs **/
    public GameObject planetLarge;
    public GameObject planetMedium;
    public GameObject planetSmall;
    public GameObject shipPrefab;
    public GameObject homePrefab;

    // Util
    private Vector3 worldCenter = Vector3.zero;

    /** References **/
    private GameObject ship;
    private GameObject home;
    public List<GameObject> planets = new List<GameObject>();

    /** Difficulty Specs **/
    public int planetFuel; // Total fuel across all planets
    public float distanceModifier; // Ratio of distance to fuel to plot for planet path

    Vector3 RandomDirection()
    {
        return new Vector3(Random.value, Random.value, Random.value);
    }

    public void CreateLevel()
    {
        ship = PlaceShip();
        List<GameObject> planets = new List<GameObject> { planetSmall, planetMedium, planetLarge };
        int remainingFuelToPlace = planetFuel;

        GameObject planet;
        ResourceHolder p;
        Vector3 origin = ship.transform.position;
        Vector3 randomPosition;
        int randIndex;
        while (remainingFuelToPlace > 0)
        {
            randIndex = Random.Range(0, planets.Count);
            planet = planets[randIndex];
            p = planet.GetComponent<ResourceHolder>();
            if (p.fuel > remainingFuelToPlace)
            {
                planets.RemoveAt(randIndex);
                continue;
            }
            randomPosition = origin + RandomDirection() * Random.Range(2000, 10000);
            Instantiate(planet, randomPosition, Quaternion.identity);
            // origin = randomPosition;
            remainingFuelToPlace -= (int)p.fuel;
        }
        home = PlaceHome();
    }

    public void DestroyLevel()
    {
        Destroy(home);
        Destroy(ship);
        foreach (GameObject p in planets)
        {
            Destroy(p);
            planets.Remove(p);
        }
    }

    private GameObject PlaceShip()
    {
        Vector3 origin = new Vector3(0, 0, 0);
        GameObject newShip = Instantiate(shipPrefab, origin, Quaternion.identity);
        newShip.name = shipPrefab.name;
        return newShip;
    }

    private GameObject PlaceHome()
    {
        Vector3 position = ship.transform.position + RandomDirection() * planetFuel * distanceModifier;
        GameObject home = Instantiate(homePrefab, position, Quaternion.identity);
        home.name = homePrefab.name;
        return home;
    }
}


