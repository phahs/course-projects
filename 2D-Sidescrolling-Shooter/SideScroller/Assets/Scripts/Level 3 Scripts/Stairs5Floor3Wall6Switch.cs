using UnityEngine;
using System.Collections;

public class Stairs5Floor3Wall6Switch : MonoBehaviour {
	public GameObject stairs5;
	public GameObject floor3;
	public GameObject wall6;
	public GameObject switch2;
	public bool show = false;
	// Use this for initialization
	void Start () {
		floor3.SetActive (true);
		wall6.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider other){
		if (switch2.collider) {
			show = !show;
			floor3.SetActive(!show);
			stairs5.SetActive(!show);
			wall6.SetActive(show);
		}
		if (show == true) {
			switch2.renderer.material.color = Color.green;
		}
		else {
			switch2.renderer.material.color = Color.magenta;
		}
	}
}
