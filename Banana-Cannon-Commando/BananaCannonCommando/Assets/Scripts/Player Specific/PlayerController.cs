using UnityEngine;
using System.Collections;
// gives actions to the buttons the player will be pressing
public class PlayerController : MonoBehaviour {

	CharacterController cc;
	public float walkSpeed = 5f;
	public float runSpeed = 7f;
	public int numOfKeysDown = 0;
	public int numOfArrowsDown = 0;
	public Vector3 facing;
	public GameObject cannonPrefab;
	public GameObject eggPrefab;
	public GameObject pelletPrefab;
	public GameObject shotPrefab;
	private float cannonForce = 600f;
	private float eggForce = 600f;
	private float pelletForce = 600f;
	private float shotForce =  600f;
	public int attachment;
	public bool hasFired;
	public float timer;
	public static float cannonInterval = 0.5f;
	public static float eggInterval = 3f;
	public static float pelletInterval = 0.5f;
	public static float shotInterval = 2f;
	public int maxEAmmo = 40;
	public int maxPAmmo = 100;
	public int maxSAmmo = 50;
	public int currentPlayerHealth;
	public int currentCAmmo;
	public int currentEAmmo;
	public int currentPAmmo;
	public int currentSAmmo;
	public static bool cannon = true;
	public bool egg;
	public bool pellet;
	public bool shot;
	public int warped;
	public Sprite [] spriteListBaseCannon; // 0 is front, 7 is frontleft
	public Sprite [] spriteListEggSplode; // 0 is front, 7 is frontleft
	public Sprite [] spriteListPelletNozzle; // 0 is front, 7 is frontleft
	public Sprite [] spriteListShotgunRain; // 0 is front, 7 is frontleft

	private float rad = 45f * Mathf.Deg2Rad;
	private enum weaponAttachment {cannon, egg, pellet, shot};
	public NavMeshAgent nav;
	private UserInterface ui;
	private SpriteRenderer currentSprite;
	private GameOver finish;
	
	void Start () {
		nav = GetComponent<NavMeshAgent> ();
		ui = GetComponent<UserInterface> ();
		finish = GetComponent<GameOver> ();
		currentSprite = GetComponent<SpriteRenderer> ();
		facing.x = PlayerPrefs.GetFloat("FacingX");
		facing.y = PlayerPrefs.GetFloat ("FacingY");
		facing.z = PlayerPrefs.GetFloat ("FacingZ");
		transform.position.Set(PlayerPrefs.GetFloat("FruitopiaPosX"),PlayerPrefs.GetFloat("FruitopiaPosY"),PlayerPrefs.GetFloat("FruitopiaPosZ"));
//		Transform.position.y = PlayerPrefs.GetFloat("FruitopiaPosY");
//		Transform.position.z = PlayerPrefs.GetFloat("FruitopiaPosZ");
		attachment = PlayerPrefs.GetInt ("Attachment");
		hasFired = false;
		warped = PlayerPrefs.GetInt ("Warped");
		timer = 0;
		if (1 == PlayerPrefs.GetInt ("EggSplodeMod")) {

			egg = true;
		} else {
			egg = false;
		}
		if (1 == PlayerPrefs.GetInt ("PelletNozzle")) {
			
			pellet = true;
		} else {
			pellet = false;
		}
		if (1 == PlayerPrefs.GetInt ("ShotgunRain")) {
			
			shot = true;
		} else {
			shot = false;
		}
		currentPlayerHealth = PlayerPrefs.GetInt("PlayerHealth");
		currentEAmmo = PlayerPrefs.GetInt("EggAmmo");
		currentPAmmo = PlayerPrefs.GetInt("PelletAmmo");
		currentSAmmo = PlayerPrefs.GetInt("ShotAmmo");
		nav.speed = walkSpeed;

		switch (attachment) {
		case (int)weaponAttachment.cannon:
			if (facing == gameObject.transform.forward) {
				currentSprite.sprite = spriteListBaseCannon [0];
			} else if (facing == gameObject.transform.right) {
				currentSprite.sprite = spriteListBaseCannon [2];
			} else if (facing == -gameObject.transform.forward) {
				currentSprite.sprite = spriteListBaseCannon [4];
			} else if (facing == -gameObject.transform.right) {
				currentSprite.sprite = spriteListBaseCannon [6];
			}
			break;
		case (int)weaponAttachment.egg:
			if (facing == gameObject.transform.forward) {
				currentSprite.sprite = spriteListEggSplode [0];
			} else if (facing == gameObject.transform.right) {
				currentSprite.sprite = spriteListEggSplode [2];
			} else if (facing == -gameObject.transform.forward) {
				currentSprite.sprite = spriteListEggSplode [4];
			} else if (facing == -gameObject.transform.right) {
				currentSprite.sprite = spriteListEggSplode [6];
			}
			break;
		case (int)weaponAttachment.pellet:
			if (facing == gameObject.transform.forward) {
				currentSprite.sprite = spriteListPelletNozzle [0];
			} else if (facing == gameObject.transform.right) {
				currentSprite.sprite = spriteListPelletNozzle [2];
			} else if (facing == -gameObject.transform.forward) {
				currentSprite.sprite = spriteListPelletNozzle [4];
			} else if (facing == -gameObject.transform.right) {
				currentSprite.sprite = spriteListPelletNozzle [6];
			}
			break;
		case (int)weaponAttachment.shot:
			if (facing == gameObject.transform.forward) {
				currentSprite.sprite = spriteListShotgunRain [0];
			} else if (facing == gameObject.transform.right) {
				currentSprite.sprite = spriteListShotgunRain [2];
			} else if (facing == -gameObject.transform.forward) {
				currentSprite.sprite = spriteListShotgunRain [4];
			} else if (facing == -gameObject.transform.right) {
				currentSprite.sprite = spriteListShotgunRain [6];
			}
			break;
		}
	}

