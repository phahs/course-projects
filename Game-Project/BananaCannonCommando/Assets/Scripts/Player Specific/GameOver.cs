using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {

	public bool defeated;
	private GameObject player;
	private PlayerController pcInfo;

	// Use this for initialization
	void Awake () {
		player = GameObject.FindGameObjectWithTag ("Player");
		pcInfo = player.GetComponent ("PlayerController") as PlayerController;
	}

	void Start() 
	{
		defeated = false;
	}

	void Update (){
		if (pcInfo.currentPlayerHealth <= 0) {
			defeated = true;
		}
	}
	
	void OnGUI() {
		if (defeated) {
			Time.timeScale = 0;
			GUI.Box (new Rect (Screen.width / 2 - 300, Screen.height / 2 - 300, 700, 600), "");
			GUI.Box (new Rect (Screen.width / 2 - 150, Screen.height / 2 - 260, 400, 220), "GAME OVER");

			//Continue game button
			if (GUI.Button (new Rect (Screen.width / 2 - 50, Screen.height / 2, 200, 70), "CONTINUE")) {
				Time.timeScale = 1;
				string level = PlayerPrefs.GetString ("WhereAmI");
				Application.LoadLevel (level);
			}
			//Back to main menu button			
			if (GUI.Button (new Rect (Screen.width / 2 - 50, Screen.height / 2 + 100, 200, 70), "BACK TO MAIN MENU")) {
				Time.timeScale = 1;
				Application.LoadLevel ("start screen");
			}
			
			//exit game 
			if (GUI.Button (new Rect (Screen.width / 2, Screen.height / 2 + 200, 100, 70), "EXIT")) {
				Application.Quit ();
			}
		}
	}
}
