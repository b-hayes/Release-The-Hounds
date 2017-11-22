using UnityEngine;
using System.Collections;

public class FluroLight : MonoBehaviour {
	
	public float flicker_duration = 3f;
	public float flick_speed = 0.1f; //lower value is faster
	public float flicker_interval = 10f;
	public bool randomise_interval = true;
	public float min_random_interval = 3f;
	public float max_random_interval = 20f;
	public bool flicker_objects = true;
	public GameObject[] objects;
	public bool OnWhenNotFlickering = true;
	
	private bool flickering = false;
	private float timepassed = 0f;
	private float flickTime =0f;
	private float timeTillNextToggle = 0f;
	
	private Light ls; //game objects light component
	
	// Use this for initialization
	void Start () {

		if (randomise_interval) {
			flicker_interval = Random.Range(min_random_interval, max_random_interval);
			Debug.Log("RANDOM CHNAGE: flicker_interval now = " + flicker_interval);
		}
		
		ls = this.GetComponent<Light> ();
		ls.enabled = OnWhenNotFlickering;
		ObjectsFlick ();
	}
	
	// Update is called once per frame
	void Update () {
		timepassed += Time.deltaTime;
		
		//ok 1st thing that should happen is the light should wait untill it is time to flicker
		if (timepassed > flicker_interval) {
			//enable flickering mode
			flickering = true;
			//reset time passed
			timepassed = 0f;
			//set time between light source toggles
			timeTillNextToggle = flick_speed;
		}
		
		//the next thing is flickering should occurr for the duration of flicker_duration
		if (flickering) {
			flickTime += Time.deltaTime; //keep track of how long we have been toggling for
			//togle lieght if it is time to do so acording to flick_speed / timeTillNextToggle
			if(timepassed > timeTillNextToggle){
				ls.enabled = !ls.enabled; //toggle the light source on or off
				ObjectsFlick();
				//reset time passed
				timepassed = 0f;
			}
			//disable flickering mode if flickertime has been reached
			if(flickTime > flicker_duration){
				flickering = false; //disable flickering toggles
				ls.enabled = OnWhenNotFlickering; //make sure the light is left on
				ObjectsFlick();
				//reset timepassed and wait for next flickering interval
				timepassed = 0f;
				flickTime = 0f;
			}
		}
	}

	void ObjectsFlick(){
		//makes given objects match the state of the light source
		if (flicker_objects) {
			foreach (GameObject o in objects){
				if (o!=null) o.SetActive(ls.enabled);
			}
		}
	}
}
