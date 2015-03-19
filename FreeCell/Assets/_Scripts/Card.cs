using UnityEngine;
using System.Collections;

public enum Suit {
	Club = 1,
	Diamond,
	Heart,
	Spade
}

public enum CardColor {
	Black,
	Red
}


// Joseph Tamberino
// CSCI 373 Game Programming : Free Cell
// This class represents the logic for each individual card.
// March 17, 2015

public class Card : MonoBehaviour {
	[SerializeField]
	Suit i_theSuit;
	[SerializeField]
	int i_value;
	public Suit theSuit {
		get {
			return i_theSuit;
		} set{
			i_theSuit = value;}
		}
	public int value { 
		get{
			return i_value;
				}
		set{
			i_value = value;	}
	}
	public bool mouseInBounds {get; set;}
	private bool i_selected;


	void Awake(){
		selected = false;
	}

	// Update is called once per frame
	void Update () {
	
	}


	//Method that checks if the card color is black
	public CardColor WhichColor(){
		if (this.theSuit == Suit.Club || this.theSuit == Suit.Spade)
			return CardColor.Black;
		return CardColor.Red;
	}

	void OnMouseEnter() {
		mouseInBounds = true;
	}
	void OnMouseExit(){
		mouseInBounds = false;
	}


	public bool selected {
				get {
			return i_selected;
				}
				set {
			i_selected = value;
			Component halo = gameObject.GetComponent("Halo");

			if (i_selected){
				halo.GetType().GetProperty("enabled").SetValue(halo, true, null);
			}
			else {
				halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
			}
		}
	}


}
