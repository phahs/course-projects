using UnityEngine;
using System.Collections;

public class SwitchFloor2 : MonoBehaviour {
	public GameObject floor2;
	public GameObject switch9;
	public bool show = false;
	// Use this for initialization
	void Start () {
		floor2.SetActive (true);
		switch9.renderer.material.color = Color.green;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider other){
		if (switch9.collider) {
			show = !show;
			floor2.SetActive(show);
		}
		if (show == true) {
			switch9.renderer.material.color = Color.green;
		}
		else {
			switch9.renderer.material.color = Color.magenta;
		}
	}
}
