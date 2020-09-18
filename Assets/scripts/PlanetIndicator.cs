using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetIndicator : MonoBehaviour
{
    public GameObject distanceText;
    public GameObject innerCircle;
    public GameObject planet;
    public GameObject player;
    public CanvasGroup canvasGroup;
    private GameManager gameManager;
    public bool hidden = false;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    public void Hide()
    {
        hidden = true;
        canvasGroup.alpha = 0;
    }

    public void Show()
    {
        hidden = false;
        canvasGroup.alpha = 1;
    }

    public void SetHighlight()
    {
        innerCircle.GetComponent<Image>().color = Color.green;
    }

    void Update()
    {
        if (gameManager.inPlay)
        {
            SetTextPosition();
            SetDistanceText();
        }

    }

    void SetDistanceText()
    {
        distanceText.GetComponent<Text>().text = System.Math.Round(Vector3.Distance(player.transform.position, planet.transform.position), 2).ToString() + "m";
    }

    void SetTextPosition()
    {
        Vector3 position = gameObject.transform.position;
        Text text = distanceText.GetComponent<Text>();
        RectTransform parentRect = gameObject.GetComponent<RectTransform>();
        RectTransform textRect = distanceText.GetComponent<RectTransform>();

        if (position.x == Screen.width) // Right 
        {
            distanceText.transform.localPosition = new Vector3((textRect.sizeDelta.x + parentRect.sizeDelta.x) * 1.05f / -2, 0, 0);
            text.alignment = TextAnchor.MiddleRight;
        }
        else if (position.x == 0) // Left
        {
            distanceText.transform.localPosition = new Vector3((textRect.sizeDelta.x + parentRect.sizeDelta.x) * 1.05f / 2, 0, 0);
            text.alignment = TextAnchor.MiddleLeft;
        }
        else if (position.y == Screen.height) // Top
        {
            distanceText.transform.localPosition = new Vector3((textRect.sizeDelta.x / 2), (textRect.sizeDelta.y + parentRect.sizeDelta.y) * 1.05f / -2, 0);
            text.alignment = TextAnchor.MiddleLeft;
        }
        else if (position.y == 0) // Bottom
        {
            distanceText.transform.localPosition = new Vector3((textRect.sizeDelta.x / 2), (textRect.sizeDelta.y + parentRect.sizeDelta.y) * 1.05f / 2, 0);
            text.alignment = TextAnchor.MiddleLeft;
        }

    }
}
