using UnityEngine;
using System.Collections;

public class SwitchWall1 : MonoBehaviour {

	public GameObject wall1;
	public GameObject switch7;
	public bool show = false;
	// Use this for initialization
	void Start () {
		wall1.SetActive (true);
		switch7.renderer.material.color = Color.green;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider other){
		if (switch7.collider) {
			show = !show;
			wall1.SetActive(show);
		}
		if (show == true) {
			switch7.renderer.material.color = Color.green;
		}
		else {
			switch7.renderer.material.color = Color.magenta;
		}
	}
}
