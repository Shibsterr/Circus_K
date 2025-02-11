using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    TMP_Text rolledNumberText;
    PlayerScript playerScript;
    DiceRollScript diceRollScript;

    int characterIndex;



    private void Awake()
    {
        characterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);

    }
}
