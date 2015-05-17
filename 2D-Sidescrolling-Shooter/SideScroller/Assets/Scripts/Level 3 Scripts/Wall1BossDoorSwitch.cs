using UnityEngine;
using System.Collections;

public class Wall1BossDoorSwitch : MonoBehaviour {

	public GameObject wall1;
	public GameObject bossdoor;
	public GameObject switch10;
	public bool show = false;
	// Use this for initialization
	void Start () {
		bossdoor.SetActive (true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider other){
		if (switch10.collider) {
			show = !show;
			wall1.SetActive(show);
			bossdoor.SetActive(!show);
		}
		if (show == true) {
			switch10.renderer.material.color = Color.green;
		}
		else {
			switch10.renderer.material.color = Color.magenta;
		}
	}
}
