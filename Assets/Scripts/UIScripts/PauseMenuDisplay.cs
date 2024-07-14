using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuDisplay : MonoBehaviour
{
    public bool isPaused = false;
    public GameObject PauseMenuUI;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        PauseMenuUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isPaused)
            {
                Resume();
            }

            else
            {
                Pause();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("LEFT CLICKING");
        }
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Debug.Log("RESUMING GAME");
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Pause()
    {
        Cursor.lockState = CursorLockMode.Confined;
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void OpenSettings()
    {
        Debug.Log("OPENING SETTINGS");
    }

    public void QuitGame()
    {
        Debug.Log("QUITTING GAME");
        Application.Quit();
    }
}
