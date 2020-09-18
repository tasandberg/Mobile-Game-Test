using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIToggle : MonoBehaviour
{
    public void Show(int level)
    {
        Text text = GameObject.Find("LevelCompleteText").GetComponent<Text>();
        text.text = "LEVEL " + level.ToString() + " COMPLETE";
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
