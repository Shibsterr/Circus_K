using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu = null;
    [SerializeField] GameObject leaderBoard;
    [SerializeField] GameObject Settings;

    bool isPaused, LeaderBoard, settings;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0 : 1;
            pauseMenu.SetActive(isPaused);
        }
    }

    public void Menu()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        pauseMenu.SetActive(isPaused);
    }

    public void LeaderboardShow()
    {
        LeaderBoard = !LeaderBoard;
        leaderBoard.SetActive(LeaderBoard);
    }

    public void SettingsShow()
    {
        settings = !settings;
        Settings.SetActive(settings);
    }

}