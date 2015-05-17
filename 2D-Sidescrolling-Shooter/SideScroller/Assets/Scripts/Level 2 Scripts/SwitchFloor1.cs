using UnityEngine;
using System.Collections;

public class SwitchFloor1 : MonoBehaviour {

	public GameObject floor1;
	public GameObject switch6;
	public bool show = false;
	// Use this for initialization
	void Start () {
		floor1.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider other){
		if (switch6.collider) {
			show = !show;
			floor1.SetActive(show);
		}
		if (show == true) {
			switch6.renderer.material.color = Color.green;
		}
		else {
			switch6.renderer.material.color = Color.magenta;
		}
	}
}
