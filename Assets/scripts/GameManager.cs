using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public int level = 1;
    public static GameManager instance = null;
    private GameObject endOfLevel;
    private Text levelCompleteText;
    public LevelManager levelManager;
    public bool inPlay = false;
    public float LEVEL_DELAY = 1f;

    // UI
    private Button continueButton;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("Game manager awake");
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        inPlay = true;
        endOfLevel = GameObject.Find("EndOfLevel");
        endOfLevel.GetComponent<CanvasGroup>().alpha = 0;
        levelCompleteText = GameObject.Find("LevelCompleteText").GetComponent<Text>();
        // continueButton = GameObject.Find("ContinueButton").GetComponent<Button>();
        // continueButton.onClick.AddListener(ContinueClick);
        endOfLevel.SetActive(false);
        levelManager.CreateLevel();
    }

    private void ShowEndOfLevel()
    {
        levelCompleteText.text = $"LEVEL {level} COMPLETE";
        endOfLevel.SetActive(true);
        endOfLevel.GetComponent<CanvasGroup>().alpha = 1;
    }

    private void HideEndOfLevel()
    {

        endOfLevel.GetComponent<CanvasGroup>().alpha = 0;
        endOfLevel.SetActive(false);
    }

    public void LevelComplete()
    {
        inPlay = false;
        levelManager.DestroyLevel();
        ShowEndOfLevel();
        /**
         * 1. Show level end screen
         * 2. (Non-mvp) Show level recap stats
         * 3. Show level starting. Hints for pilot?
         *   - What if players roll for stats like intuition (great % chance of hints about level), luck (small % change of better resources), reflexes (piloting)
         * 4. 
         */
        Debug.Log("Level complete");
        level++;
    }

    public void LoadLevel()
    {
        Invoke("ReloadScene", LEVEL_DELAY);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
