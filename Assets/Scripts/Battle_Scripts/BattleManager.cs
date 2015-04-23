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

	private bool fightMode = false;

	private TextMesh fightText;

	
	void Start () 
	{
		//Places the players and enemys on the field
		setUpFight ();

		fightText = GameObject.Find("tempReadyForFightText").GetComponent<TextMesh>();
		fightText.color = new Color(1f,1f,1f,0f);
	}
	
	void Update () 
	{

	}

	public void setSelection(GameObject _fighter)
	{
	
		if (_fighter.tag == "Player") 
		{
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
			if(playerSelected && _fighter.tag == "Enemy")
			{
				selectedEnemy = _fighter;
				enemySelected = true;
				selectedEnemy.SendMessage("SetSelected", true);
			}else{
				Debug.Log("Select Fighter First");
			}
		}

		//After a player and enemy are selected the game goes into fight mode
		if (playerSelected && enemySelected) 
		{
			fightMode = true;
			fightText.color = new Color(1f,1f,1f,1f);		
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
