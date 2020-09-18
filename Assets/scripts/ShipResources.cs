using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipResources : MonoBehaviour
{
    public float maxFuel = 500f;
    public float currentFuel = 500f;
    public Image fuelBar;

    // Fuelbar colors
    Gradient gradient;
    GradientColorKey[] colorKey;
    GradientAlphaKey[] alphaKey;

    void Start()
    {
        GetObjectReferences();
        InitializeGradient();
    }

    private void GetObjectReferences()
    {
        fuelBar = GameObject.Find("Hud-Fill").GetComponent<Image>();
    }

    private void InitializeGradient()
    {
        gradient = new Gradient();
        colorKey = new GradientColorKey[3];
        colorKey[0].color = Color.red;
        colorKey[0].time = 0.0f;
        colorKey[1].color = Color.yellow;
        colorKey[1].time = 0.5f;
        colorKey[2].color = Color.green;
        colorKey[2].time = 1.0f;
        alphaKey = new GradientAlphaKey[3];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 1.0f;
        alphaKey[1].time = 0.5f;
        alphaKey[2].alpha = 1.0f;
        alphaKey[2].time = 1.0f;

        gradient.SetKeys(colorKey, alphaKey);
    }

    public void UpdateFuel(float fuelUsed)
    {
        currentFuel = fuelUsed > currentFuel ? 0 : currentFuel - fuelUsed;
        float fuelLevel = currentFuel / maxFuel;
        // fuelBar.color = gradient.Evaluate(fuelLevel);
        fuelBar.fillAmount = fuelLevel;
    }

}
