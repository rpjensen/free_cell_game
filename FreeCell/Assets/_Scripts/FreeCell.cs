using UnityEngine;
using System.Collections;

// Ryan Jensen
// CSCI 373 Game Programming : Free Cell
// This class represents the script object for the free cell.  It holds a single card at a time
// February 27, 2014
public class FreeCell : MonoBehaviour {

	public Card card;
	[SerializeField]
	private bool _mouseInBounds;

	// property that determines if the mouse is in the objects bounds
	public bool mouseInBounds {
		get {
			if (card == null)
				return _mouseInBounds;
			return card.mouseInBounds;
		}
	}

	// property that determines if a card can be added
	public bool validMove {
		get {
			return (this.card == null);
		}
	}

	// get a reference to the card without removing it
	public Card PeekCard() {
		return this.card;
	}

	// remove the card and return a reference
	public Card RemoveCard() {
		Card temp = card;
		card = null;
		return temp;
	}

	// add the new card and move its transform to this and change its local position
	public bool AddCard(Card card) {
		if (this.card != null) { return false; }
		this.card = card;
		card.gameObject.transform.parent = this.gameObject.transform;
		Vector3 pos = Vector3.zero;
		pos.z = -5;
		card.gameObject.transform.localPosition = pos;
		return true;
	}

	// Set mouse in bounds on mouse enter
	void OnMouseEnter() {
		_mouseInBounds = true;
	}

	// Set mouse in bounds on mouse exit
	void OnMouseExit() {
		_mouseInBounds = false;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
