using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    // public variables to store card characteristics
    public bool top = false;        // indicate if top card or not
    public string suit;             // store suit
    public int value;               // store numeric value
    public int row;                 // row 
    public bool faceUp = false;     // chck if facing up
    public bool inDeckPile = false; // chck if in deck pile

    private string valueString;     // temp storage of card value

    // Start is called before the first frame update
    void Start()
    {
        // if gameObject is card
        if (CompareTag("Card"))
        {
            // extract suit from first char of the cards name
            suit = transform.name[0].ToString();

            // extract remaining value of cards name
            for (int i = 1; i < transform.name.Length; i++)
            {
                char c = transform.name[i];
                valueString = valueString + c.ToString();
            }

            // convert value string to numerical value
            if (valueString == "A") { value = 1; }
            if (valueString == "2") { value = 2; }
            if (valueString == "3") { value = 3; }
            if (valueString == "4") { value = 4; }
            if (valueString == "5") { value = 5; }
            if (valueString == "6") { value = 6; }
            if (valueString == "7") { value = 7; }
            if (valueString == "8") { value = 8; }
            if (valueString == "9") { value = 9; }
            if (valueString == "10") { value = 10; }
            if (valueString == "J") { value = 11; }
            if (valueString == "Q") { value = 12; }
            if (valueString == "K") { value = 13; }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
