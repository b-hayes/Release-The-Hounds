using UnityEngine;
using System.Collections;



public class InGameMenu : MonoBehaviour {

	public GameObject uiToHide;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ResumeGame () {
		//UNpause game
		Time.timeScale = 1;
		//hide menu
		uiToHide.SetActive (false);
		//TAKE mouse control back
		Cursor.lockState = CursorLockMode.Locked;
	}
	public void EndGame(){
		//end game
		Application.Quit ();
	}

	public void PlayAgain(){
		Application.LoadLevel (1);
	}
}
