using UnityEngine;
using System.Collections;

public class Tableau : MonoBehaviour {

	// This Card Stack will hold all the cards
	// index 0 will be the bottom most card
	// index cardStack.Count-1 will be the top-most upper
	// facing card.
	private ArrayList cardStack;

	// Use this for initialization
	void Start () {
		cardStack = new ArrayList ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// You will use this without doing an IsValidMove 
	// only when initializng it (or undoing I assume
	// if we can implement that)
	public void AddCard (Card card) {
		cardStack.Add (card);
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
		return returnMe;
	}

	// Allows you to see the top card without removing it.
	public Card GetTopCard () {
		cardStack.TrimToSize ();
		if (cardStack.Count == 0) {
			return null;
		}
		return (Card)cardStack [cardStack.Count - 1];
	}

	// IsValidMove
	// Using the rules from freecell-cardgame.com
	/*
	 *  You can move the top card of a pile on the Tableau onto another
	 *  Tableau pile, if that pile's top card is one higher than the
	 *  moved card and in a different color. For example, you could
	 *  move a red 6 onto a black 7. If you have an empty Tableau pile
	 *  then you can move any card there.
	 */
	public bool IsValidMove (Card card) {
		cardStack.TrimToSize ();
		// if you have an empty stack, you can place a card there. 
		if (cardStack.Count == 0) {
			return true;
		}
		// now get the top card to use as a comparator.
		Card topCard = (Card)cardStack [cardStack.Count - 1];
		// if the two colors are not the same and the top
		// card has a value of card.value+1, then it is 
		// a valid move!
		if ( (topCard.WhichColor() != card.WhichColor())
		  && (topCard.value == (card.value+1))) {
			return true;
		}
		return false;
	}

}
