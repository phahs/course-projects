using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){

			GUI.Box (new Rect (Screen.width/2 - 200, Screen.height/2 - 150, 400, 300), "YOU ARE DEAD. GAME OVER");
			if (GUI.Button (new Rect (Screen.width/2 - 100 , Screen.height/2 - 50, 200, 70), "BACK TO MAIN MENU")) {
				Application.LoadLevel("start screen");
			}
			//exit game 
			if (GUI.Button (new Rect (Screen.width/2 - 100, Screen.height/2  + 50, 200, 70), "EXIT")) {
				Application.Quit();
			}
		}
		

}
