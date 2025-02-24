using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class SceneChangeScript : MonoBehaviour
{
    public FadeScript fadeScript;
    public SaveLoadScript saveLoadScript;
    public PauseMenu pauseMenu;


    public void CloseGame()
    {
        StartCoroutine(Delay("quit", -1, ""));
    }

    public void Settings()
    {
        StartCoroutine(Delay("settings", -1, ""));
    }

    public void GoBack()
    {
        StartCoroutine(Delay("back", -1, ""));
    }

    public void GoBackFromLevel()
    {
        pauseMenu.Menu();
        StartCoroutine(Delay("backMenu", -1, ""));
    }

    public void EndScreenGoBack()
    {
        pauseMenu.EndScreen();
        StartCoroutine(Delay("backMenu", -1, ""));
    }

    public void GoBackToSettings()
    {
        pauseMenu.Menu();
        StartCoroutine(Delay("backSettings", -1, ""));
    }

    public IEnumerator Delay(string command, int characterIndex, string name)
    {
        if (string.Equals(command, "quit", StringComparison.OrdinalIgnoreCase))
        {
            yield return fadeScript.FadeIn(0.1f);
            PlayerPrefs.DeleteAll();
            if (UnityEditor.EditorApplication.isPlaying)
            {
                UnityEditor.EditorApplication.isPlaying = false;
            }
            else
            {
                Application.Quit();
            }
        }
        else if (string.Equals(command, "play", StringComparison.OrdinalIgnoreCase))
        {
            yield return fadeScript.FadeIn(0.1f);
            saveLoadScript.SaveGame(characterIndex, name);
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
        else if (string.Equals(command, "settings", StringComparison.OrdinalIgnoreCase))
        {
            yield return fadeScript.FadeIn(0.1f);
            SceneManager.LoadScene(2, LoadSceneMode.Single);
        }
        else if (string.Equals(command, "back", StringComparison.OrdinalIgnoreCase))
        {
            yield return fadeScript.FadeIn(0.1f);
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
        else if (string.Equals(command, "backMenu", StringComparison.OrdinalIgnoreCase))
        {
            yield return fadeScript.FadeIn(0.1f);
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
        else if (string.Equals(command, "backSettings", StringComparison.OrdinalIgnoreCase))
        {
            yield return fadeScript.FadeIn(0.1f);
            SceneManager.LoadScene(2, LoadSceneMode.Single);
        }
    }
}