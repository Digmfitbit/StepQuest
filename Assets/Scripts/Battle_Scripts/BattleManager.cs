using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour {
	
	public GameObject[] players;
	public GameObject[] enemys;

	private GameObject selectedPlayer = null;
	private GameObject selectedEnemy = null;
	private bool playerSelected = false;
	private bool enemySelected = false;
	public int numberOfPlayer = 1;
	public int numberOfEnemy = 1;

	private List<GameObject> allPlayers = new List<GameObject>();
	private List<GameObject> allEnemys = new List<GameObject>();

	private Player playerScript;
	private Enemy enemyScript;

	private bool fightMode = false;
	private bool startFight = false;

	private TextMesh fightText;

	
	void Awake () 
	{
		//Places the players and enemys on the field
		setUpFight ();

		fightText = GameObject.Find("tempReadyForFightText").GetComponent<TextMesh>();
	}
	
	void Update () 
	{
		if (startFight) 
		{
			startFight = false;
			Fight();
		}
	}

	private void Fight()
	{
		Debug.Log ("Fight between: " + selectedPlayer.name + " vs. " + selectedEnemy.name);
		playerScript.attack (selectedEnemy);
		enemyScript.attack (selectedPlayer);
		Debug.Log ("PlayerHealth: " + playerScript.health + " EnemyHealth: " + enemyScript.health);

		//set up next round
		fightMode = false;
		selectedEnemy.SendMessage ("SetSelected", false);
		selectedEnemy = null;
		enemySelected = false;
	}

	public void setSelection(GameObject _fighter)
	{
		//selection is only possible in if we are not in fight mode. 
		if (!fightMode) 
		{
			if (_fighter.tag == "Player") {
				if (selectedPlayer != null) {
					if (selectedPlayer == _fighter) {
						selectedPlayer.SendMessage ("SetSelected", false);
						selectedPlayer = null;
						playerSelected = false;
					} else {
						//tell the player selected befor that he is no longer selected
						selectedPlayer.SendMessage ("SetSelected", false);

						//set newly selected player to be the selected fighter
						selectedPlayer = _fighter;
						selectedPlayer.SendMessage ("SetSelected", true);
					}
				} else {
					selectedPlayer = _fighter;
					playerSelected = true;
					selectedPlayer.SendMessage ("SetSelected", true);
				}
			} else {
				if (playerSelected && _fighter.tag == "Enemy") {
					selectedEnemy = _fighter;
					enemySelected = true;
					selectedEnemy.SendMessage ("SetSelected", true);
				} else {
					Debug.Log ("Select Fighter First");
				}
			}
		

			//After player and enemy are selected the game goes into fight mode
			if (playerSelected && enemySelected) {
				fightMode = true;
				startFight = true;
				fightText.text = "Fight!";

				playerScript = selectedPlayer.GetComponent<Player> ();
				enemyScript = selectedEnemy.GetComponent<Enemy> ();
			}
		}
	}

	public void someoneDied(GameObject _deadFighter)
	{
		if (_deadFighter.tag == "Enemy") {
			enemySelected = false;
			selectedEnemy = null;

			fightMode = false;
		} 
		else if (_deadFighter.tag == "Player") 
		{
			enemySelected = false;
			playerSelected = false;
			selectedEnemy = null;
			selectedPlayer = null;

			fightMode = false;
		}
	}

	private void setUpFight()
	{
		Vector3 scaleUp = new Vector3 (2, 2, 1);

		for (int i = 0; i < numberOfPlayer; i++) 
		{
			GameObject toInstantiatePlayer = players [0];
			toInstantiatePlayer.name = "Player_"+i;
			toInstantiatePlayer.transform.localScale = scaleUp;
			GameObject instancePlayer = Instantiate (toInstantiatePlayer, new Vector2 (Random.Range (-1f, -5f), Random.Range (-3f, 3f)), Quaternion.identity) as GameObject;
			allPlayers.Add(instancePlayer);
		}

		for(int i = 0; i < numberOfEnemy; i++)
		{
			GameObject toInstantiateEnemy = enemys [0];
			toInstantiateEnemy.name = "Enemy_"+i;
			toInstantiateEnemy.transform.localScale = scaleUp;
			GameObject instanceEnemy = Instantiate (toInstantiateEnemy, new Vector2 (Random.Range(1f,5f), Random.Range(-3f,3f)), Quaternion.identity) as GameObject;
			allEnemys.Add(instanceEnemy);
		}
	
	}
}
