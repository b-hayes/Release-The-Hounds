using UnityEngine;
using System;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

public class playrandomsounds : MonoBehaviour {
	public AudioClip myclip1;
	public AudioClip myclip2;
	public AudioClip myclip3;
	public AudioClip myclip4;
	public AudioClip myclip5;
	public AudioClip myclip6;
	public AudioClip myclip7;
	public AudioClip myclip8;
	public AudioClip myclip9;
	public AudioClip myclip10;

	public int numberofsounds=10;
	private float r;
	public float minwait =10f;
	public float maxwait = 20f;
	private float currenttime = 0f;

	// Use this for initialization
	void Start () {
		r = Random.Range (minwait, maxwait);
		this.gameObject.AddComponent<AudioSource>();
		this.GetComponent<AudioSource>().clip = myclip1;
		this.GetComponent<AudioSource>().Play();
	}
	
	// Update is called once per frame
	void Update () {
		currenttime += 1 * Time.deltaTime;

		if (currenttime > r)
		{
			int decide = Random.Range(1, numberofsounds);
			if (decide==1)
			{
			this.gameObject.AddComponent<AudioSource>();
			this.GetComponent<AudioSource>().clip = myclip1;
			this.GetComponent<AudioSource>().Play();
			
			}
			if (decide==1)
			{
				this.gameObject.AddComponent<AudioSource>();
				this.GetComponent<AudioSource>().clip = myclip1;
				this.GetComponent<AudioSource>().Play();

			}
			if (decide==2)
			{
				this.gameObject.AddComponent<AudioSource>();
				this.GetComponent<AudioSource>().clip = myclip2;
				this.GetComponent<AudioSource>().Play();

			}
			if (decide==3)
			{
				this.gameObject.AddComponent<AudioSource>();
				this.GetComponent<AudioSource>().clip = myclip3;
				this.GetComponent<AudioSource>().Play();

			}
			if (decide==4)
			{
				this.gameObject.AddComponent<AudioSource>();
				this.GetComponent<AudioSource>().clip = myclip4;
				this.GetComponent<AudioSource>().Play();

			}
			if (decide==5)
			{
				this.gameObject.AddComponent<AudioSource>();
				this.GetComponent<AudioSource>().clip = myclip5;
				this.GetComponent<AudioSource>().Play();
	
			}
			if (decide==6)
			{
				this.gameObject.AddComponent<AudioSource>();
				this.GetComponent<AudioSource>().clip = myclip6;
				this.GetComponent<AudioSource>().Play();

			}
			if (decide==7)
			{
				this.gameObject.AddComponent<AudioSource>();
				this.GetComponent<AudioSource>().clip = myclip7;
				this.GetComponent<AudioSource>().Play();

			}
			if (decide==8)
			{
				this.gameObject.AddComponent<AudioSource>();
				this.GetComponent<AudioSource>().clip = myclip8;
				this.GetComponent<AudioSource>().Play();
	
			}
			if (decide==9)
			{
				this.gameObject.AddComponent<AudioSource>();
				this.GetComponent<AudioSource>().clip = myclip9;
				this.GetComponent<AudioSource>().Play();

			}
			if (decide==10)
			{
				this.gameObject.AddComponent<AudioSource>();
				this.GetComponent<AudioSource>().clip = myclip10;
				this.GetComponent<AudioSource>().Play();
	
			}

			r = Random.Range(minwait, maxwait);
			currenttime=0;
		}

	
	}
}
