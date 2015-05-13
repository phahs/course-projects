using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {

	private bool controlsAreShowing;

	void Awake(){
		PlayerPrefs.SetString ("WhereAmI", "Menu");
	}

	void Start(){
		controlsAreShowing = false;
	}

	void OnGUI(){
		// Make a background box
		GUI.Box(new Rect(Screen.width/2 - 500, Screen.height/2 - 300, 1000, 700), "START MENU");
		
		// Start new game button
		if(GUI.Button(new Rect(Screen.width/2 - 450, Screen.height/2 + 100, 200, 70), "NEW GAME")) {
			PlayerPrefs.SetInt("Attachment", 0);
			PlayerPrefs.SetInt("EggSplodeMod", 0);
			PlayerPrefs.SetInt("PelletNozzle", 0);
			PlayerPrefs.SetInt("ShotgunRain", 0);
			PlayerPrefs.SetInt("EggAmmo", 0);
			PlayerPrefs.SetInt("PelletAmmo", 0);
			PlayerPrefs.SetInt("ShotAmmo", 0);
			PlayerPrefs.SetInt("PlayerHealth", 100);
			PlayerPrefs.SetFloat("FacingX", transform.forward.x);
			PlayerPrefs.SetFloat("FacingY", transform.forward.y);
			PlayerPrefs.SetFloat("FacingZ", transform.forward.z);
			PlayerPrefs.SetFloat("FruitopiaPosX", 5f);
			PlayerPrefs.SetFloat("FruitopiaPosY", 1f);
			PlayerPrefs.SetFloat("FruitopiaPosZ", 5f);
			PlayerPrefs.SetString("WhereAmI", "Fruitopia");
			Application.LoadLevel("Fruitopia1");
		}
		// Controls button
		if (GUI.Button (new Rect (Screen.width / 2 - 100, Screen.height / 2 + 100, 200, 70), "CONTROLS")) {
			controlsAreShowing = !controlsAreShowing;
		}

		// Quit game button
		if (GUI.Button (new Rect (Screen.width/2 + 250, Screen.height/2 + 100, 200, 70), "EXIT")) {
			Application.Quit();
		}

		if (controlsAreShowing) {
			GUI.Box (new Rect (Screen.width / 2 - 150, Screen.height / 2 - 250, 300, 300), "CONTROLS");
			GUI.TextField (new Rect (Screen.width / 2 - 150, Screen.height / 2 - 200, 300, 250), 
			               "\tW\t\tLook Forward\n" +
			               "\tS\t\tLook Backward\n"+
			               "\tA\t\tLook Left\n" +
			               "\tD\t\tLook Right\n" +
			               "\tArrow Keys\tMove\n" +
			               "\tSpacebar\t\tFire\n" +
			               "\tShift\t\tRun\n");
		}
	}
}
