using UnityEngine;
using System.Collections;

public class Friend : Fighter 
{

	private void Start()
	{
        PlayerManager manager = FindObjectOfType<PlayerManager>();
		health = 20 * manager.mainPlayer.playerEndurance;
        damage = 4 * manager.mainPlayer.playerStrength;
		
		probabilityOfMissing = Random.Range (5f, 30f);
	}

}
