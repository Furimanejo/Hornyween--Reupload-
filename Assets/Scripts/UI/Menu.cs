using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    static Menu instance = null;
    [SerializeField] GameObject menuCanvas;
    bool isOpen = false;
    int pauseLevel = 0;
    public static int PauseLevel
    {
        set
        {
            instance.pauseLevel = value;
            if (value > 0)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;
        }
        get
        {
            return instance.pauseLevel;
        }
    }
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            TogglePauseGame();
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space))
            TogglePauseGame();
    }

    public void TogglePauseGame()
    {
        if (isOpen)
        {
            isOpen = false;
            PauseLevel--;
            menuCanvas.SetActive(false);
        }
        else
        {
            isOpen = true;
            PauseLevel++;
            menuCanvas.SetActive(true);
        }
    }

    public void LoadGameScene()
    {
        TogglePauseGame();
        PauseLevel = 0;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}