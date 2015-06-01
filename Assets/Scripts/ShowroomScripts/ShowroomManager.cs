using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShowroomManager : MonoBehaviour {

	public GameObject friendShowroom;
	public GameObject mainShowroom;
	public GameObject playerShowroom;
	List<GameObject> allShowrooms = new List<GameObject>();
	List<GameObject> friendShowrooms;
	private GameObject showroomParent;
	int numberOfShowrooms;
	float spaceBetweenShowrooms = 10f;
	PlayerManager manager;

	public int numberOfFriends = 5;

	// Use this for initialization
	void Awake () 
	{
		//Get all showroom objects and user/friend data
		manager = FindObjectOfType<PlayerManager>();

		//min number of showrooms is two (Main showroom and player showroom)
		if (manager != null)
			numberOfShowrooms = 2 + manager.fitBitFriends.Count;
		else
			numberOfShowrooms = 5;

		//Set up the start layout of the bar
		showroomParent = new GameObject("ShowroomParent");
		showroomParent.AddComponent<LeftRightSwipe>();
		showroomParent.AddComponent<BoxCollider2D> ();
		showroomParent.GetComponent<BoxCollider2D> ().size = new Vector2(numberOfShowrooms * spaceBetweenShowrooms, 100f);
		showroomParent.GetComponent<BoxCollider2D> ().offset = new Vector2 (numberOfFriends * spaceBetweenShowrooms / 2 + spaceBetweenShowrooms/2, 0f);
		setUpShowroom ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void setUpShowroom()
	{
		float spaceBetweenShowrooms = 10f;

		for (int i = 0; i < numberOfShowrooms; i++) {
		
			GameObject toInstantiate = null;

			if (i == 0)
			{
				toInstantiate = Instantiate (mainShowroom, new Vector2 (0f, 0f), Quaternion.identity) as GameObject;
			}
			else if (i == 1)
			{
				//get the right playerPrefab to instantiate
				string playerPrefab = null;
				if(manager == null)
					Debug.LogWarning("Player manager was not yet initialized. Using default character!");
				else
					playerPrefab = manager.mainPlayer.playerClassID;
				if(playerPrefab == null)
					playerPrefab = "character_01";

				toInstantiate = Instantiate (playerShowroom, new Vector2 (spaceBetweenShowrooms, 0f), Quaternion.identity) as GameObject;
				GameObject player = Instantiate(Resources.Load("Prefabs/CharacterPrefabs/" + playerPrefab,typeof(GameObject)),new Vector3 (spaceBetweenShowrooms, 0f, -1f) , Quaternion.identity) as GameObject;
				player.GetComponent<ScaleMe>().ScaleGameObject(0.5f);
				player.transform.SetParent(toInstantiate.transform);
			}
			else if(i > 1)
			{
				toInstantiate = Instantiate(friendShowroom, new Vector2(i * spaceBetweenShowrooms, 0f), Quaternion.identity) as GameObject;

				//randomly select a character for now, ToDo get the right character of the friends
				int selecter = Random.Range(0,3);
				string character = "character_01";
				if(selecter == 0)
					character = "character_01";
				if(selecter == 1)
					character = "character_02";
				if(selecter == 2)
					character = "character_03";
				if(selecter == 3)
					character = "character_04";

				GameObject friend = Instantiate(Resources.Load("Prefabs/CharacterPrefabs/"+character,typeof(GameObject)),new Vector3 (i * spaceBetweenShowrooms, 0f, -1f) , Quaternion.identity) as GameObject;
				friend.transform.localScale = new Vector3(1f,1f);
				friend.transform.SetParent(toInstantiate.transform);
			}

			toInstantiate.transform.SetParent(showroomParent.transform);

			if(toInstantiate != null)
				allShowrooms.Add(toInstantiate);
		}

	}


}
