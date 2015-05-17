using UnityEngine;
using System.Collections;

public class Wall4Switch : MonoBehaviour {

	public GameObject wall4;
	public GameObject floor5;
	public GameObject switch6;
	public bool show = true;
	// Use this for initialization
	void Start () {
		wall4.SetActive (true);
		switch6.renderer.material.color = Color.green;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider other){
		if (switch6.collider) {
			show = !show;
			wall4.SetActive(show);
			floor5.SetActive(!show);
		}
		if (show == true) {
			switch6.renderer.material.color = Color.green;
		}
		else {
			switch6.renderer.material.color = Color.magenta;
		}
	}
}
