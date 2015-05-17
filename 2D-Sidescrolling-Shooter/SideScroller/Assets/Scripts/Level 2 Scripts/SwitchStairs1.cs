using UnityEngine;
using System.Collections;

public class SwitchStairs1 : MonoBehaviour {

	public GameObject stairs1;
	public GameObject switch2;
	public bool show = false;
	// Use this for initialization
	void Start () {
		stairs1.SetActive (true);
		switch2.renderer.material.color = Color.green;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		if (stairs1.collider) {
			show = !show;
			stairs1.SetActive(show);
		}
		if (show == true) {
			switch2.renderer.material.color = Color.green;
		}
		else {
			switch2.renderer.material.color = Color.magenta;
		}
	}
}
