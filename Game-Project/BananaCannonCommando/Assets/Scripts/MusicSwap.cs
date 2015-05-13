using UnityEngine;
using System.Collections;

public class MusicSwap : MonoBehaviour {

	public AudioClip[] playlist;
	private AudioSource music;
	private string currentLevel;

	void Start(){
		music = GetComponent<AudioSource> ();
		currentLevel = PlayerPrefs.GetString ("WhereAmI");

		if (currentLevel == "Fruitopia") {
			music.Stop ();
			music.clip = playlist [0];
		} else if (currentLevel == "TheOrchard") {
			music.Stop ();
			music.clip = playlist [1];
		} else if (currentLevel == "TheSuperMarket") {
			music.Stop ();
			music.clip = playlist [2];
		} else if (currentLevel == "TheGiantFruitCakeCastle") {
			music.Stop ();
			music.clip = playlist [3];
		}else if(currentLevel == "ThePark"){
			music.Stop();
			music.clip = playlist [4];
		}

		music.Play ();
	}
}
