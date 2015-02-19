using UnityEngine;
using System.Collections;

public enum Suit {
	Club = 1,
	Diamond,
	Spade,
	Heart
}

public enum CardColor {
	Black,
	Red
}

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
	

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
	
	}


	//Method that checks if the card color is black
	public CardColor whichColor(){
		int suitInt = (int)this.theSuit;
		if (suitInt % 2 != 0) return CardColor.Black;
		return CardColor.Red;
	}

}
