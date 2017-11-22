using UnityEngine;
using System.Collections;

public class Audio_Zones : MonoBehaviour {

	private AudioSource SoundSource;

	// Use this for initialization
	void Start () {
		SoundSource = this.GetComponentInChildren<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		if (other.name == "HitBox") {
			SoundSource.enabled = true;
		}
	}

	void OnTriggerExit(Collider other){
		if (other.name == "HitBox") {
			SoundSource.enabled = false;
		}
	}
}
