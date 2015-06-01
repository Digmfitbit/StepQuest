using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Net;
using System;


public class ShowroomManagerV2 : MonoBehaviour {
	
	PlayerManager manager;
	int activShowroom = 0;
	PlayerStats activeShowroomStats;
	List<PlayerStats> friends = new List<PlayerStats>();
	List<PlayerStats> showrooms = new List<PlayerStats>();
	GameObject avatar;

	string urlToAvatarPic;
	WebClient webClient = new WebClient();

	// Use this for initialization
	void Awake () 
	{
		//Get all showroom objects and user/friend data
		manager = FindObjectOfType<PlayerManager>();
		
		activeShowroomStats = manager.mainPlayer;
		showrooms.Add (manager.mainPlayer);
		setUpShowroom ();

		friends = manager.fitBitFriends;
		showrooms.AddRange (friends);
	}
	
	void setUpShowroom()
	{
		activeShowroomStats = showrooms[activShowroom];

		if (avatar != null)
			Destroy (avatar);
		//get player avatar in game
		string avatarName = activeShowroomStats.playerClassID;
		if (avatarName != null)
			avatar = Instantiate (Resources.Load (("Prefabs/CharacterPrefabs/" + avatarName)), new Vector3 (0f, 0f, 0f), Quaternion.identity) as GameObject;
		else
			Debug.LogWarning ("Avatar ID: "+activeShowroomStats.playerClassID+" could not be resolved into a avatar name");

		//load avatar pic from FitBit
		StartCoroutine (LoadImage());

		//set player stats
		GameObject.Find ("Text_PlayerName_Val").GetComponent<Text> ().text = activeShowroomStats.playerName;
		GameObject.Find ("Text_Level_Val").GetComponent<Text> ().text = activeShowroomStats.playerLvl.ToString();
		GameObject.Find ("Text_Strength_Val").GetComponent<Text> ().text = activeShowroomStats.playerStrength.ToString();
		GameObject.Find ("Text_CurrentExp_Val").GetComponent<Text> ().text = activeShowroomStats.currentExp.ToString();
		GameObject.Find ("Text_Stamina_Val").GetComponent<Text> ().text = activeShowroomStats.playerStamina.ToString();
		GameObject.Find ("Text_Recovery_Val").GetComponent<Text> ().text = activeShowroomStats.playerRecovery.ToString();
		GameObject.Find ("Text_Endurance_Val").GetComponent<Text> ().text = activeShowroomStats.playerEndurance.ToString();
		GameObject.Find ("Text_ExpToNext_Val").GetComponent<Text> ().text = activeShowroomStats.expToNext.ToString();
	}

	//this is called whenever the right or left button is pressed.
	public void ChangeShowroom(int _i)
	{
		activShowroom += _i;
		if (activShowroom < 0)
			activShowroom = 0;
		if (activShowroom > showrooms.Count-1)
			activShowroom = showrooms.Count-1;

		Debug.Log ("ActiveShowroom" + activShowroom);
		setUpShowroom ();
	}

	//Loads the FitBit pic from the fitBit server.
	IEnumerator LoadImage() 
	{
		urlToAvatarPic = activeShowroomStats.fitbitPictureUrl.Substring (1, activeShowroomStats.fitbitPictureUrl.Length - 2);
		Debug.Log ("URLtoPic:" + urlToAvatarPic);
		WWW www = new WWW(urlToAvatarPic);
		yield return www;
		Rect rec = new Rect(0, 0, www.texture.width, www.texture.height);
		GameObject.Find ("Image_Avatar").GetComponent<Image> ().sprite = Sprite.Create(www.texture, rec, new Vector2(0,0), 1);
	}
}
