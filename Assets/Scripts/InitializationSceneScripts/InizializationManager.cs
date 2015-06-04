using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Assets.Scripts.networking;
using ResponseObjects;
using Assets.Scripts.fitbit;

public class InizializationManager : MonoBehaviour {

	string selectedCharacter = null;
	string playerName = null;
	public InputField inputPlayerName = null;
	bool allowWriteToPlayerStats = false;

    void Start()
    {
        FriendModel model = FitBit.getInstance().getUserModel();
        PlayerManager.mainPlayer = new PlayerStats(model);
        DatabaseController.updatePlayer(PlayerManager.mainPlayer);
    }

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
            PlayerPrefs.SetInt(LoadGame.PLAYED_BEFORE, 1);
		} 
		else 
		{
			Debug.Log("Pick character and player name befor continuing.");
		}
	}
}
