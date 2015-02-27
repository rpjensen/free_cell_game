using UnityEngine;
using System.Collections;

public class FreeCell : MonoBehaviour {

	public Card card;
	[SerializeField]
	private bool _mouseInBounds;

	public bool mouseInBounds {
		get {
			return _mouseInBounds;
		}
	}

	public bool validMove {
		get {
			return this.card != null;
		}
	}

	public Card PeakCard() {
		return this.card;
	}

	public Card RemoveCard() {
		Card temp = card;
		card = null;
		return temp;
	}

	public bool AddCard(Card card) {
		if (this.card != null) { return false; }
		this.card = card;
		card.transform.parent = this.gameObject;
		card.transform.localPosition = Vector3.zero;
		return true;
	}

	void OnMouseEnter() {
		_mouseInBounds = true;
	}

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
