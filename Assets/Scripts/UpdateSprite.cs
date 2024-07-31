using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSprite : MonoBehaviour
{
    // public variables for the card's face and back sprites
    public Sprite cardFace;
    public Sprite cardBack;
    private SpriteRenderer spriteRenderer;  // component that renders the sprite
    private Selectable selectable;          // ref to the Selectable script
    private Solitaire solitaire;            // ref to teh Solitaire script
    private UserInput userInput;            // ref to the UserInput script

    // Start is called before the first frame update
    void Start()
    {
        // generate a deck and find game object
        List<string> deck = Solitaire.GenerateDeck();
        solitaire = FindObjectOfType<Solitaire>();
        userInput = FindObjectOfType<UserInput>();

        // loop deck and assign correct sprite
        int i = 0;
        foreach (string card in deck)
        {
            if (this.name == card)
            {
                cardFace = solitaire.cardFaces[i];
                break;
            }
            i++;
        }
        // get SpriteRenderer and Selectable components 
        spriteRenderer = GetComponent<SpriteRenderer>();
        selectable = GetComponent<Selectable>();
    }

    // Update is called once per frame
    void Update()
    {
        // update cards sprite based on whether faced up or down
        if (selectable.faceUp == true)
        {
            spriteRenderer.sprite = cardFace;
        }
        else
        {
            spriteRenderer.sprite = cardBack;
        }

        // highlight the card if it's the selected card in slot1
        if (userInput.slot1)
        {

            if (name == userInput.slot1.name)
            {
                spriteRenderer.color = Color.yellow;    // highlight color
            }
            else
            {
                spriteRenderer.color = Color.white;     // default color
            }
        }
    }
}
