using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	string reg_game_name = "CarGame_gurinderhans";
	public float refresReqLength = 3.0f;
	public HostData[] hostData;
	string car_choosen,color_choosen;

	/********************/
	public Transform spawnOne;
	public Transform spawnTwo;
	public Transform spawnThree;
	public Transform spawnFour;
	public Transform spawnFive;
	/*******************/

	public bool playerHasName;
	public Material Green;

	//for change car
	string NewCarChoosen;
	bool isCarChanged,showCarChange;
	int myCamDepth;

	void  Awake(){
		//MSK PC
		MasterServer.ipAddress = "192.168.0.19";
		//Guri School PC
		//MasterServer.ipAddress = "10.82.32.35";

		MasterServer.port = 23466;
	}

	void Update(){
		if(Input.GetKeyDown (KeyCode.KeypadPeriod) && !showCarChange){
			showCarChange=true;
		}
	}


	public Camera gameViewCam;

	Vector3 spawnPosition;
	Vector3 CalcSpawnPos(){
		int randomNum = Random.Range (0, 5);

		if(randomNum == 0){
			spawnPosition = spawnOne.position;
		} else if(randomNum == 1){
			spawnPosition = spawnTwo.position;
		} else if(randomNum == 2){
			spawnPosition = spawnThree.position;
		} else if(randomNum == 3){
			spawnPosition  = spawnFour.position;
		} else if(randomNum == 4){
			spawnPosition = spawnFive.position;
		} else{
			spawnPosition = Vector3.zero;
		}

		return spawnPosition;
	}

	GameObject myCam,myCar;

	void SpawnPlayer(){
		Vector3 spawnPos = CalcSpawnPos ();
		Debug.Log ("Spawning Player");
		if (showCarChange) myCar = (GameObject) Network.Instantiate (Resources.Load(NewCarChoosen+color_choosen), spawnPos, Quaternion.identity, 0);
		else myCar = (GameObject) Network.Instantiate (Resources.Load(car_choosen+color_choosen), spawnPos, Quaternion.identity, 0);
		myCam = (GameObject) Instantiate(Resources.Load("DriveCam"), new Vector3(0f, 10f, 0f), Quaternion.identity);

		myCam.GetComponent<Camera> ().depth = myCamDepth;
		myCam.GetComponent<DriveCamController> ().enabled = true;
		myCar.GetComponent<CarController> ().enabled = true;
		myCar.GetComponentInChildren<Health> ().enabled = true;
		myCar.GetComponentInChildren<GUIText> ().enabled = false;
		myCar.GetComponentInChildren<PlayerIndicator> ().changeColorForPersonalCar (Green);
		myCar.GetComponentInChildren<TextMesh>().renderer.enabled = false;

		GameObject.Find ("OnHitTexture").GetComponent<ShowHitBorder> ().enabled = true;
		GameObject.Find ("Compass-Plane").GetComponent<CompassRotation> ().enabled = true;

		gameViewCam.enabled = false;

		this.gameObject.GetComponent<AllCheats> ().showInstructions = true;
	}
	void DestroyPlayer(){
		Debug.Log ("Switching Cars");
		myCam.GetComponent<GameModeManager> ().enabled = false;
		myCam.GetComponent<DriveCamController> ().enabled = false;;
		myCar.GetComponent<CarController> ().enabled = false;;
		myCar.GetComponentInChildren<Health> ().enabled = false;

		GameObject.Find ("OnHitTexture").GetComponent<ShowHitBorder> ().enabled = true;
		GameObject.Find ("Compass-Plane").GetComponent<CompassRotation> ().enabled = true;

		GameObject.Destroy (myCam);
		GameObject.Destroy (myCar);
	}
	
	public IEnumerator RefreshHostList(){
		Debug.Log ("RefreshHostList");
		MasterServer.RequestHostList (reg_game_name);//request host list from master server
		float timeEnd = Time.time + refresReqLength;
		
		while(Time.time < timeEnd){
			hostData = MasterServer.PollHostList();
			yield return new WaitForEndOfFrame();
		}
		
		if(hostData == null || hostData.Length == 0){
			Debug.Log ("No active servers have been found");
		}
		
	}

	/**************************************************************/
	//Call Backs from the Client and Server
	/**************************************************************/

	void StartServer(){
		Network.InitializeServer (32, 25000, false);
		MasterServer.RegisterHost (reg_game_name, "Unity3D Game", "Enjoy!");
	}

	void OnServerInitialized(){
		Debug.Log ("OnServerInitialized");
		SpawnPlayer ();
	}

	void OnMasterServerEvent(MasterServerEvent masterServerEvent){
		if(masterServerEvent == MasterServerEvent.RegistrationSucceeded){
			Debug.Log("Registration Succeeded");
		}
	}

	void OnConnectedToServer(){
		//SpawnPlayer ();
	}
	
	void OnDisconnectedFromServer(NetworkDisconnection info){
		Application.LoadLevel (0);
	}

	void OnFailedToConnect(NetworkConnectionError error){
		
	}

	void OnPlayerConnected(NetworkPlayer player){
		Debug.Log ("Player connected from:" + player.ipAddress + ":" + player.port);
	}

	void OnPlayerDisconnected(NetworkPlayer player){
		Debug.Log ("Player disconnected from:" + player.ipAddress + ":" + player.port);
		Network.RemoveRPCs (player);
		Network.DestroyPlayerObjects (player);
	}

	void OnFailedToConnectToMasterServer(NetworkConnectionError info){
		
	}

	void OnNetworkInstantiate(NetworkMessageInfo info){
	}

	void OnApplicationQuit(){
		if(Network.isServer){
			Network.Disconnect(200);
			MasterServer.UnregisterHost();
		}

		if(Network.isClient){
			Network.Disconnect(200);
		}
	}

	void ChooseCar(){
		//GUI BOX (l,t,w,h)
		GUI.Box(new Rect (25f, 25f, 250f, 240f), "CHOOSE YOUR CAR:");
		
		//Create buttons for cars
		if(GUI.Button( new Rect(50f, 50f, 200f, 30f) , "CHEVROLET CAMARO")){
			car_choosen = "CHEVROLET_CAMARO/";
			isCarChosen = true;
		} else if(GUI.Button( new Rect(50f, 85f, 200f, 30f) , "WAEZ CAR")){
			car_choosen = "waez_buksh_26179_assignsubmission_file_waezcar2/";
			isCarChosen = true;
		} else if(GUI.Button( new Rect(50f, 120f, 200f, 30f) , "McLaren MP4-12C N090211")){
			car_choosen = "Car McLaren MP4-12C N090211/";
			isCarChosen = true;
		} else if(GUI.Button( new Rect(50f, 155f, 200f, 30f) , "California Ferrari")){
			car_choosen = "California Ferrari/";
			isCarChosen = true;
		} else if(GUI.Button( new Rect(50f, 190f, 200f, 30f) , "MERCIELAGO640")){
			car_choosen = "MERCIELAGO640/";
			isCarChosen = true;
		} else if(GUI.Button( new Rect(50f, 225f, 200f, 30f) , "Mr.Powells JetCar")){
			car_choosen = "jetcar/";
			isCarChosen = true;
		}/*else if(GUI.Button( new Rect(50f, 190f, 200f, 30f) , "RANDOM CAR 5")){
			//print("camaro");
		}*/
	}

	void ChooseColor(){
		//GUI BOX (l,t,w,h)
		GUI.Box(new Rect (25f, 25f, 250f, 205f), "WHICH COLOR DO YOU LIKE:");

		if(GUI.Button( new Rect(50f, 50f, 200f, 30f) , "DEFAULT")){
			color_choosen = "DEFAULT";
			isColorChosen = true;
		} /*else if(GUI.Button( new Rect(50f, 85f, 200f, 30f) , "YELLOW")){
			//color_choosen = "CHEVROLET_CAMARO_YELLOW";
			//isColorChosen = true;
		}*/ /*else if(GUI.Button( new Rect(50f, 120f, 200f, 30f) , "RED")){
			color_choosen = "CHEVROLET_CAMARO_RED";
			isColorChosen = true;
		} else if(GUI.Button( new Rect(50f, 155f, 200f, 30f) , "BLUE")){
			color_choosen = "CHEVROLET_CAMARO_BLUE";
			isColorChosen = true;
		}*/// else if(GUI.Button( new Rect(50f, 85f/*CHANGE*/, 200f, 30f) , "MORE COMING SOON")){}
		/*else if(GUI.Button( new Rect(50f, 155f, 200f, 30f) , "GREY")){
			car_choosen = "CHEVROLET_CAMARO_GREY";
			isColorChosen = true;
		} else if(GUI.Button( new Rect(50f, 190f, 200f, 30f) , "YELLOW")){
			car_choosen = "CHEVROLET_CAMARO_YELLOW";
			isColorChosen = true;
		}*/
	}

	void ServerMenu(){
		if(this.GetComponent<AllCheats>().developerMode){
			//(l,t,w,h)
			if(GUI.Button(new Rect(25f, 15f, 150f, 30f), "Create Server")){
				// Start server function here
				StartServer();
				myCamDepth=0;
				isServerStarted = true;
			}
		}

		if(GUI.Button(new Rect(25f, 55f, 150f, 30f), "Find Game")){
			// Refresh server list funciton here
			StartCoroutine("RefreshHostList");
		}

		if(hostData != null){
			for(int i = 0; i < hostData.Length; i++){
				if(GUI.Button(new Rect(Screen.width/2, 65f+(30f*i), 300f, 30f), hostData[i].gameName)){
					Network.Connect(hostData[i]);
					myCamDepth=0;
					isServerStarted = true;
				}
			}
		}
		
	}

	public string myName;
	void GivePlayerName(){
		myName = GUI.TextField(new Rect(115f, 20.5f, 150f, 22.5f), myName, 25);
		if (Event.current.isKey && Event.current.keyCode == KeyCode.Return || GUI.Button (new Rect (0f, 20.5f, 100f, 22.5f), "Create Name")){
			//networkView.RPC ("changeName", RPCMode.AllBuffered, new object[]{myName});
			playerHasName = true;
		}
	}

	void ChangeCar(){
		GUI.Box(new Rect (25f, 25f, 250f, 240f), "CHOOSE YOUR CAR:");
		if(GUI.Button( new Rect(50f, 50f, 200f, 30f) , "CHEVROLET CAMARO")){
			NewCarChoosen = "CHEVROLET_CAMARO/";
			isCarChanged = true;
		} else if(GUI.Button( new Rect(50f, 85f, 200f, 30f) , "WAEZ CAR")){
			NewCarChoosen = "waez_buksh_26179_assignsubmission_file_waezcar2/";
			isCarChanged = true;
		} else if(GUI.Button( new Rect(50f, 120f, 200f, 30f) , "McLaren MP4-12C N090211")){
			NewCarChoosen = "Car McLaren MP4-12C N090211/";
			isCarChanged = true;
		} else if(GUI.Button( new Rect(50f, 155f, 200f, 30f) , "California Ferrari")){
			NewCarChoosen = "California Ferrari/";
			isCarChanged = true;
		} else if(GUI.Button( new Rect(50f, 190f, 200f, 30f) , "MERCIELAGO640")){
			NewCarChoosen = "MERCIELAGO640/";
			isCarChanged = true;
		} else if(GUI.Button( new Rect(50f, 225f, 200f, 30f) , "Mr.Powells JetCar")){
			NewCarChoosen = "jetcar/";
			isCarChanged = true;
		}
		if(isCarChanged){
			myCamDepth+=1;
			DestroyPlayer ();
			SpawnPlayer ();
			showCarChange=false;
		}

	}

	//GUI STUFF
	bool isCarChosen,isColorChosen,isServerStarted;

	public void OnGUI(){

		if(Network.isServer) GUILayout.Label("Running as a server.");
		else if(Network.isClient) GUILayout.Label("Running as a client.");

		if(!isCarChosen) ChooseCar();
		else if(!isColorChosen) ChooseColor();
		else if(!playerHasName) GivePlayerName();
		else if(!isServerStarted) ServerMenu();
		else if(showCarChange) ChangeCar();
	}
}