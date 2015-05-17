using UnityEngine;
using System.Collections;

public class WinCondition : MonoBehaviour {

	private GameObject boss;
	private bool dontShow;
	// Use this for initialization
	void Start () {
		boss = GameObject.FindGameObjectWithTag ("Boss");
		if (boss == null) {
			dontShow = false;
		} else {
			dontShow = true;
		}
	}
	
	// Update is called once per frame
	void OnGUI () {
		if (boss == null) {
			if (dontShow) {
				GUI.Box (new Rect (Screen.width / 2 - 200, Screen.height / 2 - 150, 400, 300), "YOU HAVE DEFEATED THE BOSS!\nCONGRATULATIONS!!");
				if (GUI.Button (new Rect (Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 70), "BACK TO MAIN MENU")) {
					Application.LoadLevel ("start screen");
				}
				//exit game 
				if (GUI.Button (new Rect (Screen.width / 2 - 100, Screen.height / 2 + 50, 200, 70), "EXIT")) {
					Application.Quit ();
				}
			}
		}
	}
}
