using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Deck : MonoBehaviour {
	// set in Unity inspector
	public List<Sprite> cardSprites;
	public List<GameObject> cards;
	public GameObject cardPrefab;
	public bool ____________________________________;
	//Set dynamically

	// Use this for initialization
	void Start () {
		cards = new List<GameObject>();
			int i = 0; 
			int currentSuit = 0;
			for (int j=1; j<5; j++){ //loop for each suit
				currentSuit++;//adds 1 to the suit number
				for (int k=1; k<14; k++){//for each card in the suit
					GameObject tempCard = Instantiate (cardPrefab) as GameObject; //creates the card
					tempCard.GetComponent<Card>().theSuit = (Suit)currentSuit; //sets suit to the current suit
					tempCard.GetComponent<Card>().value = k; // sets value for the card
					tempCard.GetComponentInChildren<SpriteRenderer>().sprite = cardSprites[i]; //sets sprite of the card
				i++;
				cards.Add(tempCard); //adds card to the deck
				}
			}
		cards = Shuffle (cards);


				
	}

	//method to draw a card
	public Card DrawCard(){
		GameObject tempCard = cards[cards.Count-1]; //gets card at the top of the deck
		cards.RemoveAt (cards.Count-1); //removes the top card from the deck
		return tempCard.GetComponent<Card> (); //returns the card
		}
	

	//shuffle the deck
	public List<GameObject> Shuffle(List<GameObject> originalDeck){
		List<GameObject> originList = originalDeck; //sets originList to the passed list
		List<GameObject> tempCardList = new List<GameObject>(); //creates a second list
		while (originList.Count > 0) { //runs as long as the original deck still has cards
			GameObject tempCard = originList [Random.Range (0, originList.Count)]; //randomly chooses one card from the original list
			tempCardList.Add (tempCard); //adds that card to the new list
			originList.Remove (tempCard); //removes that card from the original list
				}
		return tempCardList; //returns the new shuffled list

	}

	public void Shuffle() {
		cards = Shuffle (cards);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
