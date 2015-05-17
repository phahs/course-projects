using UnityEngine;
using System.Collections;

public class LoadLevel : MonoBehaviour {
	public int levelID = -1;
	public string levelName;
	public bool additive = false;
	// Use this for initialization/
	void OnTriggerEnter(Collider other) {
		//loading new levels
		/* Application class
		 * Replace the current level:
		 * Applicationn.LoadLevel(int id);
		 * Application.LoadLevel(string name);
		 * Objects in the current scene will be destroyed
		 * - Addm to the current level
		 * Application.LoadLevelAdditive (int id);
		 * Application.LoadLevelAdditive (string name); */
		if (other.gameObject.tag == "Player") {
			if (!additive) {
				if (levelName == null)
					Application.LoadLevel (levelName);
				else
					Application.LoadLevel (levelID);
			} else {
				if (levelName == null)
					Application.LoadLevelAdditive (levelName);
				else
					Application.LoadLevelAdditive (levelID);
			}
		}
	}

  }
