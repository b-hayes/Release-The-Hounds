using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Characters.FirstPerson
{
	[RequireComponent(typeof (CharacterController))]
	[RequireComponent(typeof (AudioSource))]
	public class FirstPersonController : MonoBehaviour
	{
		//need to lock the dam mouse curser lol
		public bool LockMousePointer = false;
		//ingame menu
		public GameObject InGameUI;
		
		[SerializeField] private bool m_IsWalking;
		[SerializeField] private float m_WalkSpeed;
		[SerializeField] private float m_RunSpeed;
		[SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
		[SerializeField] private float m_JumpSpeed;
		[SerializeField] private float m_StickToGroundForce;
		[SerializeField] private float m_GravityMultiplier;
		[SerializeField] private MouseLook m_MouseLook;
		[SerializeField] private bool m_UseFovKick;
		[SerializeField] private FOVKick m_FovKick = new FOVKick();
		[SerializeField] private bool m_UseHeadBob;
		[SerializeField] private CurveControlledBob m_HeadBob = new CurveControlledBob();
		[SerializeField] private LerpControlledBob m_JumpBob = new LerpControlledBob();
		[SerializeField] private float m_StepInterval;
		[SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
		[SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
		[SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.
		
		private Camera m_Camera;
		private bool m_Jump;
		private float m_YRotation;
		private Vector2 m_Input;
		private Vector3 m_MoveDir = Vector3.zero;
		private CharacterController m_CharacterController;
		private CollisionFlags m_CollisionFlags;
		private bool m_PreviouslyGrounded;
		private Vector3 m_OriginalCameraPosition;
		private float m_StepCycle;
		private float m_NextStep;
		private bool m_Jumping;
		private AudioSource m_AudioSource;
		
		
		public int maxStamina = 100;
		public float curStamina = 5;
		public float regeneration = 1;
		public float staminaBarLength;
		public float staminaMaxBarLength;
		public int xpositionstam=50;
		public int ypositionstam=25;
		
		public int syringeregen=5;
		public int sprintdegen=4;
		
		public int maxhealth = 100;
		public float curhealth = 100;
		public float healthBarLength;
		public float healthMaxBarLength;
		public int xpositionhealth=50;
		public int ypositionhealth=100;
		
		public int hounddamage=34;
		
		public int syringes=2;
		public Texture aTexture;
		
		public int cooldown=100;
		public float curcd=100;
		
		
		private static Texture2D _staticRectTexture;
		private static GUIStyle _staticRectStyle;
		private static Texture2D _staticRectTexture2;
		private static GUIStyle _staticRectStyle2;
		
		
		//inventory stuff
		
		public string currentlyequiped="Rock";
		
		
		
		public GameObject UIrock; //ui items
		public GameObject UIsyringe; //item pickups
		public GameObject UIbone; //item pickups
		public GameObject UImine; //item pickups
		
		
		public int startingrocks=1;
		public int startingbones=1;
		public int startingmines=1;
		
		
		public GameObject rockproj;
		public GameObject syringeproj;
		public GameObject boneproj;
		public GameObject mineproj;
		
		public GameObject iconrock;
		public GameObject iconbone;
		public GameObject iconsyringe;
		public GameObject iconmine;
		
		public AudioClip hurt1;
		public AudioClip hurt2;
		public AudioClip hurt3death;
		private int numbertimesrekt=1;
		private float origregen;
		
		
		float bulletForce = 150f;
		float bulletSpeed = 20f;
		
		
		
		void OnTriggerEnter(Collider other){
			//Only player can pickup when this item is picked up. It is chaned to thrownitem so the item is not picked up straight away after being thrown.
			if (other.gameObject.CompareTag ("Damage")) {
				if (numbertimesrekt==1)
				{
					this.gameObject.AddComponent<AudioSource>();
					this.GetComponent<AudioSource>().clip = hurt1;
					this.GetComponent<AudioSource>().Play();
					
				}
				if (numbertimesrekt==2)
				{
					this.gameObject.AddComponent<AudioSource>();
					this.GetComponent<AudioSource>().clip = hurt2;
					this.GetComponent<AudioSource>().Play();
					
				}
				if (numbertimesrekt==3)
				{
					this.gameObject.AddComponent<AudioSource>();
					this.GetComponent<AudioSource>().clip = hurt3death;
					this.GetComponent<AudioSource>().Play();
					
				}
				numbertimesrekt=numbertimesrekt+1;
				if (numbertimesrekt>3)
					numbertimesrekt=Random.Range(1,3);
				curhealth=curhealth-hounddamage;
				//other.enabled=false;
				other.gameObject.SetActive(false) ;
				
				if (curhealth < 1)
				{
					Application.LoadLevel(1);
					Debug.Log ("you are dead");
				}
				
				
				healthBarLength = (Screen.width / 4) * (curhealth / (float)maxhealth);
				
				
				
			}
			
			if (other.gameObject.CompareTag ("Rock")) {
				//Calls pickup and update held methods on the player Item Script to pick this item up.
				Destroy (other.gameObject);
				startingrocks=startingrocks+1;
				if (currentlyequiped=="none")
				{
					currentlyequiped="Rock";
					UIrock.GetComponent<Renderer>().enabled = true;
				}
				
			}
			if (other.gameObject.CompareTag ("Bone")) {
				//Calls pickup and update held methods on the player Item Script to pick this item up.
				Destroy (other.gameObject);
				startingbones=startingbones+1;
				if (currentlyequiped=="none")
				{
					currentlyequiped="Bone";
					UIbone.GetComponent<Renderer>().enabled = true;
				}
			}
			if (other.gameObject.CompareTag ("Syringe")) {
				//Calls pickup and update held methods on the player Item Script to pick this item up.
				Destroy (other.gameObject);
				syringes=syringes+1;
				
				if (currentlyequiped=="none")
				{
					currentlyequiped="Syringe";
					UIsyringe.GetComponent<Renderer>().enabled = true;
				}
			}
			if (other.gameObject.CompareTag ("Mine")) {
				//Calls pickup and update held methods on the player Item Script to pick this item up.
				Destroy (other.gameObject);
				startingmines=startingmines+1;
				if (currentlyequiped=="none")
				{
					currentlyequiped="Mine";
					UImine.GetComponent<Renderer>().enabled = true;
				}
			}
			
			
		}
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		// Use this for initialization
		private void Start()
		{
			origregen=regeneration;
			//lock mouse??
			if (LockMousePointer) {
				Cursor.lockState = CursorLockMode.Locked;
			}
			
			
			UIsyringe.GetComponent<Renderer>().enabled = false;
			UIrock.GetComponent<Renderer>().enabled = false;
			UImine.GetComponent<Renderer>().enabled = false;
			UIbone.GetComponent<Renderer>().enabled = false;
			
			
			
			
			m_CharacterController = GetComponent<CharacterController>();
			m_Camera = Camera.main;
			m_OriginalCameraPosition = m_Camera.transform.localPosition;
			m_FovKick.Setup(m_Camera);
			m_HeadBob.Setup(m_Camera, m_StepInterval);
			m_StepCycle = 0f;
			m_NextStep = m_StepCycle/2f;
			m_Jumping = false;
			m_AudioSource = GetComponent<AudioSource>();
			m_MouseLook.Init(transform , m_Camera.transform);
			
			staminaBarLength = Screen.width / 4;
			staminaMaxBarLength = Screen.width / 4;
			
			healthBarLength = Screen.width / 4;
			healthMaxBarLength = Screen.width / 4;
		}
		
		
		
		// Update is called once per frame
		private void Update()
		{
			
			
			if (Input.GetMouseButtonDown(0)){
				
				if (currentlyequiped=="Rock" && startingrocks>0)
				{
					Camera cam = Camera.main;
					GameObject thrownrock = (GameObject)Instantiate(rockproj,cam.transform.position, cam.transform.rotation);
					thrownrock.GetComponent<Rigidbody>().velocity = cam.transform.forward * bulletSpeed;
					startingrocks=startingrocks-1;
					if (startingrocks==0)
					{
						currentlyequiped="none";
						UIrock.GetComponent<Renderer>().enabled = false;
					}
				}
				if (currentlyequiped=="Bone"&& startingbones>0)
				{
					Camera cam = Camera.main;
					GameObject thrownbone = (GameObject)Instantiate(boneproj,cam.transform.position, cam.transform.rotation);
					thrownbone.GetComponent<Rigidbody>().velocity = cam.transform.forward * bulletSpeed;
					startingbones=startingbones-1;
					if (startingbones==0)
					{
						currentlyequiped="none";
						UIbone.GetComponent<Renderer>().enabled = false;
					}
				}
				if (currentlyequiped=="Mine" && startingmines>0)
				{
					Camera cam = Camera.main;
					GameObject thrownmine = (GameObject)Instantiate(mineproj,cam.transform.position, cam.transform.rotation);
					thrownmine.GetComponent<Rigidbody>().velocity = cam.transform.forward * bulletSpeed;
					startingmines=startingmines-1;
					if (startingmines==0)
					{
						currentlyequiped="none";
						UImine.GetComponent<Renderer>().enabled = false;
					}
				}
				if (currentlyequiped=="Syringe" && syringes>0)
				{
					syringes=syringes-1;
					curcd=0;
					curStamina=curStamina+25;
					regeneration=5;
					if (syringes==0)
					{
						currentlyequiped="none";
						UIsyringe.GetComponent<Renderer>().enabled = false;
					}
				}
			}
			if (Input.GetKeyDown ("1"))
			{
				if (startingrocks>0)
				{
					currentlyequiped="Rock";
					UIrock.GetComponent<Renderer>().enabled = true;
					UIbone.GetComponent<Renderer>().enabled = false;
					UIsyringe.GetComponent<Renderer>().enabled = false;
					UImine.GetComponent<Renderer>().enabled = false;
					
				}
			}
			if (Input.GetKeyDown ("2"))
			{
				if (startingbones>0)
				{
					currentlyequiped="Bone";
					UIrock.GetComponent<Renderer>().enabled = false;
					UIbone.GetComponent<Renderer>().enabled = true;
					UIsyringe.GetComponent<Renderer>().enabled = false;
					UImine.GetComponent<Renderer>().enabled = false;
				}
			}
			if (Input.GetKeyDown ("3"))
			{
				if (syringes>0)
				{
					currentlyequiped="Syringe";
					UIrock.GetComponent<Renderer>().enabled = false;
					UIbone.GetComponent<Renderer>().enabled = false;
					UIsyringe.GetComponent<Renderer>().enabled = true;
					UImine.GetComponent<Renderer>().enabled = false;
				}
			}
			if (Input.GetKeyDown ("4"))
			{
				
				if (startingmines>0)
				{
					currentlyequiped="Mine";
					UIrock.GetComponent<Renderer>().enabled = false;
					UIbone.GetComponent<Renderer>().enabled = false;
					UIsyringe.GetComponent<Renderer>().enabled = false;
					UImine.GetComponent<Renderer>().enabled = true;
				}
			}
			if (Input.GetKeyDown ("5"))
			{
				currentlyequiped="none";
				UIrock.GetComponent<Renderer>().enabled = false;
				UIbone.GetComponent<Renderer>().enabled = false;
				UIsyringe.GetComponent<Renderer>().enabled = false;
				UImine.GetComponent<Renderer>().enabled = false;
			}
			
			AdjustCurrentStamina (0);
			if (curStamina < maxStamina)
				curStamina += regeneration * Time.deltaTime;
			
			if (curcd < cooldown)
				curcd += regeneration * Time.deltaTime;
			
			if (curcd > 30)
				regeneration = 1;
			
			if (Input.GetKey(KeyCode.I)||Input.GetKey (KeyCode.Q))
			{
				if (curcd>5)
				{
					syringes=syringes-1;
					curcd=0;
					curStamina=curStamina+25;
					regeneration=syringeregen;
				}
				
			}
			if (Input.GetKey(KeyCode.LeftShift)||Input.GetKey(KeyCode.RightShift))
			{
				if (curStamina > 5)
					curStamina -= (sprintdegen/2) * Time.deltaTime;
				else
					if (curStamina > 0)
						curStamina -= sprintdegen * Time.deltaTime;
				
			}
			
			if (Input.GetKeyDown ("9")){
				if (hounddamage==0)
				{
					hounddamage=34;
					regeneration=origregen;
				}
				else{
					hounddamage=0;
					regeneration=50;
				}
			}
			if (Input.GetKeyDown ("8"))
			{
				syringes=50;
				startingrocks=50;
				startingbones=50;
				startingmines=50;
			}
			
			if (startingrocks > 0) {
				iconrock.GetComponent<RawImage> ().enabled = true;
			}else
				iconrock.GetComponent<RawImage> ().enabled = false;
			if (startingbones > 0) {
				iconbone.GetComponent<RawImage> ().enabled = true;
			}else
				iconbone.GetComponent<RawImage> ().enabled = false;
			if (syringes > 0) {
				iconsyringe.GetComponent<RawImage> ().enabled = true;
			}else
				iconsyringe.GetComponent<RawImage> ().enabled = false;
			if (startingmines > 0) {
				iconmine.GetComponent<RawImage> ().enabled = true;
			}else
				iconmine.GetComponent<RawImage> ().enabled = false;
			
			RotateView();
			// the jump state needs to read here to make sure it is not missed
			if (!m_Jump)
			{
				m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
			}
			
			if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
			{
				StartCoroutine(m_JumpBob.DoBobCycle());
				PlayLandingSound();
				m_MoveDir.y = 0f;
				m_Jumping = false;
			}
			if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
			{
				m_MoveDir.y = 0f;
			}
			
			m_PreviouslyGrounded = m_CharacterController.isGrounded;
		}
		
		
		private void PlayLandingSound()
		{
			m_AudioSource.clip = m_LandSound;
			m_AudioSource.Play();
			m_NextStep = m_StepCycle + .5f;
		}
		
		
		private void FixedUpdate()
		{
			//in game menu
			if (Input.GetKey (KeyCode.Escape)) {
				//pause game
				Time.timeScale = 0;
				//show menu
				InGameUI.SetActive(true);
				//give mouse control back
				Cursor.lockState = CursorLockMode.None;
			}
			
			float speed;
			GetInput(out speed);
			// always move along the camera forward as it is the direction that it being aimed at
			Vector3 desiredMove = transform.forward*m_Input.y + transform.right*m_Input.x;
			
			// get a normal for the surface that is being touched to move along it
			RaycastHit hitInfo;
			Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
			                   m_CharacterController.height/2f);
			desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;
			
			m_MoveDir.x = desiredMove.x*speed;
			m_MoveDir.z = desiredMove.z*speed;
			
			
			if (m_CharacterController.isGrounded)
			{
				m_MoveDir.y = -m_StickToGroundForce;
				
				if (m_Jump)
				{
					m_MoveDir.y = m_JumpSpeed;
					PlayJumpSound();
					m_Jump = false;
					m_Jumping = true;
				}
			}
			else
			{
				m_MoveDir += Physics.gravity*m_GravityMultiplier*Time.fixedDeltaTime;
			}
			m_CollisionFlags = m_CharacterController.Move(m_MoveDir*Time.fixedDeltaTime);
			
			ProgressStepCycle(speed);
			UpdateCameraPosition(speed);
		}
		
		
		private void PlayJumpSound()
		{
			m_AudioSource.clip = m_JumpSound;
			m_AudioSource.Play();
		}
		
		
		private void ProgressStepCycle(float speed)
		{
			if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
			{
				m_StepCycle += (m_CharacterController.velocity.magnitude + (speed*(m_IsWalking ? 1f : m_RunstepLenghten)))*
					Time.fixedDeltaTime;
			}
			
			if (!(m_StepCycle > m_NextStep))
			{
				return;
			}
			
			m_NextStep = m_StepCycle + m_StepInterval;
			
			PlayFootStepAudio();
		}
		
		
		private void PlayFootStepAudio()
		{
			if (!m_CharacterController.isGrounded)
			{
				return;
			}
			// pick & play a random footstep sound from the array,
			// excluding sound at index 0
			int n = Random.Range(1, m_FootstepSounds.Length);
			m_AudioSource.clip = m_FootstepSounds[n];
			m_AudioSource.PlayOneShot(m_AudioSource.clip);
			// move picked sound to index 0 so it's not picked next time
			m_FootstepSounds[n] = m_FootstepSounds[0];
			m_FootstepSounds[0] = m_AudioSource.clip;
		}
		
		
		private void UpdateCameraPosition(float speed)
		{
			Vector3 newCameraPosition;
			if (!m_UseHeadBob)
			{
				return;
			}
			if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
			{
				m_Camera.transform.localPosition =
					m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
					                    (speed*(m_IsWalking ? 1f : m_RunstepLenghten)));
				newCameraPosition = m_Camera.transform.localPosition;
				newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
			}
			else
			{
				newCameraPosition = m_Camera.transform.localPosition;
				newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
			}
			m_Camera.transform.localPosition = newCameraPosition;
		}
		
		
		private void GetInput(out float speed)
		{
			// Read input
			float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
			float vertical = CrossPlatformInputManager.GetAxis("Vertical");
			
			bool waswalking = m_IsWalking;
			
			#if !MOBILE_INPUT
			// On standalone builds, walk/run speed is modified by a key press.
			// keep track of whether or not the character is walking or running
			m_IsWalking = !Input.GetKey(KeyCode.LeftShift);
			if (curStamina<5)
			{
				m_IsWalking = true;
			}
			#endif
			// set the desired speed to be walking or running
			speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
			m_Input = new Vector2(horizontal, vertical);
			
			// normalize input if it exceeds 1 in combined length:
			if (m_Input.sqrMagnitude > 1)
			{
				m_Input.Normalize();
			}
			
			// handle speed change to give an fov kick
			// only if the player is going to a run, is running and the fovkick is to be used
			if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
			{
				StopAllCoroutines();
				StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
			}
		}
		
		
		private void RotateView()
		{
			m_MouseLook.LookRotation (transform, m_Camera.transform);
		}
		
		
		private void OnControllerColliderHit(ControllerColliderHit hit)
		{
			Rigidbody body = hit.collider.attachedRigidbody;
			//dont move the rigidbody if the character is on top of it
			if (m_CollisionFlags == CollisionFlags.Below)
			{
				return;
			}
			
			if (body == null || body.isKinematic)
			{
				return;
			}
			body.AddForceAtPosition(m_CharacterController.velocity*0.1f, hit.point, ForceMode.Impulse);
		}
		
		void OnGUI() {
			if (_staticRectTexture == null) {
				_staticRectTexture = new Texture2D (1, 1);
			}
			
			if (_staticRectStyle == null) {
				_staticRectStyle = new GUIStyle ();
			}
			
			_staticRectTexture.SetPixel (0, 0, Color.red);
			_staticRectTexture.Apply ();
			
			_staticRectStyle.normal.background = _staticRectTexture;
			
			if (_staticRectTexture2 == null) {
				_staticRectTexture2 = new Texture2D (1, 1);
			}
			
			if (_staticRectStyle2 == null) {
				_staticRectStyle2 = new GUIStyle ();
			}
			
			_staticRectTexture2.SetPixel (0, 0, Color.yellow);
			_staticRectTexture2.Apply ();
			
			_staticRectStyle2.normal.background = _staticRectTexture2;
			
			
			GUI.Box (new Rect (xpositionstam, ypositionstam, staminaBarLength, 20), "", _staticRectStyle2);
			GUI.Box (new Rect (xpositionstam, ypositionstam, staminaMaxBarLength, 20), (int)curStamina + "/" + maxStamina);
			
			
			GUI.Box (new Rect (xpositionhealth, ypositionhealth, healthBarLength, 20), "", _staticRectStyle);
			GUI.Box (new Rect (xpositionhealth, ypositionhealth, healthMaxBarLength, 20), (int)curhealth + "/" + maxhealth);
			
			if (syringes > 0) {
				int loopycat = syringes;
				int loops = 0;
				while (loopycat>0) {
					
					GUI.DrawTexture (new Rect (xpositionstam + 10 + loops, ypositionstam + 30, 50, 50), aTexture, ScaleMode.ScaleToFit, true, 0.0F);
					loopycat = loopycat - 1;
					loops = loops + 50;
				}
			}
		}
		
		public void AdjustCurrentStamina(int adj) {
			curStamina += adj;
			
			if (curStamina < 0)
				curStamina = 0;
			
			if (curStamina > maxStamina)
				curStamina = maxStamina;	
			staminaBarLength = (Screen.width / 4) * (curStamina / (float)maxStamina);
			
		}
	}
	
}
