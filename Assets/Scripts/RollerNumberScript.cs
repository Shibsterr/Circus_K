using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RollerNumberScript : MonoBehaviour
{
    DiceRollScript diceRollScript;
    [SerializeField]
    TMP_Text rolledNumberText;


    // Start is called before the first frame update
    void Awake()
    {
        diceRollScript = FindObjectOfType<DiceRollScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (diceRollScript != false)
        {
            if (diceRollScript.isLanded)
                rolledNumberText.text = diceRollScript.diceFaceNum;

            else
                rolledNumberText.text = "?";
        }
        else
            Debug.LogError("DiceRollScript not found in scene!");
    }
}
