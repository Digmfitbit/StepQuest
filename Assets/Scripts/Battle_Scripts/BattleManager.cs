using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour {
	
	public GameObject[] players;
	public GameObject[] enemys;

	private GameObject selectedPlayer = null;
	private GameObject selectedEnemy = null;
	private Player playerScript;
	private Enemy enemyScript;
	public int numberOfPlayer = 1;
	public int numberOfEnemy = 1;

	private List<GameObject> allPlayers = new List<GameObject>();
	private List<GameObject> allEnemys = new List<GameObject>();
	
	private bool playerSelected = false;
	private bool enemySelected = false;
	private bool battleOver = false;
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
		//if everthing is ready to fight start fight
		if (startFight) {
			startFight = false;
			Fight ();
		}

		Debug.DrawLine (new Vector3(5,5,0), new Vector3(0,0,0), Color.red);

		//Check if the battle is over. Battle is over when all enemys or player are dead.
		if (!battleOver)
		{
			//check if enemies left, if not call win function at all left over players
			if (allEnemys.Count <= 0)
			{
				foreach (GameObject player in allPlayers) 
				{
					player.SendMessage("WinFight");
				}
				battleOver = true;
				fightText.text = "You Win!";
			}

			//do the same for all players, if no player is left call win function on all enemys alive
			if (allPlayers.Count <= 0) 
			{
				foreach(GameObject enemy in allPlayers)
				{
					enemy.SendMessage("WinFight");
				}
				battleOver = true;
				fightText.text = "You Suck!";
			}
		}
	}

	private void Fight()
	{
		//Manual fight sequence
//		Debug.Log ("Fight between: " + selectedPlayer.name + " vs. " + selectedEnemy.name);
		if(playerSelected && fightMode)
			playerScript.attack (selectedEnemy);

		if(enemySelected && fightMode)
			enemyScript.attack (selectedPlayer);
//		Debug.Log ("PlayerHealth: " + playerScript.health + " EnemyHealth: " + enemyScript.health);


		//Automated fight sequence
		foreach(GameObject _player in allPlayers )
		{
			// all player except the selected fight
			if(_player != selectedPlayer)
			{
				//pick a random alive enemy and attack them
				if(allEnemys.Count > 0)
				{
					GameObject randomEnemy = allEnemys[Random.Range(0, allEnemys.Count)];
					_player.SendMessage("attack",randomEnemy);
				}
			}
		}

		//Enemy fight back
		//Get a random enemy and let them attack a random player
		foreach (GameObject _enemy in allEnemys) 
		{
			if(_enemy != selectedEnemy)
			{
				if(allPlayers.Count > 0)
				{
					GameObject randomEnemyToFightBack = allEnemys[Random.Range(0, allEnemys.Count)];
					GameObject randomPlayer = allPlayers[Random.Range(0, allPlayers.Count)];
					randomEnemyToFightBack.SendMessage("attack",randomPlayer);
				}
			}
		}


		//set up next round
		fightMode = false;
		if (selectedEnemy != null) 
		{
			selectedEnemy.SendMessage ("SetSelected", false);
			selectedEnemy = null;
		}
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
			_deadFighter.SendMessage ("SetSelected", false);
			allEnemys.Remove(_deadFighter);

			if(_deadFighter == selectedEnemy)
			{
				enemySelected = false;
				selectedEnemy = null;
				fightMode = false;
			}
		} 
		else if (_deadFighter.tag == "Player") 
		{
			_deadFighter.SendMessage ("SetSelected", false);
			allPlayers.Remove(_deadFighter);

			if(_deadFighter == selectedPlayer)
			{
				enemySelected = false;
				playerSelected = false;
				selectedEnemy = null;
				selectedPlayer = null;
				fightMode = false;
			}
		}
	}

	private void setUpFight()
	{
		//Scle of the fighters
		Vector3 scaleUp = new Vector3 (2, 2, 1);

		for (int i = 0; i < numberOfPlayer; i++) 
		{
			GameObject toInstantiatePlayer = players [0];
			toInstantiatePlayer.name = "Player_"+i;
			toInstantiatePlayer.transform.localScale = scaleUp;
			GameObject instancePlayer = Instantiate (toInstantiatePlayer, new Vector2 (- i-1, Random.Range(-2f,2f)), Quaternion.identity) as GameObject;
			allPlayers.Add(instancePlayer);
		}

		for(int i = 0; i < numberOfEnemy; i++)
		{
			GameObject toInstantiateEnemy = enemys [0];
			toInstantiateEnemy.name = "Enemy_"+i;
			toInstantiateEnemy.transform.localScale = scaleUp;
			GameObject instanceEnemy = Instantiate (toInstantiateEnemy, new Vector2 (i+1, Random.Range(-3f,3f)), Quaternion.identity) as GameObject;
			allEnemys.Add(instanceEnemy);
		}
	
	}
}
