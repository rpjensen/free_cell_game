using UnityEngine;
using System.Collections;

public class Foundation : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool mouseInBounds {
		get {
			return true;
		}
	}

	public Card GetTopCard () {
		return new Card();
	}

	public Card RemoveTopCard() {
		return new Card ();
	}

	public bool IsValidMove (Card card) {
		return true;
	}

	public void AddCard(Card card) {

	}

	public int GetCardCount() {
		return 0;
	}
}
