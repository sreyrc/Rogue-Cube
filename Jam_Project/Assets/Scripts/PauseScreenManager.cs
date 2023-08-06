using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreenManager : MonoBehaviour
{
    [SerializeField] GameObject pauseScreen;

    bool isPaused = false;
    private void Start()
    {
        pauseScreen.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            isPaused = !isPaused;
            if (isPaused) { PauseGame(); }
            else { ResumeGame(); }
        }
    }

    public void PauseGame()
    {
        pauseScreen.SetActive(true);
        Time.timeScale = 0.0f;
        isPaused = true;
    }
    
    public void ResumeGame()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;
    } 
}
