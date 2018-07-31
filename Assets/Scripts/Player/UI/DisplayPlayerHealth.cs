using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPlayerHealth : MonoBehaviour
{
    public Text playerHealthText;


    // Update is called once per frame
    void Update()
    {
        playerHealthText.text = "Health: " + PlayerData.damageTaken;
    }
}
