using UnityEngine;
using System.Collections;

public class Pathing : MonoBehaviour {

	public Transform target;
	public Vector3 seekTarget;
	public Transform[] patrols;
	private UnityEngine.AI.NavMeshAgent agent;
	public bool hasTarget = false;
	public bool hasSeekTarget = false;
	// Use this for initialization
	void Start () {
		agent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		searchForTarget();
	}
	
	// Update is called once per frame
	void Update () {
		if (hasTarget) {
			agent.SetDestination (target.position);
		} else if (hasSeekTarget) {		
			if (seekTarget.z - gameObject.transform.position.z < 3f && seekTarget.x - gameObject.transform.position.x < 3f) {
				//gameObject.GetComponent<Renderer> ().material.color = Color.yellow;
				hasSeekTarget = false;
			} else {
				//gameObject.GetComponent<Renderer> ().material.color = Color.red;
				agent.SetDestination (seekTarget);
			}
		} else {
			searchForTarget();
		}
	}
	
	void OnTriggerEnter(Collider Other){
		if (Other.gameObject.name == "Player") {
			target = Other.gameObject.transform;
			hasSeekTarget = false;
			hasTarget = true;
		}
	}

	void OnTriggerExit(Collider Other){
		if (Other.gameObject.name == "Player") {
			hasTarget = false;
		}
	}

	void searchForTarget(){
		seekTarget = (Random.insideUnitSphere * 50);
		seekTarget += gameObject.transform.position;
		seekTarget.y = 1.0f;
		if (seekTarget.x > 50 || seekTarget.z > 50 || seekTarget.x < -50 || seekTarget.z < -50) {
			searchForTarget();
		}

		hasSeekTarget = true;
	}
}


