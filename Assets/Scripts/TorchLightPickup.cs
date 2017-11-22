using UnityEngine;
using System.Collections;

public class TorchLightPickup : MonoBehaviour {

	public float BurnOutTime = 60f;
	public GameObject PlayerTorch;
	public bool TorchOn =false;

	private float CountTime = 0f;

	// Use this for initialization
	void Start () {
		PlayerTorch.SetActive (TorchOn);
	}
	
	// Update is called once per frame
	void Update () {

		if (TorchOn) CountTime += Time.deltaTime;
		if (CountTime > BurnOutTime) {
			TorchOn = false;
			PlayerTorch.SetActive(TorchOn);
			CountTime=0;
		}
	}

	private void OnTriggerEnter(Collision collision){ //different to OnTriggerEnter

		//Debug.Log ( " collided with " + other.gameObject.name + " tagged as " + other.gameObject.tag);

		if (collision.gameObject.tag =="TorchLight" && TorchOn == false) {//only pickup if yours has run out
			Destroy(collision.gameObject);
			TorchOn=true;
			PlayerTorch.SetActive(TorchOn);
			CountTime=0f;
		}
	}
}
