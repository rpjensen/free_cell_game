using UnityEngine;
using System.Collections;

public class Foundation : MonoBehaviour {

	// This Card Stack will hold all the cards
	// index 0 will be the bottom most card
	// index cardStack.Count-1 will be the top-most upper
	// facing card.
	private ArrayList cardStack;

	// Which suit of ACE will be placed here first?
	public Suit suit;

	// Use this for initialization
	void Awake () {
		cardStack = new ArrayList ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool mouseInBounds {
		get {
			return true;
		}
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
	// Using the rules from freecell-cardgame.com
	/*
	 *  When the Free Cells are empty and all cards
	 *	on the Tableau are arranged in 4 piles and each of
	 *	the piles has been ordered in descending order with
	 *	alternating red/black cards then the Tableau will
	 *	clear itself, since at that point you are guaranteed
	 *	to win the game.
	 */
	public bool IsValidMove (Card card) {
		int ACE_VALUE = 1;  
		int MAX_CARD_NUMBER = 13;
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
		if ((card.value == MAX_CARD_NUMBER - cardStack.Count) && (card.WhichColor() != topCard.WhichColor()))
			return true;

		return false;
	}

	// You will use this without doing an IsValidMove 
	// only when initializng it (or undoing I assume
	// if we can implement that)
	public void AddCard (Card card) {
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
		// and then move each card after it away from the camera by 0.5 (+Z)
		Vector3 position = Vector3.zero;
		cardStack.TrimToSize();
		for (int i=0; i<cardStack.Count; i++) {
			((Card)cardStack[i]).gameObject.transform.localPosition = position;
			position.z += 0.5f;
		}
	}
}

// Joseph El-Khouri
