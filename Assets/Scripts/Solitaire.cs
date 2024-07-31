using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Solitaire : MonoBehaviour
{
    // public prefabs for card faces, prefabs, deckbuttons and positions
    public Sprite[] cardFaces;
    public GameObject cardPrefab;
    public GameObject deckButton;
    public GameObject[] bottomPos;
    public GameObject[] topPos;

    // static arrays for suits and values of the cards
    public static string[] suits = new string[] { "C", "D", "H", "S" };
    public static string[] values = new string[] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
    
    // lists for bottom and top positions, trips, and deck
    public List<string>[] bottoms;  // bottom 7 cards
    public List<string>[] tops;     // top 4 cards
    public List<string> tripsOnDisplay = new List<string>();        // trips to cycle back into the deck
    public List<List<string>> deckTrips = new List<List<string>>(); // trips...??

    // lists for each of the 7 bottom positions
    private List<string> bottom0 = new List<string>();
    private List<string> bottom1 = new List<string>();
    private List<string> bottom2 = new List<string>();
    private List<string> bottom3 = new List<string>();
    private List<string> bottom4 = new List<string>();
    private List<string> bottom5 = new List<string>();
    private List<string> bottom6 = new List<string>();

    // lists for deck and discard pile
    public List<string> deck;
    public List<string> discardPile = new List<string>();

    // variables for deck management
    private int deckLocation;
    private int trips;
    private int tripsRemainder;


    // Start is called before the first frame update
    void Start()
    {
        bottoms = new List<string>[] { bottom0, bottom1, bottom2, bottom3, bottom4, bottom5, bottom6 };
        PlayCards();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // PlayCards method sets up the game by shuffling and dealing cards 
    public void PlayCards()
    {
        // clear bottom piles, generate, and shuffle new deck
        foreach (List<string> list in bottoms)
        {
            list.Clear();
        }

        deck = GenerateDeck();
        Shuffle(deck);

        //test the cards in the deck:
        foreach (string card in deck)
        {
            print(card);
        }

        // call these methods to organize deck into solitaire layout and deal cards
        SolitaireSort();
        StartCoroutine(SolitaireDeal()); // coroutine is the 
        SortDeckIntoTrips();

    }

    // generate new deck of cards
    public static List<string> GenerateDeck()
    {
        List<string> newDeck = new List<string>();
        foreach (string s in suits)         // club, hearts, diamonds, spades
        {
            foreach (string v in values)    // A-K
            {
                newDeck.Add(s + v);
            }
        }
        return newDeck;
    }

    // shuffle any list using fisher-yates shuffle (algorithm for shuffling a finite sequence to an unbias permutation)
    void Shuffle<T>(List<T> list)
    {
        System.Random random = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            int k = random.Next(n);
            n--;
            T temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }
    }

    // coroutine for dealing cards to bottom piles, stack bottom to top
    IEnumerator SolitaireDeal()
    {
        for (int i = 0; i < 7; i++)
        {
            // this is for card positioning
            float yOffset = 0;
            float zOffset = 0.03f;
            foreach (string card in bottoms[i])
            {
                // delay and instantiate cards at specific positions
                yield return new WaitForSeconds(0.05f);
                GameObject newCard = Instantiate(cardPrefab, new Vector3(bottomPos[i].transform.position.x, bottomPos[i].transform.position.y - yOffset, bottomPos[i].transform.position.z - zOffset), Quaternion.identity, bottomPos[i].transform);
                newCard.name = card;
                newCard.GetComponent<Selectable>().row = i;
                if (card == bottoms[i][bottoms[i].Count -1])
                {
                    newCard.GetComponent<Selectable>().faceUp = true;
                }
                
                // update yOffset and zOffset for next card
                yOffset = yOffset + 0.3f;
                zOffset = zOffset + 0.03f;
                discardPile.Add(card);
            }
        }

        // remove dealt cards from deck
        foreach (string card in discardPile)
        {
            if (deck.Contains(card))
            {
                deck.Remove(card);
            }
        }
        discardPile.Clear();

    }

    // solitaire sort - sorts deck into the initial solitaire layout
    void SolitaireSort()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = i; j < 7; j++)
            {
                bottoms[j].Add(deck.Last<string>());
                deck.RemoveAt(deck.Count - 1);
            }

        }

    }

    // sort deck into trips (groups of 3)
    public void SortDeckIntoTrips()
    {
        trips = deck.Count / 3;
        tripsRemainder = deck.Count % 3;
        deckTrips.Clear();

        // create trips
        int modifier = 0;
        for (int i = 0; i < trips; i++)
        {
            List<string> myTrips = new List<string>();
            for (int j = 0; j < 3; j++)
            {
                myTrips.Add(deck[j + modifier]);
            }
            deckTrips.Add(myTrips);
            modifier = modifier + 3;
        }

        // remaining cards that don't fit into trips
        if (tripsRemainder != 0)
        {
            List<string> myRemainders = new List<string>();
            modifier = 0;
            for (int k = 0; k < tripsRemainder; k++)
            {
                myRemainders.Add(deck[deck.Count - tripsRemainder + modifier]);
                modifier++;
            }
            deckTrips.Add(myRemainders);
            trips++;
        }
        deckLocation = 0;

    }

    // deal from deck 
    public void DealFromDeck()
    {
        // add remaining cards to discard pile
        foreach (Transform child in deckButton.transform)
        {
            if (child.CompareTag("Card"))
            {
                deck.Remove(child.name);
                discardPile.Add(child.name);
                Destroy(child.gameObject);
            }
        }

        // draw 3 new cards or restack the top deck
        if (deckLocation < trips)
        {
            tripsOnDisplay.Clear();
            float xOffset = 2.5f;
            float zOffset = -0.2f;

            foreach (string card in deckTrips[deckLocation])
            {
                GameObject newTopCard = Instantiate(cardPrefab, new Vector3(deckButton.transform.position.x + xOffset, 
                    deckButton.transform.position.y, deckButton.transform.position.z + zOffset), Quaternion.identity, 
                    deckButton.transform);
                xOffset = xOffset + 0.5f;
                zOffset = zOffset - 0.2f;
                newTopCard.name = card;
                tripsOnDisplay.Add(card);
                newTopCard.GetComponent<Selectable>().faceUp = true;
                newTopCard.GetComponent<Selectable>().inDeckPile = true;
            }
            deckLocation++;
        }
        else
        {
            //restack the top deck
            RestackTopDeck();
        }
    }

    // moves cards from discard pile back to deck
    void RestackTopDeck()
    {
        deck.Clear();
        foreach (string card in discardPile)
        {
            deck.Add(card);
        }
        discardPile.Clear();
        SortDeckIntoTrips();
    }
}
