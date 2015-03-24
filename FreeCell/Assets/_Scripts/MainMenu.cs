using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		// make a HighScore database if there isn't one already
		HighScore.db_CheckedCreateDatabase ();
	}

}