	void Update()
	{
		// determining if motion puttons have been pushed
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
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			numOfArrowsDown++;
		}
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			numOfArrowsDown++;
		}
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			numOfArrowsDown++;
		}
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			numOfArrowsDown++;
		}
		if (Input.GetKeyUp (KeyCode.UpArrow)) {
			numOfArrowsDown--;
		}
		if (Input.GetKeyUp (KeyCode.DownArrow)) {
			numOfArrowsDown--;
		}
		if (Input.GetKeyUp (KeyCode.RightArrow)) {
			numOfArrowsDown--;
		}
		if (Input.GetKeyUp (KeyCode.LeftArrow)) {
			numOfArrowsDown--;
		}

		// facing
		if (numOfKeysDown == 1) {
			// Face forward
			if (Input.GetKey (KeyCode.W)) {
				facing = gameObject.transform.forward;
			}
			// Face backward
			if (Input.GetKey (KeyCode.S)) {
				facing = -gameObject.transform.forward;
			}
			// Face right
			if (Input.GetKey (KeyCode.A)) {
				facing = -gameObject.transform.right;
			}
			// Face left
			if (Input.GetKey (KeyCode.D)) {
				facing = gameObject.transform.right;
			}
		} else if (numOfKeysDown > 1) {
			if (Input.GetKey (KeyCode.W) && Input.GetKey (KeyCode.A)) {
				facing = Vector3.RotateTowards (gameObject.transform.forward, -gameObject.transform.right, rad, 0.0F);
			}
			if (Input.GetKey (KeyCode.W) && Input.GetKey (KeyCode.D)) {
				facing = Vector3.RotateTowards (gameObject.transform.forward, gameObject.transform.right, rad, 0.0F);
			}
			if (Input.GetKey (KeyCode.S) && Input.GetKey (KeyCode.A)) {
				facing = Vector3.RotateTowards (-gameObject.transform.forward, -gameObject.transform.right, rad, 0.0F);
			}
			if (Input.GetKey (KeyCode.S) && Input.GetKey (KeyCode.D)) {
				facing = Vector3.RotateTowards (-gameObject.transform.forward, gameObject.transform.right, rad, 0.0F);
			}
		} 
		// movement
