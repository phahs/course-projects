using UnityEngine;
using System.Collections;

public class SwitchWall2 : MonoBehaviour {

	public GameObject wall2;
	public GameObject switch8;
	public bool show = false;
	// Use this for initialization
	void Start () {
		wall2.SetActive (true);
		switch8.renderer.material.color = Color.green;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider other){
		if (switch8.collider) {
			show = !show;
			wall2.SetActive(show);
		}
		if (show == true) {
			switch8.renderer.material.color = Color.green;
		}
		else {
			switch8.renderer.material.color = Color.magenta;
		}
	}
}
