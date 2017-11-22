using UnityEngine;
using UnityEngine.UI;// we need this namespace in order to access UI elements within our script
using System.Collections;


public class menuScript : MonoBehaviour 
{
	public Canvas quitMenu;
	public Canvas optionsMenu;
	public Canvas controlsMenu;
	public Button startText;
	public Button exitText;
	public Button optionsText;
	public Button controlsText;

	
	void Start ()
		
	{
		quitMenu = quitMenu.GetComponent<Canvas>();
		optionsMenu = optionsMenu.GetComponent<Canvas>();
		controlsMenu = controlsMenu.GetComponent<Canvas>();
		startText = startText.GetComponent<Button> ();
		exitText = exitText.GetComponent<Button> ();
		quitMenu.enabled = false;
		controlsMenu.enabled = false;
		optionsMenu.enabled = false;

		optionsText = optionsText.GetComponent<Button> ();
		controlsText = controlsText.GetComponent<Button> ();
		
	}
	
	public void ExitPress() //this function will be used on our Exit button
		
	{
		quitMenu.enabled = true; //enable the Quit menu when we click the Exit button
		startText.enabled = false; //then disable the Play and Exit buttons so they cannot be clicked
		exitText.enabled = false;
		optionsText.enabled = false;
		controlsText.enabled = false;
		
	}
	public void OptionsPress() //this function will be used on our Exit button
		
	{
		optionsMenu.enabled = true; //enable the Quit menu when we click the Exit button
		startText.enabled = false; //then disable the Play and Exit buttons so they cannot be clicked
		exitText.enabled = false;
		optionsText.enabled = false;
		controlsText.enabled = false;
		
	}
	public void ControlsPress() //this function will be used on our Exit button
		
	{
		controlsMenu.enabled = true; //enable the Quit menu when we click the Exit button
		startText.enabled = false; //then disable the Play and Exit buttons so they cannot be clicked
		exitText.enabled = false;
		optionsText.enabled = false;
		controlsText.enabled = false;
		
	}
	
	public void NoPress() //this function will be used for our "NO" button in our Quit Menu
		
	{
		quitMenu.enabled = false; //we'll disable the quit menu, meaning it won't be visible anymore
		startText.enabled = true; //enable the Play and Exit buttons again so they can be clicked
		exitText.enabled = true;
		optionsText.enabled = true;
		controlsText.enabled = true;
		
	}
	public void OptionsBack() //this function will be used for our "NO" button in our Quit Menu
		
	{
		optionsMenu.enabled = false; //we'll disable the quit menu, meaning it won't be visible anymore
		startText.enabled = true; //enable the Play and Exit buttons again so they can be clicked
		exitText.enabled = true;
		optionsText.enabled = true;
		controlsText.enabled = true;
		
	}
	public void ControlsBack() //this function will be used for our "NO" button in our Quit Menu
		
	{
		controlsMenu.enabled = false; //we'll disable the quit menu, meaning it won't be visible anymore
		startText.enabled = true; //enable the Play and Exit buttons again so they can be clicked
		exitText.enabled = true;
		optionsText.enabled = true;
		controlsText.enabled = true;
		
	}
	
	public void StartLevel () //this function will be used on our Play button
		
	{
		Application.LoadLevel(1);
		
	}
	
	public void ExitGame () //This function will be used on our "Yes" button in our Quit menu
		
	{
		Application.Quit(); //this will quit our game. Note this will only work after building the game
		
	}
	
}