using UnityEngine;
using System.Collections;

public class HoundAIStateMachine : MonoBehaviour {

	public float stunDuration = 2f;
	private float stunTimer = 0f;
	
	//navigation variables
	public Transform[] patrolPoints;
	public GameObject target;
	public UnityEngine.AI.NavMeshAgent navMeshAgent;
	private string CurState;
	private int nextPatrolPoint;
	//this is used to remove the need for smell collider on the player
	public GameObject Player;
	private float DistanceFromPlayer;
	public float SmellRadius = 20f;
	//additional range before giving up the chase.. 
	//tht it would be cool to have hound notice u at less range than they chase your for
	public float Persistance=10f;
	//hounds health
	public int HealthPoints = 2000;
	
	//Audio Source & sound collections
	private AudioSource SoundSource;
	public AudioClip[] Attack_Sounds;
	public AudioClip[] Hurt_Sounds;
	public AudioClip[] Nutral_Sounds;

	//attacking variables
	private float AttackTotalTime = 1.19f;	//total # of frames in the animation
	private float AttackDamageTime = 0.16f;	//frame 16 in attack animation looks the best point to have damage
	public float AttackRange = 1f;			//launch attack and only apply damage if player is this close
	private float AttackTimer = 0f;			//keep track of time when in attack mode
	public GameObject AttackDamageBox;		//a box that becomes active to apply damge to player
	public float Attackduration = 0.03f;	//how long the damage box is active for

	//movement speeds
	public float Investigate_Speed = 5f;
	public float Chase_Speed = 10f;
	public float Attack_Speed = 20f;

	void PlayRandomSound (AudioClip[] Sounds){
		//play a random sound from collection if not already playing one
		if(SoundSource.isPlaying == false){
			int SoundNumber = Random.Range(0,100) % Sounds.Length;
			SoundSource.clip = Sounds[SoundNumber];
			SoundSource.Play();
		}
	}

	public void HurtPlayer(int Damage){

	}

	public void HurtMe(int Damage){
		HealthPoints -= Damage;
		SoundSource.Stop ();
		PlayRandomSound(Hurt_Sounds);
	}
	
	public void KillMe(){
		//what happens when the hound dies
		Debug.Log(this.gameObject.name + " HAS BEEN KILLED!");
		Destroy (this.gameObject);
	}

	public void ChangeAnim(string clip, int speed=1){
		//if speed or clip = 0 stop animation
		//else play and set speed (figure out later fi u get time)
		this.GetComponent<Animation>().Play (clip);
	}

	private void Awake(){
		navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
	}

	// Use this for initialization
	void Start () {
		//set the initial state of hound
		CurState = "asleep";
		//get the audio source component to use for playing sounds
		SoundSource = this.GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		//make sure the attack timer is reset whenever not in attack state
		if (CurState != "attack") {
			AttackTimer=0f;	//make sure timing isnt screwed up next attack state happens.
		}

		//reguardless of CurState if no health left the hound dies..
		if (HealthPoints <= 0) {
			KillMe();
		}

		if (CurState != "stunned") {
		
			//Distance from Player
			DistanceFromPlayer = Vector3.Distance (this.transform.position, Player.transform.position);
			
			//if within smell range begin chasing player
			if (DistanceFromPlayer < SmellRadius && CurState != "chase" && CurState != "stunned") { //less debug msg's with extra  condition lol. :)
				if (DistanceFromPlayer > AttackRange)
					CurState = "chase";
				Debug.Log (this.gameObject.name + " Chasing Player");
			}
			//if within attack range launch attack
			else if (DistanceFromPlayer <= AttackRange) {
				CurState = "attack";
			}
		}



		switch (CurState) {
		case "asleep":
			//speed & target
			navMeshAgent.Stop();
			//no animation for asleep
			this.GetComponent<Animation>().Stop();
			break;
		case "attack":
			//speed & target
			navMeshAgent.speed = Attack_Speed;
			navMeshAgent.SetDestination (Player.gameObject.transform.position);
			navMeshAgent.Resume ();

			//keep track of time in attack state (must be Before animation & sound)
			AttackTimer += Time.deltaTime;
			//only do animation and sound once per attack
			if(AttackTimer == Time.deltaTime){
				//soundfx
				SoundSource.Stop();
				PlayRandomSound(Attack_Sounds);
				//animation
				ChangeAnim("Attack");
			}
			//enable damage box at correct attack frame
			if(AttackTimer>=AttackDamageTime){
				AttackDamageBox.gameObject.SetActive(true);
			}
			//disable damagebox after duration
			if(AttackTimer >= (AttackDamageTime + Attackduration)){
				AttackDamageBox.gameObject.SetActive(false);
			}
			//reset attack timer when animation is compleate
			if(AttackTimer >= AttackTotalTime){
				AttackTimer=0f;
				ChangeAnim("Walking");
				PlayRandomSound(Nutral_Sounds);
			}
			break;
		case "chase":
			//speed and target
			navMeshAgent.speed = Chase_Speed;
			navMeshAgent.SetDestination (Player.gameObject.transform.position);
			navMeshAgent.Resume ();
			//sound
			PlayRandomSound(Nutral_Sounds);
			//animation
			ChangeAnim("Walking");
			//disbale chase when out of range
			if (DistanceFromPlayer > (SmellRadius + Persistance)) {
				CurState="investigate";
			}
			break;
		case "investigate":
			//speed and target
			navMeshAgent.speed = Investigate_Speed;
			navMeshAgent.destination = patrolPoints[nextPatrolPoint].position;
			navMeshAgent.Resume ();
			//change patrol points when arriving at target
			if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !navMeshAgent.pathPending) {
				nextPatrolPoint = Random.Range(0,100) % patrolPoints.Length;
			}
			//no sounds
			//animation
			ChangeAnim("Walking");
			break;
		case "stunned":
			//speed and target
			navMeshAgent.Stop ();
			//stun duration
			stunTimer += Time.deltaTime;
			if (stunTimer >= stunDuration) {
				Debug.Log (this.gameObject.name + " Stun wore off!");
				//reset stun counter for next time
				stunTimer = 0f;
				//go back to investigating
				CurState="investigate";
			}
			//no sounds
			//no animation
			this.GetComponent<Animation>().Stop();
			break;
		}
	}

	private void OnTriggerEnter(Collider other){
		//Debug.Log (this.gameObject + " was hit by " + other.gameObject.tag);

		//could not work out how to detect the non collision projectiles in range and
		//some other problems.
		//so for now the same action happens for 3 of the projectiles...
		if(other.gameObject.CompareTag("Projectilerock") || 
		   other.gameObject.CompareTag("Projectilebone") ||
		   other.gameObject.CompareTag("Projectilesyringe")){
			//owch!
			HurtMe(50);
			//we bin hit by a rock so get stunned
			CurState = "stunned";
		}

		//land mines should do lots of damge reguardless if they are walked over or hit by a throw
		if(other.gameObject.CompareTag("Projectilemine")){ //mine tags do not change...
			HurtMe(2000);
			Destroy(other.gameObject);
		}
	}

	public void OnTriggerExit(Collider other){

	}
}
