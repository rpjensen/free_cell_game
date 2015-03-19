using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Menu : MonoBehaviour {

	public Canvas quitMenu; 
	public Button startText;
	public Button highscoreText;
	public Button quitText;

	void Awake ()
		
	{
		quitMenu.enabled = false;	
	}
	
	public void ExitPress()
		
	{
		quitMenu.enabled = true; 
		startText.enabled = false; 
		quitText.enabled = false;
		highscoreText.enabled = false;
		
	}
	
	public void NoPress()
		
	{
		quitMenu.enabled = false;
		startText.enabled = true;
		quitText.enabled = true;
		highscoreText.enabled = true;
		
	}
	
	public void StartLevel ()
		
	{
		Application.LoadLevel ("_GameScene"); 
	}

	public void LoadHighScore ()
		
	{
		Application.LoadLevel ("_HighScores"); 
	}
	
	public void ExitGame () 
		
	{
		Application.Quit(); 
		
	}

}
