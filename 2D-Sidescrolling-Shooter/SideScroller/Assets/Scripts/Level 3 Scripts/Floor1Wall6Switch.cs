using UnityEngine;
using System.Collections;

public class Floor1Wall6Switch : MonoBehaviour {

	public GameObject wall6;
	public GameObject floor1;
	public GameObject switch8;
	public bool show = false;
	// Use this for initialization
	void Start () {
		floor1.SetActive (true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider other){
		if (switch8.collider) {
			show = !show;
			wall6.SetActive(show);
			floor1.SetActive(!show);
		}
		if (show == true) {
			switch8.renderer.material.color = Color.green;
		}
		else {
			switch8.renderer.material.color = Color.magenta;
		}
	}
}
