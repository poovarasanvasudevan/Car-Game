using UnityEngine;
using System.Collections;

public class testingOnCube : MonoBehaviour {

	void FixedUpdate(){

		float front = -Input.GetAxis ("Vertical");
		float right = Input.GetAxis ("Horizontal");
		
		rigidbody.AddRelativeTorque (front * 10, right * 10, 0);

	}
	
}
