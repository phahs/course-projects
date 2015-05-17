using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

	public int maxAppleHealth;
	public int maxOrangeHealth;
	public int maxTomatoHealth;
	public int maxGrapeHealth;
	public int maxWatermelonHealth;
	public int maxOrppleHealth;
	public int maxMachoMatoHealth;
	public int maxDrRottenHealth;
	public int currentHealth;
	public GameObject enemy;

	void Start()
	{
		maxAppleHealth = 50;
		maxOrangeHealth = 60;
		maxTomatoHealth = 80;
		maxGrapeHealth = 1;
		maxWatermelonHealth = 100;
		maxOrppleHealth = 160;
		maxMachoMatoHealth = 200;
		maxDrRottenHealth = 400;
	}

	void Awake ()
	{
		if (enemy.tag == "AppleE") {
			currentHealth = maxAppleHealth;
		}
		if (enemy.tag == "OrangeE") {
			currentHealth = maxOrangeHealth;
		}
		if (enemy.tag == "TomatoE") {
			currentHealth = maxTomatoHealth;
		}
		if (enemy.tag == "GrapeE") {
			currentHealth = maxGrapeHealth;
		}
		if (enemy.tag == "WatermelonE") {
			currentHealth = maxWatermelonHealth;
		}
		if (enemy.tag == "Orpple") {
			currentHealth = maxOrppleHealth;
		}
		if (enemy.tag == "MachoMato") {
			currentHealth = maxMachoMatoHealth;
		}
		if (enemy.tag == "Dr. Rotten") {
			currentHealth = maxDrRottenHealth;
		}
	}
}
