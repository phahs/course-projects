using UnityEngine;
using System.Collections;

public class Stairs6Wall3Switch : MonoBehaviour {

	public GameObject wall3;
	public GameObject stairs6;
	public GameObject switch5;
	public bool show = false;
	// Use this for initialization
	void Start () {
		wall3.SetActive (true);
		stairs6.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider other){
		if (switch5.collider) {
			show = !show;
			wall3.SetActive(!show);
			stairs6.SetActive(show);
		}
		if (show == true) {
			switch5.renderer.material.color = Color.green;
		}
		else {
			switch5.renderer.material.color = Color.magenta;
		}
	}
}
