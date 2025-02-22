using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideDetectionScript : MonoBehaviour
{
    DiceRollScript diceRollScript;

    void Awake()
    {
        diceRollScript = FindObjectOfType<DiceRollScript>();

        if (diceRollScript == null)
        {
            Debug.LogError("DiceRollScript not found in scene!");
        }
    }

    private void OnTriggerStay(Collider sideCollider)
    {
        if (diceRollScript != null)
        {
            if (diceRollScript.GetComponent<Rigidbody>().velocity.magnitude < 0.1f)
            {
                diceRollScript.isLanded = true;
                diceRollScript.diceFaceNum = sideCollider.gameObject.name; // Ensure the dice face is correctly assigned
            }
            else
            {
                diceRollScript.isLanded = false;
            }
        }
    }
}
