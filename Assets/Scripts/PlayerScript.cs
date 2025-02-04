using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public GameObject[] playerPreferbs;
    int characterIndex;
    public GameObject spawnpoint;
    int[] otherPlayers;
    int index;
    private const string textFileName = "playerNames";


    void Awake()
    {
        characterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
        GameObject mainCharacter = Instantiate(playerPreferbs[characterIndex], spawnpoint.transform.position, Quaternion.identity);
        mainCharacter.GetComponent<NameScript>().SetPlayerName(PlayerPrefs.GetString("PlayerName"));

        otherPlayers = new int[PlayerPrefs.GetInt("PlayerCount")];
        string[] nameArray = ReadLinesFromFile(textFileName);
        for (int i = 0; i<otherPlayers.Length-1; i++){
            Debug.Log("Count: " + i);
            spawnpoint.transform.position += new Vector3(0.7f, 0, 0.6f);
            index = Random.Range(0, playerPreferbs.Length - 1);
            GameObject character = Instantiate(playerPreferbs[index], spawnpoint.transform.position, Quaternion.identity);
            character.GetComponent<NameScript>().SetPlayerName(nameArray[Random.Range(0, nameArray.Length-1)]);
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
