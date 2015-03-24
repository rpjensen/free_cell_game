using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class HighScoreVisualizer : MonoBehaviour {

	public Text text_Name;
	public Text text_Moves;
	public Text text_Time;

	// Use this for initialization
	void Awake () {
		UpdateTextGraphics ();
	}

	void UpdateTextGraphics() {
		string names = "Name\n";
		string moves = "Moves\n";
		string time  = "Time\n";

		List<string> l = HighScore.db_getAsList ();
		for (int i=0; i<HighScore.NUM_HS; i++) {
			names = names + HighScore.db_e_getParamater(l[i], "NAME") + "\n";
			moves = moves + HighScore.db_e_getParamater(l[i], "MOVES") + "\n";
			time  = time  + HighScore.db_e_getParamater(l[i], "TIME") + "\n";
		}

		text_Name.text  = names;
		text_Moves.text = moves;
		text_Time.text  = time;
	}
}
