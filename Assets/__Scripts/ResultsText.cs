using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsText : MonoBehaviour
{
    public Text rText;
    public int finalScore;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //retrieve score from PlayerPrefs
        finalScore = PlayerPrefs.GetInt("PlayerScore", 0);

        //find text component and assign the reference to rText
        rText = GetComponent<Text>();

        //construct game over text
        rText.text = "Your body has been lost to space...\n" + "FINAL PAYOUT: $" + finalScore.ToString("#,0");
    }

}
