using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class InizializationManager : MonoBehaviour {

	PlayerStats mainPlayerStats;
	string selectedCharacter = null;
	string playerName = null;
	public InputField inputPlayerName = null;
	bool allowWriteToPlayerStats = false;

	void Awake () 
	{
		mainPlayerStats = PlayerManager.mainPlayer;
	}

	void Update()
	{
		if (playerName != null && selectedCharacter != null)
			allowWriteToPlayerStats = true;
	}

	public void SetSelected(string _name)
	{
		selectedCharacter = _name;
		Debug.Log (selectedCharacter + "was selected!");
	}
	
	public void GetPlayerName()
	{
		playerName = inputPlayerName.text;
		Debug.Log (playerName);
	}

	public void WriteToPlayerStats()
	{
		if (allowWriteToPlayerStats) 
		{
			mainPlayerStats.playerClassID = selectedCharacter;
			mainPlayerStats.playerName = playerName;
		} 
		else 
		{
			Debug.Log("Pick character and player name befor continuing.");
		}
	}
}
