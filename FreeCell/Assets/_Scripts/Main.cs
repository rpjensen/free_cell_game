using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum Location {
	Tableau,
	Foundation,
	FreeCell,
	None
}



// Ryan Jensen
// CSCI 373 Game Programming : Free Cell
// This class represents the core game logic which includes moving cards, undoing cards, and updating the ui elements
// February 27, 2014
public class Main : MonoBehaviour {

	public GameObject[] goTableaus;
	public GameObject[] goFoundations;
	public GameObject[] goFreeCells;
	public GameObject goDeck;
	public GameObject goScore;
	public GameObject goTimer;
	public GameObject goResetButton;
	public GameObject goUndoButton;
	public GameObject goWinnerLabel;
	public GameObject goValidMove;
	public static bool gameStarted = false;

	public bool _____________________;

	public List<Tableau> tableaus;
	public List<Foundation> foundations;
	public List<FreeCell> freeCells;
	public Deck deck;
	public Text scoreLabel;
	public int score;
	public Text timer;

	// Undo stack
	private Stack<UndoEntry> _undoManager;

	// Holds a reference to the mouse click location and index (valid for a single update cycle)
	private Location _mouseLocation;
	private int _mouseIndex;

	// Holds a reference to the currently selected card and its location/index
	private Card _selectedCard;
	private Location _selectedLocation;
	private int _selectedIndex;
	private float _initTime;
	private int _initDeckCount;

	// Init the internal resources
	void Awake() {
		_undoManager = new Stack<UndoEntry> ();
		tableaus = new List<Tableau>();
		foundations = new List<Foundation>();
		freeCells = new List<FreeCell>();
	}

	// Use this for initialization
	void Start () {
		// init the deck
		deck = goDeck.GetComponent<Deck> ();
		_initDeckCount = deck.cards.Count;

		// init the score label
		scoreLabel = goScore.GetComponent<Text> ();
		scoreLabel.text = "Score: 0";

		// init the timer
		timer = goTimer.GetComponent<Text> ();
		_initTime = Time.realtimeSinceStartup;

		// init the undo button
		Button undoButton = goUndoButton.GetComponent<Button> ();
		undoButton.onClick.AddListener (() => UndoMove ());

		// init the reset button
		Button resetButton = goResetButton.GetComponent<Button> ();
		resetButton.onClick.AddListener(() => GameOver());

		goWinnerLabel.SetActive (false);
		goValidMove.SetActive (false);

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

		// thourougly shuffle the deck
		deck.Shuffle ();
		deck.Shuffle ();
		deck.Shuffle ();
		deck.Shuffle ();
		deck.Shuffle ();

		// deal the cards until they are all gone (add them to columns left to right)
		int col = 0;
		while (deck.cards.Count > 0) {
			Card card = deck.DrawCard();
			tableaus[col].AddCard(card);
			// keep wrapping the column around
			col = (col + 1) % tableaus.Count;
			// used this instead of the above code to test moving from and to empty tableaus
			// col = (col + 1) % (tableaus.Count-1);
		}
		Main.gameStarted = true;
	}
	
	// Update is called once per frame
	void Update () {
		// Get the elapsed game time
		float deltaT = Time.realtimeSinceStartup - _initTime;
		// cast to int
		int time = Mathf.FloorToInt (deltaT);
		int secs = time % 60;// get secs
		string sSecs = secs < 10 ? "0"+secs : secs.ToString();// add second zero if needed
		int mins = (time / 60) % 60;// get mins
		string sMins = mins < 10 ? "0"+mins : mins.ToString();// add second zero if needed
		int hours = time / 3600;// get hours
		// Update timer
		timer.text = "Time: " + hours+":"+sMins+":"+sSecs;

		// If the mouse was clicked, dispatch to the appropriate function
		if (Input.GetMouseButtonUp(0)) {
			MouseClickDispatcher();
		}
	}

	// Determines what action needs to be taken after a mouse click based on the context of the current frame
	void MouseClickDispatcher() {
		// Get the current mouse location (and set _mouseLocation/_mouseIndex for this frame)
		Location location = mouseLocation;

		// If there is no previously selected card
		if (_selectedCard == null) {

			//get the card at the mouse's current location
			_selectedCard = GetCardAtLocation(location, _mouseIndex);
			if (_selectedCard != null) {
				//avoid a null pointer exception
				_selectedCard.selected = true;
			}
			// Record the location/index of the selected card for later use (can be location.none)

			_selectedIndex = _mouseIndex;
			_selectedLocation = location;

			/* debugging 
			if (_selectedCard != null) {
				print("Location Info: " + _mouseLocation + " " + _mouseIndex);
				print("Card Info: " + _selectedCard.theSuit + " " + _selectedCard.value + " " + _selectedCard.WhichColor());
				print("");
			} */

			return;


		}
		// else we have a selected card and are now clicking somewhere else

		// If we are clicking the same card that was selected then deselect it
		if (_mouseIndex == _selectedIndex && location.Equals(_selectedLocation) || (mouseLocation == Location.None)) {
			_selectedCard.selected = false;
			_selectedCard = null;
			_selectedLocation = Location.None;
			_selectedIndex = -1;
			return;
		}


		// We have a selected card so now check if someone is trying to make a move
		bool didMove = MoveCardToLocation(_selectedCard, location, _mouseIndex);// records whether the move was made
		
		if (didMove) {
			// We have moved the card to the new location so remove it from the old location
			RemoveCardAtLocation(_selectedLocation, _selectedIndex);
			// Update the score
			UpdateScore();
			// Add the entry to the undo stack

			_undoManager.Push(new UndoEntry(_selectedLocation, _selectedIndex, _mouseLocation, _mouseIndex));

			// If it was moved un select the card
			_selectedCard.selected = false;
			_selectedCard = null;
			_selectedIndex = -1;
			_selectedLocation = Location.None;

			if (GetRemainingCards() <= 0) {
				goWinnerLabel.SetActive(true);
				Invoke("GameOver", 5);
			}
		}
		else {
			// If they didn't click in a valid move area don't clear the selected card
			if (location.Equals(Location.None)) { return; }
			EnableBadMove();

		}
		
	}

