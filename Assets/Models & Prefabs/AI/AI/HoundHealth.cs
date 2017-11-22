using UnityEngine;
using System.Collections;

public class HoundHealth : MonoBehaviour {

	public int houndHealth; //Keeps track of hounds health.
	public int houndDamage; //Damage dealt to player.

	// Use this for initialization
	void Start () {
		houndHealth = 500;
		houndDamage = 40; //3 bites and then gameover.
	}
	
	// Update is called once per frame
	void Update () {
		CheckIfHoundDead ();
	}

	void CheckIfHoundDead(){
		if (houndHealth == 0) {
			//Play Sound
			Debug.Log ("Hound Dead");
			//The 3float delays destroying the hound.
			//We disable the colliders and renderers to hide the hound so sound still plays.
			gameObject.SetActive(false);
			Destroy(gameObject);
		}
	}
}
