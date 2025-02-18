using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public GameObject[] playerPreferbs;
    int characterIndex;
    public GameObject spawnpoint;
    public GameObject mainCharacter;
    int[] otherPlayers;
    int index;
    private const string textFileName = "playerNames";


    void Awake()
    {
        characterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
        mainCharacter = Instantiate(playerPreferbs[characterIndex], spawnpoint.transform.position, Quaternion.identity);
        mainCharacter.GetComponent<NameScript>().SetPlayerName(PlayerPrefs.GetString("PlayerName"));

        otherPlayers = new int[PlayerPrefs.GetInt("PlayerCount")];
        string[] nameArray = ReadLinesFromFile(textFileName);
        for (int i = 0; i < otherPlayers.Length; i++)
        {
            Debug.Log("Count: " + i);
            float spawnRadius = 2.0f;

            Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = spawnpoint.transform.position + new Vector3(randomOffset.x, 0, randomOffset.y);
            index = Random.Range(0, playerPreferbs.Length);
            GameObject character = Instantiate(playerPreferbs[index], spawnPosition, Quaternion.identity);
            character.GetComponent<NameScript>().SetPlayerName(nameArray[Random.Range(0, nameArray.Length)]);
        }


    }

    string[] ReadLinesFromFile(string filename)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(filename);
        if (textAsset != null)
        {
            return textAsset.text.Split(new[] {'\r','\n'}, System.StringSplitOptions.RemoveEmptyEntries);
        }
        else
        {
            Debug.LogError("File not found: "+filename);
            return new string[0];
        }
    }
}
