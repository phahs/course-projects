// code written by Jake
// written August 16, 2013
// http://www.mybringback.com/unity/14055/unity-2d-game-development-15-creating-level-navigation-system/
// Modified by Philip Hahs
// Modified April 8, 2015

#pragma strict

var Level = "";
var player : GameObject;
//var Destination : Transform[];
var posx : float;
var posz : float;

function Start ()
{
	player = GameObject.FindGameObjectWithTag("Player");
}

function OnTriggerStay(other : Collider)
{
	if(other.tag == "Player")
	{
		//	posx = Destination.position.x;
		//	posz = Destination.position.z;
			if(Level == "TheOrchard")
			{
					PlayerPrefs.SetFloat("FacingX", -1);
					PlayerPrefs.SetFloat("FacingZ", 0);
					PlayerPrefs.SetString("WhereAmI", "TheOrchard");
					
		//		PlayerPrefs.SetFloat("FruitopiaPosX", posx);
		//		PlayerPrefs.SetFloat("FruitopiaPosZ", posz);
			}
			if(Level == "TheSuperMarket")
			{
					PlayerPrefs.SetFloat("FacingX", 0);
					PlayerPrefs.SetFloat("FacingZ", 1);
					PlayerPrefs.SetString("WhereAmI", "TheSuperMarket");
					
		//			PlayerPrefs.SetFloat("FruitopiaPosX", posx);
		//			PlayerPrefs.SetFloat("FruitopiaPosZ", posz);
			}

			if(Level == "TheGiantFruitCakeCastle")
			{
					PlayerPrefs.SetFloat("FacingX", 0);
					PlayerPrefs.SetFloat("FacingZ", 1);
					PlayerPrefs.SetString("WhereAmI", "TheGiantFruitCakeCastle");
					
		//			PlayerPrefs.SetFloat("FruitopiaPosX", posx);
		//			PlayerPrefs.SetFloat("FruitopiaPosZ", posz);
			}
			if(Level == "ThePark")
			{
				PlayerPrefs.SetFloat("FacingX", 0);
				PlayerPrefs.SetFloat("FacingZ", 1);
				PlayerPrefs.SetString("WhereAmI", "ThePark");
			}
			if(Level == "Fruitopia2")
			{
				
				if( "TheOrchard" == PlayerPrefs.GetString("WhereAmI"))
				{
					PlayerPrefs.SetFloat("FacingX", 0);
					PlayerPrefs.SetFloat("FacingZ", -1);
				}
				PlayerPrefs.SetString("WhereAmI", "Fruitopia");
			}
			
			if(Level == "Fruitopia3")
			{
				
				if( "TheSuperMarket" == PlayerPrefs.GetString("WhereAmI"))
				{
					PlayerPrefs.SetFloat("FacingX", 0);
					PlayerPrefs.SetFloat("FacingZ", -1);
				}
				PlayerPrefs.SetString("WhereAmI", "Fruitopia");
			}
			if(Level == "Fruitopia4")
			{
				
				if( "ThePark" == PlayerPrefs.GetString("WhereAmI"))
				{
					PlayerPrefs.SetFloat("FacingX", 0);
					PlayerPrefs.SetFloat("FacingZ", -1);
				}
				PlayerPrefs.SetString("WhereAmI", "Fruitopia");
			}
			if(Level == "Fruitopia5")
			{
				
				if( "TheFruitCakeCastle" == PlayerPrefs.GetString("WhereAmI"))
				{
					PlayerPrefs.SetFloat("FacingX", 1);
					PlayerPrefs.SetFloat("FacingZ", 0);
				}
				PlayerPrefs.SetString("WhereAmI", "Fruitopia");
			}

			//load multiple levels
			Application.LoadLevel(Level);
	}
}
/*function OnTriggerEnter (other : Collider) {
	if (other == player.collider) {
  
 	Application.LoadLevel (Level);    
 	}
}
function OnLevelWasLoaded(){
 
//	player.transform.position = Destination.position;
	if(PlayerPrefs.GetString("WhereAmI") == "TheOrchard"){
		player.transform.position.Set(Destination[0].position.x, 1f, Destination[0].position.z);
		}
	if(PlayerPrefs.GetString("WhereAmI") == "TheSuperMarket"){
		player.transform.position.Set(Destination[0].position.x, 1f, Destination[0].position.z);
		}
	if(PlayerPrefs.GetString("WhereAmI") == "TheFruitCakeCastle"){
		player.transform.position.Set(Destination[0].position.x, 1f, Destination[0].position.z);
		}
	if(PlayerPrefs.GetString("WhereAmI") == "ThePark"){
		player.transform.position.Set(Destination[0].position.x, 1f, Destination[0].position.z);
		}

} */
