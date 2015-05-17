using UnityEngine;
using System.Collections;

public class UserInterface : MonoBehaviour {
	
	public bool paused;
	private GameObject player;
	private PlayerController pcInfo;

	// GUI rectangles
	Rect playerArea = new Rect(10, 10, 200, 100);
	Rect guiBox	 = new Rect(0,  0,  200, 100);
	Rect guiHealth = new Rect(0, 0, 200, 50);
	Rect guiCAmmo = new Rect(0, 20, 200, 50);
	Rect guiEAmmo = new Rect(0, 40, 200, 50);
	Rect guiPAmmo = new Rect(0, 60, 200, 50);
	Rect guiSAmmo = new Rect(0, 80, 200, 50);

	void Awake() {
		player = GameObject.FindGameObjectWithTag ("Player");
		pcInfo = player.GetComponent ("PlayerController") as PlayerController;
	}

	void Start () {
		paused = false;
		Time.timeScale = 1;
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			paused = !paused;
			if(paused){
				Time.timeScale = 0; //freeze the game
			}
			else {
				Time.timeScale = 1; //unfreeze
			}
		}
	}

	void OnGUI() {
		// Set our coordinate group
		GUI.BeginGroup (playerArea);
		// draw stuff!
		GUI.Box (guiBox, GUIContent.none);
		GUI.Label (guiHealth, "Health: " + pcInfo.currentPlayerHealth);

		GUI.Label (guiCAmmo, "Cannon Ammo: INFINITE");
		
		if (pcInfo.egg) {
			GUI.Label (guiEAmmo, "Egg Ammo: " + pcInfo.currentEAmmo);
		}
		if (pcInfo.pellet) {
			GUI.Label (guiPAmmo, "Pellet Ammo: " + pcInfo.currentPAmmo);
		}
		if (pcInfo.shot) {
			GUI.Label (guiSAmmo, "Shotgun Ammo: " + pcInfo.currentSAmmo);
		}
		// MUST NOT FORGET
		GUI.EndGroup ();

		if (paused){

			GUI.Box (new Rect (Screen.width/2 - 300, Screen.height/2 - 300, 700, 600), "PAUSE MENU");
			GUI.Box(new Rect(Screen.width/2 - 150, Screen.height/2 - 260, 400, 220), "CONTROLS");
			GUI.TextField (new Rect (Screen.width / 2 - 150, Screen.height / 2 - 240, 400, 200), 
			               "\tW\t\t\tLook Forward\n" +
			               "\tS\t\t\tLook Backward\n"+
			               "\tA\t\t\tLook Left\n" +
			               "\tD\t\t\tLook Right\n" +
			               "\tArrow Keys\t\tMove\n" +
			               "\tSpacebar\t\t\tFire\n" +
			               "\tShift\t\t\tRun\n");
			//Resume game button
			if (GUI.Button (new Rect (Screen.width/2 - 50, Screen.height/2, 200, 70), "RESUME GAME")) {
				paused = !paused;
				Time.timeScale = 1;
			}
			//Back to main menu button			
			if (GUI.Button (new Rect (Screen.width/2 - 50, Screen.height/2 + 100, 200, 70), "BACK TO MAIN MENU")) {
				Application.LoadLevel("TestScene");
				paused = !paused;
			}
			
			//exit game 
			if (GUI.Button (new Rect (Screen.width/2, Screen.height/2 + 200, 100, 70), "EXIT")) {
				Application.Quit();
			}
		}
	}
}
