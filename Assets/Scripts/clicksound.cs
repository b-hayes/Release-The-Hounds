using UnityEngine;
using System.Collections;

public class clicksound : MonoBehaviour {

	public AudioClip myclip;
	
	// Use this for initialization
	void Start ()
	{
		this.gameObject.AddComponent<AudioSource>();
		this.GetComponent<AudioSource>().clip = myclip;
		this.GetComponent<AudioSource>().Play();
	}
	void FixedUpdate() 
	{


		if(Input.GetMouseButtonDown(0))
		{
			this.gameObject.AddComponent<AudioSource>();
			this.GetComponent<AudioSource>().clip = myclip;
			this.GetComponent<AudioSource>().Play();

			
		}
		
		
	}
}