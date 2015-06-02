using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class InizializationManager : MonoBehaviour {

	PlayerStats mainPlayer;
	string selectedCharacter = null;
	string playerName = null;
	public InputField inputPlayerName = null;

	void Awake () 
	{
		mainPlayer = FindObjectOfType<PlayerManager> ().mainPlayer;
	}

	public void SetSelected(string _name)
	{
		selectedCharacter = _name;
		Debug.Log (selectedCharacter + "was selected!");
	}

	public void SetEverthing()
	{
		if (selectedCharacter != null)
			mainPlayer.playerClassID = selectedCharacter;
		else
			Debug.LogWarning ("Select a Player befor continuing");

		if(player
	}

	public void GetPlayerName()
	{
		playerName = inputPlayerName.text;
		Debug.Log (playerName);
	}
}
