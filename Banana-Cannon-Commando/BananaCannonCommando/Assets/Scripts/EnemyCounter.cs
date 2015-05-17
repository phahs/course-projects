using UnityEngine;
using System.Collections;

public class EnemyCounter : MonoBehaviour {

	public int ApplesLeft = 0;
	public int OrangesLeft = 0;
	public int TomatoesLeft = 0;
	public int GrapesLeft = 0;
	public int WatermelonsLeft = 0;

    public int OrpplesLeft = 0;
	public int MachoMatosLeft = 0;
	public int DrRottensLeft = 0;

    public int FruitsLeft = 0;                 // Total Fruits:  enable door to Boss ?
    public int BossesLeft = 0;                 // total Bosses:  enable next Level Portal ?

    public bool FruitGone = false;             // previous cycle: one-shot trigger (see Update)
    public bool BossesGone = false;            // previous cycle: one-shot trigger (see Update)
	public bool finalLevel;						// is this the final level?

	void Start()                // Start this Level ( don't need this?? )
    {
		getCount();
		if ("TheGiantFruitCakeCastle" == PlayerPrefs.GetString ("WhereAmI")) {
			finalLevel = true; // if we are at the fruitcake castle, then we are in the final level
		}

        FruitGone = false;
        BossesGone = false;     // 
    }
    
    void Update()               // Count all "enemies" remaining (this Level)
    {
		getCount();
        //OnGUI();                         // Debug ??? Label

        if ( !FruitGone && FruitsLeft <= 0 )        // Just Destroyed Last Fruit (this Level)
        {
             FruitGone = true;                      // 
             // remove barrier to "Boss" code here: remove barrier object ???
        } 
        if ( !BossesGone && BossesLeft <= 0 )       // Just Destroyed Last Boss (this Level)
        {                                                     
             BossesGone = true;                     // Player has to go to special (Portal) Location ??
             // Enable going to next Level ??? Portal Location
        }

		if (finalLevel) {
			if (DrRottensLeft <= 0) {
				Time.timeScale = 0;				// when dr rotten is defeated pause the game as best we can 
			}
		}
    }

	void getCount()
	{
		GameObject[] apples = GameObject.FindGameObjectsWithTag("AppleE");
		ApplesLeft = apples.Length;
		GameObject[] oranges = GameObject.FindGameObjectsWithTag("OrangeE");
		OrangesLeft = oranges.Length;
		GameObject[] tomatoes = GameObject.FindGameObjectsWithTag("TomatoE");
		TomatoesLeft = tomatoes.Length;
		GameObject[] grapes = GameObject.FindGameObjectsWithTag("GrapeE");
		GrapesLeft = grapes.Length;
		GameObject[] watermelons = GameObject.FindGameObjectsWithTag("WatermelonE");
		WatermelonsLeft = watermelons.Length;
		FruitsLeft = ApplesLeft +OrangesLeft +TomatoesLeft +GrapesLeft +WatermelonsLeft;
		GameObject[] orpples= GameObject.FindGameObjectsWithTag("Orpple");
		OrpplesLeft = orpples.Length;
		GameObject[] machomatos = GameObject.FindGameObjectsWithTag("MachoMato");
		MachoMatosLeft = machomatos.Length;
		GameObject[] rottens= GameObject.FindGameObjectsWithTag("Dr. Rotten");
		DrRottensLeft = rottens.Length;
		BossesLeft = OrpplesLeft +MachoMatosLeft +DrRottensLeft;
	}
	
	void OnGUI()
	{/*
		GUI.Label(new Rect(0,0, 200,20),"FruitsLeft: "+FruitsLeft+"    BossesLeft: "+BossesLeft);
        GUI.Label(new Rect(0,20,400,20),"  A:"+ApplesLeft+"  O:"+OrangesLeft+
                    "  T:"+TomatoesLeft+"  G:"+GrapesLeft+"  W:"+WatermelonsLeft+
                    " ---  O:"+OrpplesLeft+"  M:" +MachoMatosLeft+"  R:"+DrRottensLeft);
*/
		if (finalLevel) {
			if (DrRottensLeft <= 0) {
				// the final display.
				GUI.Box (new Rect (Screen.width/2 - 300, Screen.height/2 - 300, 700, 600), "");
				GUI.Box(new Rect(Screen.width/2 - 150, Screen.height/2 - 260, 400, 220), "");
				GUI.TextField (new Rect (Screen.width / 2 - 150, Screen.height / 2 - 240, 400, 200), 
				               "\tCONGRATULATIONS!\n" +
				               "\tYOU HAVE DEFEATED\n"+
				               "\tDR. ROTTEN\n");

				//Back to main menu button
				if (GUI.Button (new Rect (Screen.width/2 - 50, Screen.height/2, 200, 70), "BACK TO MAIN MENU")) {
					Application.LoadLevel("start screen");
				}
							
				//exit game 
				if (GUI.Button (new Rect (Screen.width/2, Screen.height/2 + 200, 100, 70), "EXIT")) {
					Application.Quit();
				}
			}
		}
    } 
}
