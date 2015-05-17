using UnityEngine;
using System.Collections;

public class SwitchStairs5Floor4 : MonoBehaviour {

	public GameObject floor4;
	public GameObject stairs3;
	public GameObject stairs5;
	public GameObject switch1;
	public bool show = false;
	// Use this for initialization
	void Start () {
		floor4.SetActive (true);
		stairs3.SetActive (false);
		stairs5.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider other){
		if (switch1.collider) {
			show = !show;
			floor4.SetActive(!show);
			stairs3.SetActive(show);
			stairs5.SetActive(show);
		}
		if (show == true) {
			switch1.renderer.material.color = Color.green;
		}
		else {
			switch1.renderer.material.color = Color.magenta;
		}
	}
}
