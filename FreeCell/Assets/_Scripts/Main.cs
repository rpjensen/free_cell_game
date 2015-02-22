using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Location {
	Tableau,
	Foundation,
	FreeCell,
	None
}

public class Main : MonoBehaviour {

	public GameObject[] goTableaus;
	public GameObject[] goFoundations;
	public GameObject[] goFreeCells;
	public GameObject goDeck;


	public bool _____________________;

	public List<Tableau> tableaus;
	public List<Foundation> foundations;
	public List<FreeCell> freeCells;
	public Deck deck;

	private Location _mouseLocation;
	private int _mouseIndex;

	private Card _selectedCard;
	private Location _selectedLocation;
	private int _selectedIndex;

	void Awake() {

	}

	// Use this for initialization
	void Start () {
		// init the three lists and deck
		tableaus = new Tableau[goTableaus.Length];
		foundations = new Foundation[goFoundations.Length];
		freeCells = new FreeCell[goFreeCells.Length];
		deck = goDeck.GetComponent<Deck> ();

		// get the script objects for convenience
		foreach (GameObject go in goTableaus) {
			tableaus.Add(go.GetComponent<Tableau>());
		}
		foreach (GameObject go in goFoundations) {
			foundations.Add(go.GetComponent<Foundation>());
		}
		foreach (GameObject go in goFreeCells) {
			freeCells.Add(go.GetComponent<FreeCell>());
		}

		// triple shuffle the deck
		deck = deck.Shuffle ();
		deck = deck.Shuffle ();
		deck = deck.Shuffle ();

		// deal the cards
		int col = 0;
		while (deck.cards.Count > 0) {
			Card card = deck.DrawCard();
			tableaus[col].AddCard(card);
			col = (col + 1) % tableaus.Count;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp(0)) {
			MouseClickDispatcher();
		}
	}

	private Location mouseLocation {
		get {
			Location location = Location.None;// init location
			int index = 0;// init index
			// Foreach foundation, check if the card is in bounds
			foreach (Foundation foundation in foundations) {
				if (foundation.mouseInBounds) {
					_mouseLocation = Location.Foundation;
					_mouseIndex = index;
					return _mouseLocation;
				}
				index++;
			}

			index = 0;// init index
			// Foreach tableau check if the mouse is in bounds
			foreach (Tableau tableau in tableaus) {
				if (tableau.mouseInBounds) {
					_mouseLocation = Location.Tableau;
					_mouseIndex = index;
					return _mouseLocation;
				}
				index++;
			}

			index = 0;
			// Foreach freecell check if the mouse is in bounds
			foreach (FreeCell freeCell in freeCells) {
				if (freeCell.mouseInBounds) {
					_mouseLocation = Location.FreeCell;
					_mouseIndex = index;
					return _mouseLocation;
				}
				index++;
			}

			// The mouse isn't over anything I care about so return non
			_mouseLocation = Location.None;
			_mouseIndex = -1;

			return _mouseLocation;
		}
	}

	void MouseClickDispatcher() {
		// Get the mouse location
		Location location = mouseLocation;

		// If there is no previously selected card
		if (_selectedCard == null) {

			switch (location) {
			case Location.None:
				return;
			case Location.Foundation:
				// Select the top card in the foundation
				_selectedCard = foundations[_mouseIndex].GetTopCard();
				break;
			case Location.Tableau:
				// Select the top card in the tableau
				_selectedCard = tableaus[_mouseIndex].GetTopCard();
				break;
			case Location.FreeCell:
				// Select the only card in the free cell
				_selectedCard = freeCells[_mouseIndex].PeakCard();
				break;
			}
			// Record the location/index of the selected card
			_selectedIndex = _mouseIndex;
			_selectedLocation = location;
			_selectedCard.selected = true;
			return;
		}

		// We have a selected card so now check if someone is trying to make a move
		bool didMove = false;
		switch (location) {
		case Location.None:
			return;
		case Location.Tableau:
			// If the move is valid the move it and set the flag
			if (tableaus[_mouseIndex].IsValidMove(_selectedCard)) {
				tableaus[_mouseIndex].AddCard(_selectedCard);
				didMove = true;
			}
			break;
			// If the move is valid move it and set the flag
		case Location.Foundation:
			if (foundations[_mouseIndex].IsValidMove(_selectedCard)) {
				foundations[_mouseIndex].AddCard(_selectedCard);
				didMove = true;
			}
			break;
			// If the move is valid move it and set the flag
		case Location.FreeCell:
			if (freeCells[_mouseIndex].validMove) {
				freeCells[_mouseIndex].AddCard(_selectedCard);
				didMove = true;
			}
		}

		if (didMove) {
			// We have moved the card to the new location so remove it from the old location
			switch (_selectedLocation) {
			case Location.None:
				print ("Selected location doesn't match selected card != null");
				return;
			case Location.Foundation:
				foundations[_selectedIndex].RemoveTopCard();
				break;
			case Location.Tableau:
				tableaus[_selectedIndex].RemoveTopCard();
				break;
			case Location.FreeCell:
				freeCells[_selectedIndex].RemoveCard();
				break;
			}
		}
		else {
			PrintErrorMessage();
		}
		// Unselect the card
		_selectedCard.selected = false;
		_selectedCard = null;
		_selectedIndex = -1;
		_selectedLocation = Location.None;
	}

	void PrintErrorMessage() {
		print ("Not a valid move");
	}
}
