using UnityEngine;
using System.Collections;

public class StageSelect : MonoBehaviour {
	
	void OnGUI() {
		GUI.Box(new Rect(Screen.width/2 - 300, Screen.height/2 - 300, 600, 500), "STAGE SELECT");

		if(GUI.Button(new Rect(Screen.width/2 - 100, Screen.height/2 - 200, 200, 70), "Level 1")){
			Application.LoadLevel("finalProject level 1");
		}
		if(GUI.Button(new Rect(Screen.width/2 - 100, Screen.height/2 - 100, 200, 70), "Level 2")){
			Application.LoadLevel("finalProject level 2");
		}
		if(GUI.Button(new Rect(Screen.width/2 - 100, Screen.height/2, 200, 70), "Level 3")){
			Application.LoadLevel("finalProject level 3");
		}

		if (GUI.Button (new Rect (Screen.width/2 - 100, Screen.height/2 + 100, 200, 70), "Back to Main Menu")) {
			Application.LoadLevel("start screen");
		}
	}
}

