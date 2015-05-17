using UnityEngine;
using System.Collections;

public class SwitchWall4 : MonoBehaviour {

	public GameObject wall4;
	public GameObject switch11;
	public bool show = false;
	// Use this for initialization
	void Start () {
		wall4.SetActive (true);
		switch11.renderer.material.color = Color.green;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider other){
		if (switch11.collider) {
			show = !show;
			wall4.SetActive(show);
		}
		if (show == true) {
			switch11.renderer.material.color = Color.green;
		}
		else {
			switch11.renderer.material.color = Color.magenta;
		}
	}
}
