using UnityEngine;
using System.Collections;

public class SwitchWall3 : MonoBehaviour {

	public GameObject wall3;
	public GameObject switch10;
	public bool show = false;
	// Use this for initialization
	void Start () {
		wall3.SetActive (true);
		switch10.renderer.material.color = Color.green;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider other){
		if (switch10.collider) {
			show = !show;
			wall3.SetActive(show);
		}
		if (show == true) {
			switch10.renderer.material.color = Color.green;
		}
		else {
			switch10.renderer.material.color = Color.magenta;
		}
	}
}


