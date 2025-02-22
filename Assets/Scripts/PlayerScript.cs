using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public GameObject[] playerPreferbs;
    int characterIndex;
    public GameObject spawnpoint;
    public GameObject mainCharacter;
    private List<GameObject> aiCharacters = new List<GameObject>();
    private const string textFileName = "playerNames";

    void Awake()
    {
        characterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
        mainCharacter = Instantiate(playerPreferbs[characterIndex], spawnpoint.transform.position, Quaternion.identity);
        mainCharacter.GetComponent<NameScript>().SetPlayerName(PlayerPrefs.GetString("PlayerName"));

        int playerCount = PlayerPrefs.GetInt("PlayerCount", 7) - 1; // 6 AI players
        string[] nameArray = ReadLinesFromFile(textFileName);
        for (int i = 0; i < playerCount; i++)
        {
            float spawnRadius = 2.0f;
            Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = spawnpoint.transform.position + new Vector3(randomOffset.x, 0, randomOffset.y);
            int index = Random.Range(0, playerPreferbs.Length);
            GameObject character = Instantiate(playerPreferbs[index], spawnPosition, Quaternion.identity);
            character.GetComponent<NameScript>().SetPlayerName(nameArray[Random.Range(0, nameArray.Length)]);
            aiCharacters.Add(character);
        }
    }

    string[] ReadLinesFromFile(string filename)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(filename);
        if (textAsset != null)
        {
            return textAsset.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        }
        else
        {
            Debug.LogError("File not found: " + filename);
            return new string[0];
        }
    }

    public List<GameObject> GetAllPlayers()
    {
        List<GameObject> allPlayers = new List<GameObject> { mainCharacter };
        allPlayers.AddRange(aiCharacters);
        return allPlayers;
    }
}
