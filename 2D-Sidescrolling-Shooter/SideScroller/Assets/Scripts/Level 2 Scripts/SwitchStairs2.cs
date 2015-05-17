using UnityEngine;
using System.Collections;

public class SwitchStairs2 : MonoBehaviour {

	public GameObject stairs2;
	public GameObject switch3;
	public bool show = false;
	// Use this for initialization
	void Start () {
		stairs2.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider other){
		if (switch3.collider) {
			show = !show;
			stairs2.SetActive(show);
		}
		if (show == true) {
			switch3.renderer.material.color = Color.green;
		}
		else {
			switch3.renderer.material.color = Color.magenta;
		}
	}
}
