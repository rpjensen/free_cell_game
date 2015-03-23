using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Menu : MonoBehaviour {

	public Canvas quitMenu; 
	public Button startText;
	public Button highscoreText;
	public Button quitText;
	public Camera mainCamera;

	private Color color;
	private Color originalColor;
	private Color darkColor;
	private Color targetColor;

	void Awake ()
		
	{
		quitMenu.enabled = false;	
		color = mainCamera.backgroundColor;
		originalColor = mainCamera.backgroundColor;
		darkColor = new Color (0.0353f, 0.31765f, 0f);
		targetColor = mainCamera.backgroundColor;
	}
	
	public void ExitPress()
		
	{
		quitMenu.enabled = true; 
		startText.enabled = false; 
		quitText.enabled = false;
		highscoreText.enabled = false;
		targetColor = darkColor;
		
	}

	public void CreditsPress(){
		Application.LoadLevel ("_Credits");
		}
	
	public void NoPress()
		
	{
		quitMenu.enabled = false;
		startText.enabled = true;
		quitText.enabled = true;
		highscoreText.enabled = true;
		targetColor = originalColor;
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

	public void FixedUpdate() {
		float SMOOTH_FACTOR = 0.1f;
		color.r = color.r + ((targetColor.r - color.r) * SMOOTH_FACTOR);
		color.g = color.g + ((targetColor.g - color.g) * SMOOTH_FACTOR);
		color.b = color.b + ((targetColor.b - color.b) * SMOOTH_FACTOR);
		mainCamera.backgroundColor = color;
	}

}
