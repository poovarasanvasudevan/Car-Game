using UnityEngine;
using System.Collections;

public class justlookat : MonoBehaviour {

	public Transform target;

	void Update () {
		transform.position = target.position;
		transform.rotation = target.rotation;
		transform.Translate (-Vector3.forward * 10);
		transform.Translate (Vector3.up * 10);
		transform.LookAt (target);
	}
}
