using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RollerNumberScript : MonoBehaviour
{
    DiceRollScript diceRollScript;
    GameLogic gameLogic;

    [SerializeField]
    TMP_Text rolledNumberText;

    void Awake()
    {
        diceRollScript = FindObjectOfType<DiceRollScript>();
        gameLogic = FindObjectOfType<GameLogic>();

        if (diceRollScript == null)
            Debug.LogError("DiceRollScript not found in scene!");

        if (gameLogic == null)
            Debug.LogError("GameLogic script not found in scene!");
    }

    void Update()
    {
        if (diceRollScript != null && diceRollScript.isLanded)
        {
            rolledNumberText.text = diceRollScript.diceFaceNum;

            int rolledValue;
            if (int.TryParse(diceRollScript.diceFaceNum, out rolledValue))
            {
                // Pass the player's index (assumed here as 0 for the main player) along with the rolled value.
                gameLogic.MovePlayer(rolledValue);
            }
            else
            {
                Debug.LogError("Failed to convert diceFaceNum to an integer: " + diceRollScript.diceFaceNum);
            }
        }
        else
        {
            rolledNumberText.text = "?";
        }
    }
}
