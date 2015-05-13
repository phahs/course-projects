using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	CharacterController cc;
	public float walkSpeed = 5f;
	public float runSpeed = 7f;
	public Vector3 facing;
	public int playerHealth;
	public int cAmmo;
	public int eAmmo;
	public int pAmmo;
	public int sAmmo;

	private float rad = 45f * Mathf.Deg2Rad;
	private int numOfKeysDown = 0;

	// Use this for initialization
	void Start () {
		cc = GetComponentInChildren<CharacterController> ();
		facing = gameObject.transform.forward;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Forward")) {
			numOfKeysDown++;
		}
		if (Input.GetButtonDown ("Backward")) {
			numOfKeysDown++;
		}
		if (Input.GetButtonDown ("Right")) {
			numOfKeysDown++;
		}
		if (Input.GetButtonDown ("Left")) {
			numOfKeysDown++;
		}
		if (numOfKeysDown == 1) {
			// Move forward
			if (Input.GetKey (KeyCode.W)) {
				facing = gameObject.transform.forward;
				if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
					cc.Move (facing * runSpeed * Time.deltaTime);
				}
				cc.Move (facing * walkSpeed * Time.deltaTime);
			}
			// Move backward
			if (Input.GetKey (KeyCode.S)) {
				facing = -gameObject.transform.forward;
				if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
					cc.Move (facing * runSpeed * Time.deltaTime);
				}
				cc.Move (facing * walkSpeed * Time.deltaTime);
			}
			// Move right
			if (Input.GetKey (KeyCode.A)) {
				facing = -gameObject.transform.right;
				if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
					cc.Move (facing * runSpeed * Time.deltaTime);
				}
				cc.Move (facing * walkSpeed * Time.deltaTime);
			}
			// Move left
			if (Input.GetKey (KeyCode.D)) {
				facing = gameObject.transform.right;
				if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
					cc.Move (facing * runSpeed * Time.deltaTime);
				}
				cc.Move (facing * walkSpeed * Time.deltaTime);
			}
		} else if (numOfKeysDown > 1) {
			if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A)){
				facing = Vector3.RotateTowards(gameObject.transform.forward, -gameObject.transform.right, rad, 0.0F);
				if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
					cc.Move (facing * runSpeed * Time.deltaTime);
				}
				cc.Move(facing * walkSpeed * Time.deltaTime);
			}
			if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D)){
				facing = Vector3.RotateTowards(gameObject.transform.forward, gameObject.transform.right, rad, 0.0F);
				if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
					cc.Move (facing * runSpeed * Time.deltaTime);
				}
				cc.Move(facing * walkSpeed * Time.deltaTime);
			}
			if(Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A)){
				facing = Vector3.RotateTowards(-gameObject.transform.forward, -gameObject.transform.right, rad, 0.0F);
				if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
					cc.Move (facing * runSpeed * Time.deltaTime);
				}
				cc.Move(facing * walkSpeed * Time.deltaTime);
			}
			if(Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D)){
				facing = Vector3.RotateTowards(-gameObject.transform.forward, gameObject.transform.right, rad, 0.0F);
				if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
					cc.Move (facing * runSpeed * Time.deltaTime);
				}
				cc.Move(facing * walkSpeed * Time.deltaTime);
			}
		}
		if (Input.GetButtonUp ("Forward")) {
			numOfKeysDown--;
		}
		if (Input.GetButtonUp ("Backward")) {
			numOfKeysDown--;
		}
		if (Input.GetButtonUp ("Right")) {
			numOfKeysDown--;
		}
		if (Input.GetButtonUp ("Left")) {
			numOfKeysDown--;
		}
	}
}
