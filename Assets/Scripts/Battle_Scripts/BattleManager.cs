using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour {
	
	public GameObject[] players;
	public GameObject[] friends;
	public GameObject[] enemys;

    public GameObject attackBar;
    private bool comboOver = false;

	private GameObject selectedPlayer = null;
	private GameObject selectedEnemy = null;
	private Player playerScript;
	private Enemy enemyScript;
	public int numberOfFriends = 1;
	private int numberOfEnemy;

	private List<GameObject> allFriends = new List<GameObject>();
	private List<GameObject> allEnemys = new List<GameObject>();
	
	private bool playerSelected = false;
	private bool playerDead = false;
	private bool enemySelected = false;
	private bool battleOver = false;
	private bool fightMode = false;
	private bool startFight = false;
	private bool inRound = false;

	private bool playerstatsAdded = false;

	private int roundNr = 0;


	private TextMesh textObject;

	public float attackDuration = 0.5f;
	
	void Awake () 
	{
		//Places the players and enemys on the field
		setUpFight (playerStats.playerLvl);

		textObject = GameObject.Find("tempReadyForFightText").GetComponent<TextMesh>();


	}
	
	void Update () 
	{
		//if everthing is ready to fight start fight
		if (startFight) 
		{
			startFight = false;
			Fight ();
		}

		if (playerDead && !inRound && !battleOver) 
		{
			inRound = true;
			StartCoroutine (StartAttacks ());
		}
	
		//Check if the battle is over. Battle is over when all enemys or player are dead.
		if (!battleOver)
		{
			//check if enemies left, if not call win function at all left over players
			if (allEnemys.Count <= 0)
			{
				foreach (GameObject player in allFriends) 
				{
					player.SendMessage("WinFight");
				}
				battleOver = true;
				textObject.text = "You Win!";
			}

			//do the same for all players, if no player is left call win function on all enemys alive
			if (allFriends.Count <= 0 && playerDead) 
			{
				foreach(GameObject enemy in allFriends)
				{
					enemy.SendMessage("WinFight");
				}
				battleOver = true;
				textObject.text = "You Suck!";
			}
		}

		if (battleOver) 
		{
			//Show Ui panel when battle is over

			if(!playerstatsAdded)
			{
				playerstatsAdded = true;
				GameObject.Find("StatsManager").GetComponent<StatsManager>().AddPlayerExperiences();
			}
		}
	}

	private void Fight()
	{
		inRound = true;
		StartCoroutine( StartAttacks ());
	}

	private IEnumerator StartAttacks()
	{
		roundNr += 1;
		textObject.text = "Round: " + roundNr;

		//Manual fight sequence
		if(playerSelected && fightMode && !playerDead)
			playerScript.StartAttack (selectedEnemy);

        // wait to finish round until player's combo is over
        while (!comboOver)
            yield return null;

		//Automated fight sequence
		foreach(GameObject _player in allFriends )
		{
			// all player except the selected fight
			if(_player != selectedPlayer)
			{
				//pick a random alive enemy and attack them
				if(allEnemys.Count > 0)
				{
					yield return new WaitForSeconds(attackDuration);
					GameObject randomEnemy = allEnemys[Random.Range(0, allEnemys.Count)];
					_player.SendMessage("StartAttack",randomEnemy);
				}
			}
		}
		
		//Enemy fight back
		//Let every Enemy StartAttack a random Player
		foreach (GameObject _enemy in allEnemys) 
		{
			if(allFriends.Count > 0)
			{
				yield return new WaitForSeconds(attackDuration);
				GameObject randomPlayer = allFriends[Random.Range(0, allFriends.Count)];
				_enemy.SendMessage("StartAttack",randomPlayer);
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
        comboOver = false;
		inRound = false;
	}

	public void setSelection(GameObject _fighter)
	{
		//selection is only possible if we are not in fight mode. 
		if (!fightMode)
		{

			if (_fighter.tag == "Enemy")
			{
				if (selectedEnemy == null) 
				{
					selectedEnemy = _fighter;
					selectedEnemy.SendMessage ("SetSelected", true);
					enemySelected = true;
				}
				else 
				{
					selectedEnemy.SendMessage ("SetSelected", false);
					selectedEnemy = _fighter;
					selectedEnemy.SendMessage ("SetSelected", true);
					enemySelected = true;
				}
			}


//		{
//			if (_fighter.tag == "Friend") {
//				if (selectedPlayer != null) {
//					if (selectedPlayer == _fighter) {
//						selectedPlayer.SendMessage ("SetSelected", false);
//						selectedPlayer = null;
//						playerSelected = false;
//					} else {
//						//tell the player selected befor that he is no longer selected
//						selectedPlayer.SendMessage ("SetSelected", false);
//
//						//set newly selected player to be the selected fighter
//						selectedPlayer = _fighter;
//						selectedPlayer.SendMessage ("SetSelected", true);
//					}
//				} else {
//					selectedPlayer = _fighter;
//					playerSelected = true;
//					selectedPlayer.SendMessage ("SetSelected", true);
//				}
//			} else {
//				if (playerSelected && _fighter.tag == "Enemy") {
//					selectedEnemy = _fighter;
//					enemySelected = true;
//					selectedEnemy.SendMessage ("SetSelected", true);
//				} else {
//					Debug.Log ("Select Fighter First");
//				}
//			}
		

			//After player and enemy are selected the game goes into fight mode
			if (enemySelected) 
			{
				fightMode = true;
				startFight = true;
				textObject.text = "Fight!";

				playerScript = selectedPlayer.GetComponent<Player> ();
				enemyScript = selectedEnemy.GetComponent<Enemy> ();
			}
		}
	}

	public void someoneDied(GameObject _deadFighter)
	{
		if (_deadFighter.tag == "Enemy") 
		{
			_deadFighter.SendMessage ("SetSelected", false);

			if (_deadFighter == selectedEnemy)
			{
				enemySelected = false;
				selectedEnemy = null;
				fightMode = false;
			}
			allEnemys.Remove (_deadFighter);
		} 
		else if (_deadFighter.tag == "Friend") 
		{
			_deadFighter.SendMessage ("SetSelected", false);
			if (selectedEnemy != null)
				selectedEnemy.SendMessage ("SetSelected", false);

			allFriends.Remove (_deadFighter);

//			if(_deadFighter == selectedPlayer)
//			{
//				enemySelected = false;
//				selectedEnemy = null;
//				selectedPlayer = null;
//				fightMode = false;
//			}
		} else if (_deadFighter.tag == "Player") 
		{
			playerDead = true;
			playerSelected = false;
			allFriends.Remove(_deadFighter);
		}
	}

	private void setUpFight(float _playerLevel)
	{
		//Scle of the fighters
		Vector3 scaleUp = new Vector3 (2, 2, 1);
		Transform fightSceneHolder = GameObject.Find("FightSceneHolder").transform;
        attackBar.SendMessage("Reset");

		//determin number of Enemys by player level
		if (numberOfEnemy <= 0) {
			numberOfEnemy = 2;
		} else {
			numberOfEnemy = Mathf.RoundToInt (_playerLevel * 2);
		}

		//Make all the Fighters
		for (int i = 0; i < 1; i++) 
		{
			GameObject toInstantiatePlayer = players [0];
			toInstantiatePlayer.tag = "Player";
			toInstantiatePlayer.transform.localScale = scaleUp;
			GameObject instancePlayer = Instantiate (toInstantiatePlayer, new Vector2 (- i-1, Random.Range(-2f,2f)), Quaternion.identity) as GameObject;
			instancePlayer.transform.parent = fightSceneHolder;
            instancePlayer.SendMessage("SetAttackBar", attackBar);
			selectedPlayer = instancePlayer;
			playerSelected = true;
			selectedPlayer.SendMessage("SetSelected", true);
			allFriends.Add(instancePlayer);
		}

		for (int i = 0; i < numberOfFriends; i++) 
		{
			GameObject toInstantiateFriend = friends [0];
			toInstantiateFriend.name = "Friends_"+i;
			toInstantiateFriend.tag = "Friend";
			toInstantiateFriend.transform.localScale = scaleUp;
			GameObject instanceFriend = Instantiate (toInstantiateFriend, new Vector2 (- i-2, Random.Range(-2f,2f)), Quaternion.identity) as GameObject;
			allFriends.Add(instanceFriend);
			instanceFriend.transform.parent = fightSceneHolder;
		}

		for(int i = 0; i < numberOfEnemy; i++)
		{
			GameObject toInstantiateEnemy = enemys [0];
			toInstantiateEnemy.name = "Enemy_"+i;
			toInstantiateEnemy.transform.localScale = scaleUp;
			GameObject instanceEnemy = Instantiate (toInstantiateEnemy, new Vector2 (i+1, Random.Range(-3f,3f)), Quaternion.identity) as GameObject;
			allEnemys.Add(instanceEnemy);
			instanceEnemy.transform.parent = fightSceneHolder;
		}
	
	}

    private void SetComboOver(bool c)
    {
        comboOver = c;
    }
}
