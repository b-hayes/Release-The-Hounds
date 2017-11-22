using UnityEngine;
using System.Collections;

public class engame : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnTriggerEnter(Collider other){
		if (other.gameObject.name == "HitBox") {
			Cursor.lockState = CursorLockMode.None;
			Application.LoadLevel(2);
		}
	}
}
