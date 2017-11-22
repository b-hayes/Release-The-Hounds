using UnityEngine;
using System.Collections;

public class LoadOnClick : MonoBehaviour {
	

	
	public void LoadScene(int level)
	{
		level = 1;
	
		Application.LoadLevel (level);
	}
	void FixedUpdate() 
	{
		int level = 1;
		
		// move left if A or left-arrow pressed
		if (Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Space))
		{

			Application.LoadLevel (level);
			
		}
		if(Input.GetMouseButtonDown(0))
		{
		
			Application.LoadLevel (level);
			
		}
		
		
	}
}
