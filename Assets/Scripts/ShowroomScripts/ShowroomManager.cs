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

	public int numberOfFriends = 5;

	// Use this for initialization
	void Awake () 
	{
		//min number of showrooms is two (Main showroom and player showroom)
		numberOfShowrooms = 2 + numberOfFriends;
		//Get all showroom objects and user/friend data


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
				toInstantiate = Instantiate (playerShowroom, new Vector2 (spaceBetweenShowrooms, 0f), Quaternion.identity) as GameObject;
				GameObject player = Instantiate(Resources.Load("Prefabs/BattlePrefabs/PlayerShowroom",typeof(GameObject)),new Vector3 (spaceBetweenShowrooms, 0f, -1f) , Quaternion.identity) as GameObject;
				player.transform.localScale = new Vector3(5f,5f);
				player.transform.SetParent(toInstantiate.transform);
			}
			else if(i > 1)
			{
				toInstantiate = Instantiate(friendShowroom, new Vector2(i * spaceBetweenShowrooms, 0f), Quaternion.identity) as GameObject;
				GameObject friend = Instantiate(Resources.Load("Prefabs/BattlePrefabs/PlayerShowroom",typeof(GameObject)),new Vector3 (i * spaceBetweenShowrooms, 0f, -1f) , Quaternion.identity) as GameObject;
				friend.transform.localScale = new Vector3(5f,5f);
				friend.transform.SetParent(toInstantiate.transform);
			}

			toInstantiate.transform.SetParent(showroomParent.transform);

			if(toInstantiate != null)
				allShowrooms.Add(toInstantiate);
		}

	}
}