	// Undo the last move
	void UndoMove() {
		if (this.canUndo) {
			UndoEntry entry = _undoManager.Pop ();
			Card card = RemoveCardAtLocation (entry.toLocation, entry.toIndex);
			MoveCardToLocation(card, entry.fromLocation, entry.fromIndex, true);
			
			UpdateScore ();
		}
	}

	// Update the score and label
	void UpdateScore() {
		score++;
		print ("Score updated: " + score);
		scoreLabel.text = "Score: " + score;
	}

	void EnableBadMove() {
		goValidMove.SetActive (true);
		Invoke ("DisableBadMove", 2);
	}

	void DisableBadMove() {
		goValidMove.SetActive (false);
	}

	// Return the number of cards left in the game
	public int GetRemainingCards() {
		int count = 0;

		foreach (Foundation foundation in this.foundations) {
			count += foundation.GetCardCount();
		}
		return _initDeckCount - count;
	}

	// Get the card at a given location and index (or null)
	private Card GetCardAtLocation(Location location, int index) {
		Card card = null;
		switch (location) {
		case Location.None:
			break;
		case Location.Foundation:
			// Select the top card in the foundation
			card = foundations[index].GetTopCard();
			break;
		case Location.Tableau:
			// Select the top card in the tableau
			card = tableaus[index].GetTopCard();
			break;
		case Location.FreeCell:
			// Select the only card in the free cell
			card = freeCells[index].PeekCard();
			break;
		}
		return card;
	}

	// Remove the top card at a given location 
	private Card RemoveCardAtLocation(Location location, int index) {
		Card card = null;
		switch (location) {
		case Location.None:
			break;
		case Location.Foundation:
			card = foundations[index].RemoveTopCard();
			break;
		case Location.Tableau:
			card = tableaus[index].RemoveTopCard();
			break;
		case Location.FreeCell:
			card = freeCells[index].RemoveCard();
			break;
		}
		return card;
	}

	// Move a card to the given location
	private bool MoveCardToLocation(Card card, Location location, int index) {
		return MoveCardToLocation (card, location, index, false);
	}

	// Move the card to the given location and choose whether to force add it or respect the game rules
	private bool MoveCardToLocation(Card card, Location location, int index, bool forced) {
		bool didMove = false;
		switch (location) {
		case Location.None:
				didMove = false;
				break;
		case Location.Tableau:
			// If the move is valid (or forced) then move it and set the flag
			if (forced || tableaus[index].IsValidMove(card)) {
				tableaus[index].AddCard(card);
				didMove = true;
			}
			break;
			// If the move is valid (or forced) then move it and set the flag
		case Location.Foundation:
			if (forced || foundations[index].IsValidMove(card)) {
				foundations[index].AddCard(card);
				didMove = true;
			}
			break;
			// If the move is valid (or forced) move it and set the flag
		case Location.FreeCell:
			if (forced || freeCells[index].validMove) {
				freeCells[index].AddCard(card);
				didMove = true;
			}
			break;
		}
		return didMove;
	}

	// Set and get the mouseLocation from the last frame
	private Location mouseLocation {
		get {
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

	// get a boolean whether there is a move that can be undone
	public bool canUndo {
		get {
			return _undoManager.Count > 0;
		}
	}

	// Reload the level
	void GameOver() {
		// Invoke high scores here
		// 
		Application.LoadLevel ("_HighScores");
	}
	
	// calls to this are made by the buttons
	public void LoadMenu(int menuIndex) {
		if (menuIndex == 0)
			Application.LoadLevel ("_MainMenu"); 
	}


	// Represents one entry in the undo stack (it has two locations from and to)
	private class UndoEntry {
		private Location _fromLocation;
		private int _fromIndex;
		private Location _toLocation;
		private int _toIndex;

		public Location fromLocation {
			get {
				return _fromLocation;
			}
		}

		public int fromIndex {
			get {
				return _fromIndex;
			}
		}

		public Location toLocation {
			get {
				return _toLocation;
			}
		}

		public int toIndex {
			get {
				return _toIndex;
			}
		}

		public UndoEntry(Location fromLocation, int fromIndex, Location toLocation, int toIndex) {
			this._fromLocation = fromLocation;
			this._fromIndex = fromIndex;
			this._toLocation = toLocation;
			this._toIndex = toIndex;
		}
	}

}
