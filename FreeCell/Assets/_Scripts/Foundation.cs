using UnityEngine;
using System.Collections;

public class Foundation : MonoBehaviour {

	// This Card Stack will hold all the cards
	// index 0 will be the bottom most card
	// index cardStack.Count-1 will be the top-most upper
	// facing card.
	private ArrayList cardStack;
	private bool mouseOverBase;
	 
	// Which suit of ACE will be placed here first?
	public Suit suit;

	// Use this for initialization
	void Awake () {
		cardStack = new ArrayList ();
		mouseOverBase = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool mouseInBounds {
		get {
			cardStack.TrimToSize();
			// if we have cards...
            // use the top card's mouseinbounds property
			if (cardStack.Count != 0)
				return ((Card)cardStack[cardStack.Count-1]).mouseInBounds;
			// otherwise, use the tracking bool for the base area
			return mouseOverBase;
		}
	}

	void OnMouseEnter() {
		mouseOverBase = true;
	}
	void OnMouseExit(){
		mouseOverBase = false;
	}
	
	// Allows you to see the top card without removing it.
	public Card GetTopCard () {
		cardStack.TrimToSize ();
		if (cardStack.Count == 0) {
			return null;
		}
		return (Card)cardStack [cardStack.Count - 1];
	}

	// This will remove the top upper facing card and
	// return it.
	public Card RemoveTopCard () {
		cardStack.TrimToSize ();
		if (cardStack.Count == 0) {
			return null;
		}
		Card returnMe = (Card)cardStack [cardStack.Count - 1];
		cardStack.RemoveAt (cardStack.Count - 1);
		LayerCalculate ();
		return returnMe;
	}

	// IsValidMove
	// 
	/*
	 *  You stack them in order
	 *  eg. A 2 of spades goes on an Ace of Spades
	 *      a 6 of diamonds goes on a 5 of diamonds
	 */
	public bool IsValidMove (Card card) {
		int ACE_VALUE = 1;  
		// 11 = J
		// 12 = Q
		// 13 = K

		cardStack.TrimToSize ();
		if (cardStack.Count == 0) {
			// If we have an empty foundation,
			// make sure the first card is an ACE and matches
			// the public suit variable.
			if ((card.value == ACE_VALUE) && (card.theSuit == suit))
				return true;
			return false; // otherwise, bail outta this method.
		}
		// continue if we at least have an ace on here...

		Card topCard = (Card)cardStack [cardStack.Count - 1];
		if ((card.value == topCard.value + 1) && (card.theSuit == topCard.theSuit))
			return true;
		return false;
	}

	// You will use this without doing an IsValidMove 
	// only when initializng it (or undoing I assume
	// if we can implement that)
	public void AddCard (Card card) {
		card.gameObject.transform.parent = this.gameObject.transform;
		cardStack.Add (card);
		LayerCalculate ();
	}

	public int GetCardCount() {
		cardStack.TrimToSize ();
		return cardStack.Count;
	}

	// This positions all the cards directly under one another.
	public void LayerCalculate () {
		// go through every card, starting at the top
		// put the local transform at 0,0,0 for the first card
		// and then move each card after it towards the camera by 0.1 (-Z)
		Vector3 position = Vector3.zero;
		cardStack.TrimToSize();
		for (int i=0; i<cardStack.Count; i++) {
			((Card)cardStack[i]).gameObject.transform.localPosition = position;
			position.z -= 0.1f;
		}
	}


}

// Joseph El-Khouri
