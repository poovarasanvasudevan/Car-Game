using UnityEngine;
using System.Collections;

public class PlayerIndicator : MonoBehaviour {
	Camera camMain,camMandeep,camMustafa;
	bool showMaps;
	Transform myCar;
	void Start() {
		myCar = this.gameObject.transform.parent.gameObject.transform;
		transform.localScale = new Vector3 (150, 1, 150);
		camMain=GameObject.Find ("MiniMap-main").GetComponent<Camera>();
		camMandeep=GameObject.Find ("MiniMap-mandeep").GetComponent<Camera>();
		camMustafa=GameObject.Find ("MiniMap-mustafa").GetComponent<Camera>();
	}

	public void changeColorForPersonalCar(Material mat){
		renderer.material = mat;
	}

	void Update () {
		transform.rotation = Quaternion.identity;
		Vector3 pos = myCar.position;
		pos.y += 1000f;
		transform.position = pos;

		if(Input.GetKeyDown (KeyCode.M)){
			showMaps=!showMaps;
			if(showMaps){
				camMain.farClipPlane=3000f;
				camMandeep.farClipPlane=3000f;
				camMustafa.farClipPlane=3000f;
			}
			else{
				camMain.farClipPlane=1f;
				camMandeep.farClipPlane=1f;
				camMustafa.farClipPlane=1f;
			}
		}
	}
}
