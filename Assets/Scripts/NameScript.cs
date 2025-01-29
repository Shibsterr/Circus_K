using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NameScript : MonoBehaviour
{
    TextMeshPro tmp;


    
    void Awake()
    {
        tmp = transform.Find("NameField").gameObject.GetComponent<TMPro.TextMeshPro>();
     }

    public void SetPlayerName(string name)
    {
        tmp.text = name;
        tmp.color = new Color32((byte)Random.Range(0,255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);
    }
}
