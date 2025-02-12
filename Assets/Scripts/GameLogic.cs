using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public TMP_Text rolledNumberText;
    public PlayerScript playerScript;

    int characterIndex;
    int[] boardPosition; //players position on the board
    public int[] boardsPlatforms;

    public GameObject target;
    public float speed;
    void Awake()
    {
        characterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
        boardPosition = new int[PlayerPrefs.GetInt("PlayerCount")];
    }

    void Update()
    {
        float step = speed * Time.deltaTime;
        // rolledNumberText.text == "4"
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerScript.mainCharacter.transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);   //puts them at scriptholder (bad)
        }
    }
}
