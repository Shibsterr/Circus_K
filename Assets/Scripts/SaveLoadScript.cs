using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class SaveLoadScript : MonoBehaviour
{
    string saveFileName = "saveFile.json";
    int characterIndex, playerCount;
    string characterName;

    [Serializable]
    public class GameData
    {
        public int characterIndex;
        public string characterName;
        // other silly stuff (positions,time ect)
    }

    private GameData gameData = new GameData();

    public void SaveGame(int character, string name)
    {
        gameData.characterIndex = character;
        gameData.characterName = name;

        string json = JsonUtility.ToJson(gameData);
        File.WriteAllText(Application.persistentDataPath + "/" + saveFileName, json);

        Debug.Log("Game Saved to: " + Application.persistentDataPath + "/" + saveFileName);
    }

    public void LoadGame()
    {
        string filePath = Application.persistentDataPath + "/" + saveFileName;
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            gameData = JsonUtility.FromJson<GameData>(json);
            Debug.Log("Game Loaded from: " + filePath + "\nCharacter Index: " + gameData.characterIndex + "\nCharacter Name: " + gameData.characterName);
        }
        else
            Debug.LogError("Save file not found in: " + filePath);
    }

    public void Save()
    {
        PlayerPrefs.SetInt("SelectedCharacter", characterIndex);
        PlayerPrefs.SetString("PlayerName", characterName);
        PlayerPrefs.SetInt("PlayerCount", playerCount);
    }

}
