using UnityEngine;
using System.Collections;

public class bonescript : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		
	}
	
	
	IEnumerator OnCollisionEnter(Collision collision) {
		
		print(Time.time);
		yield return new WaitForSeconds(1);
		print(Time.time);
		this.gameObject.tag="Bone";
	}
}