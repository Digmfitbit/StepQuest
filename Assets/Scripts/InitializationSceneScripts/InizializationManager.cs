using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Assets.Scripts.networking;

public class InizializationManager : MonoBehaviour {

	string selectedCharacter = null;
	string playerName = null;
	public InputField inputPlayerName = null;
	bool allowWriteToPlayerStats = false;

	void Update()
	{
		if (playerName != null && selectedCharacter != null)
		{
			allowWriteToPlayerStats = true;
		}
		else 
		{
			allowWriteToPlayerStats = false;
		}
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
			PlayerManager.mainPlayer.playerClassID = selectedCharacter;
			PlayerManager.mainPlayer.playerName = playerName;
			DatabaseController.updatePlayer(PlayerManager.mainPlayer);
		} 
		else 
		{
			Debug.Log("Pick character and player name befor continuing.");
		}
	}
}
