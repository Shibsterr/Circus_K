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


    void Start()
    {
        characterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
        GameObject mainCharacter = Instantiate(playerPreferbs[characterIndex], spawnpoint.transform.position, Quaternion.identity);
        mainCharacter.GetComponent<NameScript>().SetPlayerName(PlayerPrefs.GetString("PlayerName"));

        // will continue later...
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
