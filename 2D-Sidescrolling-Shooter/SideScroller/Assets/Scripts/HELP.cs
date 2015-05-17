using UnityEngine;
using System.Collections;

public class HELP : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	void OnGUI(){
		GUI.Box (new Rect (Screen.width/2 - 300, Screen.height/2 - 300, 700, 600), "HELP");
		GUI.TextField (new Rect (Screen.width / 2 - 200, Screen.height / 2 - 250, 500, 400), 
		               "\tCONTROLS\n" +
		               "\n\tMOVE RIGHT:\t.................................\tD\n\n" +
		               "\tMOVE LEFT:\t.................................\tA\n\n" +
		               "\tJUMP:\t\t.................................\tSPACEBAR\n\n" +
		               "\tSHOOT FORWARD:\t.................................\tF\n\n" +
		               "\tSHOOT UP:\t.................................\tW\n\n" +
		               "\tSHOOT DOWN:\t.................................\tS\n\n" +
		               "\tTURN AROUND:\t.................................\tQ");

		if (GUI.Button (new Rect (Screen.width/2 - 50, Screen.height/2 + 200, 200, 70), "Back to Main Menu")) {
			Application.LoadLevel("start screen");
		}
	}
	// Update is called once per frame
	void Update () {

	}
}
