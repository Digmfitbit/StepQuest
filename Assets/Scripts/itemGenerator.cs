using UnityEngine;
using System.Collections;

public class itemGenerator : MonoBehaviour {

	private string[] itemTypes = new string[]{"Weapon", "Armor"};
	public string itemType;

	private string[] adjectives = new string[]{"Adorable", "Beautiful", "Clean", "Drab", "Elegant", "Fancy", "Glamorous", "Handsome", "Long", "Magnificent", "Old-Fashioned", "Plain", "Quaint", "Sparkling", "Ugliest", "Unsightly", "Wide-Eyed", "Red", "Orange", "Yellow", "Green", "Blue", "Purple", "Gray", "Black", "White", "Alive", "Better", "Careful", "Clever", "Dead", "Easy", "Famous", "Gifted", "Helpful", "Important", "Inexpensive", "Mushy", "Odd", "Powerful", "Rich", "Shy", "Tender", "Uninterested", "Vast", "Wrong", "Angry", "Bewildered", "Clumsy", "Defeated","Embarrassed","Fierce","Grumpy","Helpless","Itchy","Jealous","Lazy","Mysterious","Nervous","Obnoxious","Panicky","Repulsive","Scary","Thoughtless","Uptight","Worried","Agreeable","Brave","Calm","Delightful","Eager","Faithful","Gentle","Happy","Jolly","Kind","Lively","Nice","Obedient","Proud","Relieved","Silly","Thankful","Victorious","Witty","Zealous","Broad","Chubby","Crooked","Curved","Deep","Flat","High","Hollow","Low","Narrow","Round","Shallow","Skinny","Square","Steep","Straight","Wide","Big","Colossal","Fat","Gigantic","Great","Huge","Immense","Large","Little","Mammoth","Massive","Miniature","Petite","Puny","Scrawny","Short","Small","Tall","Teeny","Teeny-Tiny","Tiny","Cooing","Deafening","Faint","Hissing","Loud","Melodic","Noisy","Purring","Quiet","Raspy","Screeching","Thundering","Voiceless","Whispering","Ancient","Brief","Early","Fast","Late","Long","Modern","Old","Old-Fashioned","Quick","Rapid","Short","Slow","Swift","Young","Bitter","Delicious","Fresh","Greasy","Juicy","Hot","Icy","Loose","Melted","Nutritious","Prickly","Rainy","Rotten","Salty","Sticky","Strong","Sweet","Tart","Tasteless","Uneven","Weak","Wet","Wooden","Yummy","Boiling","Broken","Bumpy","Chilly","Cold","Cool","Creepy","Crooked","Cuddly","Curly","Damaged","Damp","Dirty","Dry","Dusty","Filthy","Flaky","Fluffy","Freezing","Hot","Warm","Wet","Abundant","Empty","Few","Full","Heavy","Light","Substantial"};
	public string adjective;

	private string[] weaponTypes = new string[]{"Sabre", "Katana", "Flamberge", "Mace", "Club", "Battle Axe", "Quarterstaff", "Katana", "Pike", "Spetum", "Partisan", "Lance", "Crossbow", "Longbow", "Sling", "Shortsword", "Dagger", "Axe", "Pistol", "Machine Gun", "Hammer", "Flail"};
	public string weaponType;

	private string[] armorTypes = new string[]{"Shirt", "Breastplate", "Chainmail", "Vest", "Jacket"};
	public string armorType;

	public string itemName;

	public int[] statBoost;

	public int strBoost;
	public int stamBoost;
	public int endBoost;
	public int recBoost;

	public int seed;
	private System.Random rand;

	public GameObject player;

	void Awake () {
		player = GameObject.Find ("Player");

		seed = (int)gameObject.GetComponent<branchMapGen>().stepCost + (int)gameObject.GetComponent<branchMapGen>().u_id + GameObject.Find ("mapGen").GetComponent<mapGen>().seed;
		rand = new System.Random(seed);

		itemType = itemTypes[rand.Next (0,itemTypes.Length)];
		adjective = adjectives[rand.Next (0,adjectives.Length)];

		switch(itemType){
		case "Weapon":
			weaponType = weaponTypes[rand.Next (0,weaponTypes.Length)];
			statBoost = new int[]{rand.Next (0,10), rand.Next (0,10), rand.Next (0,3), rand.Next(0,4)};
			/*
			strBoost = rand.Next (0,10);
			stamBoost = rand.Next (0,10);
			endBoost = rand.Next (0,3);
			recBoost = rand.Next (0,4);
			*/
			itemName = adjective + " " + weaponType;
			break;
		case "Armor":
			armorType = armorTypes[rand.Next(0, armorTypes.Length)];
			strBoost = rand.Next (0,3);
			stamBoost = rand.Next (0,4);
			endBoost = rand.Next (0,10);
			recBoost = rand.Next (0,10);
			itemName = adjective + " " + armorType;
			break;
		default:
			break;
		}
	}

	public void GrantItem () {
		switch(itemType){
		case "Weapon":
			if(player.GetComponent<playerInv>().weaponName != "nothing"){
				Debug.Log ("There is already a weapon");
			}
			else{
				player.GetComponent<playerInv>().weaponName = itemName;
				//player.
			}
			break;
		case "Armor":
			break;
		}
	}
}
