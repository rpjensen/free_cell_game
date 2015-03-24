using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class HighScore : MonoBehaviour {

	// number of high scores to save
	public static int NUM_HS = 8;
	public static int NUM_CHAR_IN_NAME = 11;
	public static string DATABASE_NAME = "HighScoreDatabase";
	public static string DATABASE_INIT = "FREECELL_DATABASE";

	/* DATABASE FORMAT by Joseph El-Khouri
	 * 
	 * begins with "FREECELL_DATABASE" followed by size in brackets
	 * and max name size in brackets
	 * an entry starts with "::"
	 * an entry needs NAME, MOVES, TIME, SET
	 * data for an entry is followed with {{ and }}

	HERE'S AN EXAMPLE DATABASE (newlines are ignored btw)

	FREECELL_DATABASE{{8}}{{11}}
	::NAME{{Joseph}}MOVES{{1044}}TIME{{01:22:15}}SET{{1}}
	::NAME{{DONGUS}}MOVES{{885}}TIME{{00:53:12}}SET{{1}}
	::NAME{{NO NAME AVL}}MOVES{{0}}TIME{{00:00:00}}SET{{0}}
	::NAME{{NO NAME AVL}}MOVES{{0}}TIME{{00:00:00}}SET{{0}}
	::NAME{{NO NAME AVL}}MOVES{{0}}TIME{{00:00:00}}SET{{0}}
	::NAME{{NO NAME AVL}}MOVES{{0}}TIME{{00:00:00}}SET{{0}}
	::NAME{{NO NAME AVL}}MOVES{{0}}TIME{{00:00:00}}SET{{0}}
	::NAME{{NO NAME AVL}}MOVES{{0}}TIME{{00:00:00}}SET{{0}}

	
	 */
	
	public static void db_CheckedCreateDatabase() {
		// only create a new database if there isn't one already
		if (!PlayerPrefs.GetString (DATABASE_NAME).StartsWith (DATABASE_INIT))
			db_CreateDatabase(); // ...create a new database
	}

	// creates a new formatted database
	public static void db_CreateDatabase() {
		// Database Header
		string newDatabase = DATABASE_INIT 
						+ "{{"
						+ NUM_HS
						+ "}}{{"
						+ NUM_CHAR_IN_NAME
						+ "}}\n";
		// Default Name
		string defaultName = "";
		for (int i=0; i<NUM_CHAR_IN_NAME; i++) {
			defaultName = defaultName + "-";
		}
		// Default Entries
		string defaultEntry = "NAME{{" 
							+ defaultName 
							+ "}}MOVES{{0}}TIME{{0:00:00}}SET{{0}}\n";
		for (int i=0; i<NUM_HS; i++) {
			newDatabase = newDatabase + "::" + defaultEntry;
		}
		PlayerPrefs.SetString (DATABASE_NAME, newDatabase);
	}

	// returns true if added to database
	// returns false if it did not beat the existing 8 entries
	public static bool db_AddHighScore (string name, int moves, string time) {
		if (name.Length > 11)
			name = name.Substring (0, NUM_CHAR_IN_NAME); // name limiter, just in case...
		if (moves > 99999) // too many moves hhhhhehehe 
			return false;
		string entry = dbp_EntryFormatter (name, moves, time);
		// get database as a list, add the new entry
		// sort it, and then
		List<string> db = db_getAsList ();
		db.Add (entry);
		db = dbp_Sort (db);
		db.RemoveAt (NUM_HS);
		// return that it was added if the new database is not equal to the old one
		return (!dbp_SetDatabaseIfExternalDatabaseDoesNotEqualsInternalDatabase (db));
	}

	// write a formatted database list to the player prefs database
	private static void dbp_DatabaseListWrite(List<string> l) {
		string currentDatabase = PlayerPrefs.GetString (DATABASE_NAME);
		// clear all database info except header
		int endOfHeader = currentDatabase.IndexOf ("::");
		string header = currentDatabase.Substring (0, endOfHeader);
		string formattedBody = "";
		for (int i=0; i<l.Count; i++) {
			formattedBody = formattedBody + "::" + l[i] + "\n";
		}
		PlayerPrefs.SetString (DATABASE_NAME, header + formattedBody);
	}

	// longest method name you'll ever see in you life
	// returns true if the external database is equal to the internal database
	// otherwise replace the internal database with the external database 
	private static bool dbp_SetDatabaseIfExternalDatabaseDoesNotEqualsInternalDatabase (List<string> ex) {
		string s_ex = dbp_DatabaseToStringConcat (ex).Replace ("\n", "");
		string database = dbp_DatabaseToStringConcat (db_getAsList ()).Replace ("\n", "");
		if (string.Equals(s_ex, database))
			return true;
		dbp_DatabaseListWrite (ex);
		return false;
	}

	private static string dbp_DatabaseToStringConcat (List<string> l) {
		string s = "";
		for (int i=0; i<l.Count; i++) {
			s = s + l[i] + "\n";
		}
		return s;
	}

	private static int dbp_CompareEntriesByMovesAndTime (string entry1, string entry2) {

		int e1_SET, e1_MOVES, e2_SET, e2_MOVES;
		string e1_TIME, e2_TIME;

		e1_SET   = int.Parse(db_e_getParamater (entry1, "SET"));
		e2_SET   = int.Parse(db_e_getParamater (entry2, "SET"));
		e1_MOVES = int.Parse(db_e_getParamater (entry1, "MOVES"));
		e2_MOVES = int.Parse(db_e_getParamater (entry2, "MOVES"));
		e1_TIME  = db_e_getParamater (entry1, "TIME");
		e2_TIME  = db_e_getParamater (entry2, "TIME");

		// if both SETs == 0 then they are the same
		if ((e1_SET == 0) && (e2_SET == 0))
			return 0;
		// if e1's SET == 1 and e2's SET == 0 then e1 is greater
		if (e1_SET > e2_SET)
			return -1;
		// less than if it's the other way around
		if (e1_SET < e2_SET)
			return 1;

		// if both moves are same, then compare by time
		if (e1_MOVES == e2_MOVES) {
			char[] seperator = {':'};
			string[] t1 = e1_TIME.Split(seperator, 3);
			string[] t2 = e2_TIME.Split(seperator, 3);
			int t1_h, t1_m, t1_s;
			int t2_h, t2_m, t2_s;
			t1_h = int.Parse(t1[0]);
			t1_m = int.Parse(t1[1]);
			t1_s = int.Parse(t1[2]);
			t2_h = int.Parse(t2[0]);
			t2_m = int.Parse(t2[1]);
			t2_s = int.Parse(t2[2]);
			if (t1_h == t2_h) {
				if (t1_m == t2_m) {
					if (t1_s == t2_s) {
						// if all paramaters match (woah) they're the same
						return 0;
					} else if (t1_s > t2_s)
						return 1;
					return -1;
				} else if (t1_m > t2_m)
					return 1;
				return -1;
			} else if (t1_h > t2_h) 
				return 1;
			return -1;
		} else if (e1_MOVES > e2_MOVES) 
			return 1;
		return -1;
	}

	// sorts the current database
	private static void dbp_Sort () {
		List<string> stringArray = db_getAsList ();
		stringArray.Sort(dbp_CompareEntriesByMovesAndTime);  
	}

	private static List<string> dbp_Sort (List<string> database) {
		database.Sort(dbp_CompareEntriesByMovesAndTime);  
		return database;
	}

	// get all entries as a List<string>
	public static List<string> db_getAsList() {
		List<string> s_list = new List<string> (NUM_HS);
		string currentDatabase = PlayerPrefs.GetString (DATABASE_NAME);
		string ENTRY_SEPERATOR = "::";
		int lastIndex = 0;
		for (int i=0; i<NUM_HS; i++) {
			int indexStart, indexEnd;
			indexStart = currentDatabase.IndexOf (ENTRY_SEPERATOR, lastIndex) + ENTRY_SEPERATOR.Length;
			indexEnd   = currentDatabase.IndexOf (ENTRY_SEPERATOR, indexStart);
			if (i == NUM_HS-1)
				indexEnd = currentDatabase.Length-1;
			lastIndex  = indexEnd;
			s_list.Add(currentDatabase.Substring(indexStart, indexEnd-indexStart).Replace("\n", ""));
		}
		return s_list;
	}

	// formats name, moves, time into an entry for the database
	private static string dbp_EntryFormatter (string name, int moves, string time) {
		return "NAME{{" 
			+ name
			+ "}}MOVES{{" 
			+ moves
			+ "}}TIME{{"
			+ time
			+ "}}SET{{1}}";
	}
	
	public static string db_e_getParamater(string entry, string param) {
		string db_PARAM = param + "{{";

		int indexStart, indexEnd;
		indexStart = entry.IndexOf(db_PARAM) + db_PARAM.Length;
		indexEnd = entry.IndexOf ("}}", indexStart);

		return entry.Substring (indexStart, indexEnd-indexStart);
	}

	public static void db_ToDebugLog() {
		List<string> l = db_getAsList ();
		string s = "";
		for (int i=0; i<l.Count; i++) {
			s = s + l[i] + "\n";
		}
		Debug.Log (s);
	}

	public static void db_ToDebugLog(List<string> l) {
		string s = "";
		for (int i=0; i<l.Count; i++) {
			s = s + l[i] + "\n";
		}
		Debug.Log (s);
	}

	// for loading the main menu
	public void MainMenu() {
		Application.LoadLevel("_MainMenu");
	}
}
