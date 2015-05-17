using UnityEngine;
using System.Collections;

public class SwitchStairs4 : MonoBehaviour {

	public GameObject stairs4;
	public GameObject switch5;
	public bool show = false;
	// Use this for initialization
	void Start () {
		stairs4.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider other){
		if (switch5.collider) {
			show = !show;
			stairs4.SetActive(show);
		}
		if (show == true) {
			switch5.renderer.material.color = Color.green;
		}
		else {
			switch5.renderer.material.color = Color.magenta;
		}
	}
}
