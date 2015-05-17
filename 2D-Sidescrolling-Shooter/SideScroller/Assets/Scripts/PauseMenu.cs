using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {
	public bool showControls = false;
	bool paused;

	// Use this for initialization
	void Start () {
		paused = false;
		Time.timeScale = 1;


	}
	
	// Update is called once per frame
	void Update () {
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
		if (paused == true) {
			GUI.Box (new Rect (Screen.width/2 - 300, Screen.height/2 - 300, 700, 600), "PAUSE MENU");
			//Resume game button
			if (GUI.Button (new Rect (Screen.width/2 - 50, Screen.height/2 - 200, 200, 70), "RESUME GAME")) {
				paused = !paused;
				Time.timeScale = 1;
			}
			//Display Controls I suppose		
			if (GUI.Button (new Rect (Screen.width/2 - 50, Screen.height/2 - 100, 200, 70), "CONTROLS")) {
				showControls = !showControls;
			}
			//Back to main menu button			
			if (GUI.Button (new Rect (Screen.width/2 - 50, Screen.height/2, 200, 70), "BACK TO MAIN MENU")) {
				Application.LoadLevel("start screen");
				paused = !paused;
			}

			//exit game 
			if (GUI.Button (new Rect (Screen.width/2, Screen.height/2 + 100, 100, 70), "EXIT")) {
				Application.Quit();
			}

			if(showControls == true)
			{
				GUI.TextField(new Rect(Screen.width/2 - 350,Screen.height/2 - 100, 250, 150) , 
				              "\tCONTROLS\n\n" +
				              "MOVE LEFT:\tA\n" +
				              "MOVE RIGHT:\tD\n" +
				              "TURN AROUND:\tQ\n" +
				              "SHOOT:\t\tF\n" +
				              "SHOOT UP:\tW\n" +
				              "SHOOT DOWN:\tS\n" +
				              "JUMP:\t\tSPACEBAR");
			}
		}
	}
}