/*
		if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
					cc.Move (facing * runSpeed * Time.deltaTime);
				}
				cc.Move(facing * walkSpeed * Time.deltaTime);
*/
		if (!ui.paused && !finish.defeated) {
			if (numOfArrowsDown == 1) {

				// Move forward
				if (Input.GetKey (KeyCode.UpArrow)) {
					nav.speed = walkSpeed;
					nav.velocity = Vector3.forward * walkSpeed;
					if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) {
						nav.speed = runSpeed;
						nav.velocity = Vector3.forward * runSpeed;
					}
				}
				// Move backward
				if (Input.GetKey (KeyCode.DownArrow)) {
					nav.speed = walkSpeed;
					nav.velocity = -Vector3.forward * walkSpeed;
					if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) {
						nav.speed = runSpeed;
						nav.velocity = -Vector3.forward * runSpeed;
					}
				}
				// Move right
				if (Input.GetKey (KeyCode.RightArrow)) {
					nav.speed = walkSpeed;
					nav.velocity = Vector3.right * walkSpeed;
					if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) {
						nav.speed = runSpeed;
						nav.velocity = Vector3.right * runSpeed;
					}
				}
				// Move left
				if (Input.GetKey (KeyCode.LeftArrow)) {
					nav.speed = walkSpeed;
					nav.velocity = -Vector3.right * walkSpeed;
					if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) {
						nav.speed = runSpeed;
						nav.velocity = -Vector3.right * runSpeed;
					}
				}
			} else if (numOfArrowsDown > 1) {
				if (Input.GetKey (KeyCode.UpArrow) && Input.GetKey (KeyCode.LeftArrow)) {
					nav.speed = walkSpeed;
					nav.velocity = Vector3.RotateTowards (gameObject.transform.forward, -gameObject.transform.right, rad, 0.0F) * walkSpeed;
					if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) {
						nav.speed = runSpeed;
						nav.velocity = Vector3.RotateTowards (gameObject.transform.forward, -gameObject.transform.right, rad, 0.0F) * runSpeed;
					}
				}
				if (Input.GetKey (KeyCode.UpArrow) && Input.GetKey (KeyCode.RightArrow)) {
					nav.speed = walkSpeed;
					nav.velocity = Vector3.RotateTowards (gameObject.transform.forward, gameObject.transform.right, rad, 0.0F) * walkSpeed;
					if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) {
						nav.speed = runSpeed;
						nav.velocity = Vector3.RotateTowards (gameObject.transform.forward, gameObject.transform.right, rad, 0.0F) * runSpeed;
					}
				}
				if (Input.GetKey (KeyCode.DownArrow) && Input.GetKey (KeyCode.LeftArrow)) {
					nav.speed = walkSpeed;
					nav.velocity = Vector3.RotateTowards (-gameObject.transform.forward, -gameObject.transform.right, rad, 0.0F) * walkSpeed;
					if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) {
						nav.speed = runSpeed;
						nav.velocity = Vector3.RotateTowards (-gameObject.transform.forward, -gameObject.transform.right, rad, 0.0F) * runSpeed;
					}
				}
				if (Input.GetKey (KeyCode.DownArrow) && Input.GetKey (KeyCode.RightArrow)) {
					nav.speed = walkSpeed;
					nav.velocity = Vector3.RotateTowards (-gameObject.transform.forward, gameObject.transform.right, rad, 0.0F) * walkSpeed;
					if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) {
						nav.speed = runSpeed;
						nav.velocity = Vector3.RotateTowards (-gameObject.transform.forward, gameObject.transform.right, rad, 0.0F) * runSpeed;
					}
				}
			}
		}

		// choosing weapons
		if (!ui.paused && !finish.defeated) {
			if (Input.GetKey (KeyCode.Alpha1)) {
				attachment = (int)weaponAttachment.cannon;
			}
			if (Input.GetKey (KeyCode.Alpha2)) {
				if (egg) {
					attachment = (int)weaponAttachment.egg;
				}
			}
			if (Input.GetKey (KeyCode.Alpha3)) {
				if (pellet) {
					attachment = (int)weaponAttachment.pellet;
				}
			}
			if (Input.GetKey (KeyCode.Alpha4)) {
				if (shot) {
					attachment = (int)weaponAttachment.shot;
				}
			}
		}

		// shooting weapons
		if (Input.GetKey (KeyCode.Space) && !ui.paused && !finish.defeated) {
			//determine which bullet prefab will be used based on active weapon attachment
			Debug.Log ("firing weapon");
			GameObject newBullet;
			
			switch (attachment) {
			case (int)weaponAttachment.cannon:
				if (!hasFired) {
					newBullet = (GameObject)Instantiate (cannonPrefab, transform.position + facing * 3f, transform.localRotation);
					if (newBullet.rigidbody != null) {
						newBullet.rigidbody.AddForce (facing * cannonForce);
					}
					hasFired = !hasFired;
					timer = Time.time + cannonInterval;
				}
				break;
			case (int)weaponAttachment.egg:
				if (egg) {
					if (!hasFired) {
						if (currentEAmmo > 0) {
							newBullet = (GameObject)Instantiate (eggPrefab, transform.position + facing * 3f, transform.localRotation);
							if (newBullet.rigidbody != null) {
								newBullet.rigidbody.AddForce (facing * eggForce);
							}
							hasFired = !hasFired;
							timer = Time.time + eggInterval;
							currentEAmmo--;
						}
					}
				}
				break;
			case (int)weaponAttachment.pellet:
				if (pellet) {
					if (!hasFired) {
						if (currentPAmmo > 0) {
							newBullet = (GameObject)Instantiate (pelletPrefab, transform.position + facing * 3f, transform.localRotation);
							if (newBullet.rigidbody != null) {
								newBullet.rigidbody.AddForce (facing * pelletForce);
							}
							hasFired = !hasFired;
							timer = Time.time + pelletInterval;
							currentPAmmo--;
						}
					}
				}
				break;
			case (int)weaponAttachment.shot:
				if (shot) {
					if (!hasFired) {
						if (currentSAmmo > 0) {
							newBullet = (GameObject)Instantiate (shotPrefab, transform.position + facing * 3f, transform.localRotation);
							if (newBullet.rigidbody != null) {
								newBullet.rigidbody.AddForce (facing * shotForce);
							}
							newBullet = (GameObject)Instantiate (shotPrefab, transform.position + facing * 3f
								+ transform.up * 0.2f, transform.localRotation);
							if (newBullet.rigidbody != null) {
								newBullet.rigidbody.AddForce (facing * shotForce);
							}
							newBullet = (GameObject)Instantiate (shotPrefab, transform.position + facing * 3f
								+ transform.right * 0.2f, transform.localRotation);
							if (newBullet.rigidbody != null) {
								newBullet.rigidbody.AddForce (facing * shotForce);
							}
							newBullet = (GameObject)Instantiate (shotPrefab, transform.position + facing * 3f
								- transform.right * 0.2f, transform.localRotation);
							if (newBullet.rigidbody != null) {
								newBullet.rigidbody.AddForce (facing * shotForce);
							}
							hasFired = !hasFired;
							timer = Time.time + shotInterval;
							currentSAmmo--;
						}
					}
				}
				break;
			}
		}
		if (Time.time >= timer && hasFired) {
			hasFired = !hasFired;
		}

		//changing sprite image based on facing and weapon attachment currently active
		if (!ui.paused && !finish.defeated) {
			switch (attachment) {
			case (int)weaponAttachment.cannon:
				if (facing == gameObject.transform.forward) {
					currentSprite.sprite = spriteListBaseCannon [0];
				} else if (facing == gameObject.transform.right) {
					currentSprite.sprite = spriteListBaseCannon [2];
				} else if (facing == -gameObject.transform.forward) {
					currentSprite.sprite = spriteListBaseCannon [4];
				} else if (facing == -gameObject.transform.right) {
					currentSprite.sprite = spriteListBaseCannon [6];
				} else if (facing == Vector3.RotateTowards (gameObject.transform.forward, gameObject.transform.right, rad, 0.0F)) {
					currentSprite.sprite = spriteListBaseCannon [1];
				} else if (facing == Vector3.RotateTowards (-gameObject.transform.forward, gameObject.transform.right, rad, 0.0F)) {
					currentSprite.sprite = spriteListBaseCannon [3];
				} else if (facing == Vector3.RotateTowards (-gameObject.transform.forward, -gameObject.transform.right, rad, 0.0F)) {
					currentSprite.sprite = spriteListBaseCannon [5];
				} else if (facing == Vector3.RotateTowards (gameObject.transform.forward, -gameObject.transform.right, rad, 0.0F)) {
					currentSprite.sprite = spriteListBaseCannon [7];
				}
				break;
			case (int)weaponAttachment.egg:
				if (facing == gameObject.transform.forward) {
					currentSprite.sprite = spriteListEggSplode [0];
				} else if (facing == gameObject.transform.right) {
					currentSprite.sprite = spriteListEggSplode [2];
				} else if (facing == -gameObject.transform.forward) {
					currentSprite.sprite = spriteListEggSplode [4];
				} else if (facing == -gameObject.transform.right) {
					currentSprite.sprite = spriteListEggSplode [6];
				} else if (facing == Vector3.RotateTowards (gameObject.transform.forward, gameObject.transform.right, rad, 0.0F)) {
					currentSprite.sprite = spriteListEggSplode [1];
				} else if (facing == Vector3.RotateTowards (-gameObject.transform.forward, gameObject.transform.right, rad, 0.0F)) {
					currentSprite.sprite = spriteListEggSplode [3];
				} else if (facing == Vector3.RotateTowards (-gameObject.transform.forward, -gameObject.transform.right, rad, 0.0F)) {
					currentSprite.sprite = spriteListEggSplode [5];
				} else if (facing == Vector3.RotateTowards (gameObject.transform.forward, -gameObject.transform.right, rad, 0.0F)) {
					currentSprite.sprite = spriteListEggSplode [7];
				}
				break;
			case (int)weaponAttachment.pellet:
				if (facing == gameObject.transform.forward) {
					currentSprite.sprite = spriteListPelletNozzle [0];
				} else if (facing == gameObject.transform.right) {
					currentSprite.sprite = spriteListPelletNozzle [2];
				} else if (facing == -gameObject.transform.forward) {
					currentSprite.sprite = spriteListPelletNozzle [4];
				} else if (facing == -gameObject.transform.right) {
					currentSprite.sprite = spriteListPelletNozzle [6];
				} else if (facing == Vector3.RotateTowards (gameObject.transform.forward, gameObject.transform.right, rad, 0.0F)) {
					currentSprite.sprite = spriteListPelletNozzle [1];
				} else if (facing == Vector3.RotateTowards (-gameObject.transform.forward, gameObject.transform.right, rad, 0.0F)) {
					currentSprite.sprite = spriteListPelletNozzle [3];
				} else if (facing == Vector3.RotateTowards (-gameObject.transform.forward, -gameObject.transform.right, rad, 0.0F)) {
					currentSprite.sprite = spriteListPelletNozzle [5];
				} else if (facing == Vector3.RotateTowards (gameObject.transform.forward, -gameObject.transform.right, rad, 0.0F)) {
					currentSprite.sprite = spriteListPelletNozzle [7];
				}
				break;
			case (int)weaponAttachment.shot:
				if (facing == gameObject.transform.forward) {
					currentSprite.sprite = spriteListShotgunRain [0];
				} else if (facing == gameObject.transform.right) {
					currentSprite.sprite = spriteListShotgunRain [2];
				} else if (facing == -gameObject.transform.forward) {
					currentSprite.sprite = spriteListShotgunRain [4];
				} else if (facing == -gameObject.transform.right) {
					currentSprite.sprite = spriteListShotgunRain [6];
				} else if (facing == Vector3.RotateTowards (gameObject.transform.forward, gameObject.transform.right, rad, 0.0F)) {
					currentSprite.sprite = spriteListShotgunRain [1];
				} else if (facing == Vector3.RotateTowards (-gameObject.transform.forward, gameObject.transform.right, rad, 0.0F)) {
					currentSprite.sprite = spriteListShotgunRain [3];
				} else if (facing == Vector3.RotateTowards (-gameObject.transform.forward, -gameObject.transform.right, rad, 0.0F)) {
					currentSprite.sprite = spriteListShotgunRain [5];
				} else if (facing == Vector3.RotateTowards (gameObject.transform.forward, -gameObject.transform.right, rad, 0.0F)) {
					currentSprite.sprite = spriteListShotgunRain [7];
				}
				break;
		
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Portal") {
			PlayerPrefs.SetInt ("PlayerHealth", currentPlayerHealth);
			PlayerPrefs.SetInt ("Attachment", attachment);
			PlayerPrefs.SetInt ("EggAmmo", currentEAmmo);
			PlayerPrefs.SetInt ("PelletAmmo", currentPAmmo);
			PlayerPrefs.SetInt ("ShotAmmo", currentSAmmo);
			PlayerPrefs.SetInt ("Warped", 1);
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "Portal") {
			warped = 0;
		}
	}
}
