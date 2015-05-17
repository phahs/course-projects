using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	
	// Use this for initialization
		void OnGUI(){
		// Make a background box
		GUI.Box(new Rect(Screen.width/2 - 300, Screen.height/2 - 300,600,500), "START MENU");
		
		// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
		if(GUI.Button(new Rect(Screen.width/2 - 100,Screen.height/2 - 200,200,70), "NEW GAME")) {
			Application.LoadLevel("finalProject level 1"); //ok so this works, now to build a first level
		}
		//stage select
		if (GUI.Button (new Rect (Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 70), "STAGE SELECT")) {
			Application.LoadLevel("Stage Select");
				}
		// Make the second button.
		if(GUI.Button(new Rect(Screen.width/2 - 100,Screen.height/2,200,70), "HELP")) {
			Application.LoadLevel("help"); //load the help  menu scene
		}

		//third button
		if (GUI.Button (new Rect (Screen.width/2 - 100, Screen.height/2 + 100, 200, 70), "EXIT")) {
			Application.Quit();
		}
	}
}

	
