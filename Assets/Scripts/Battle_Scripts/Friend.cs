using UnityEngine;
using System.Collections;

public class Friend : Fighter 
{

	private void Start()
	{
        health = 20 * PlayerManager.mainPlayer.playerEndurance;
        damage = 4 * PlayerManager.mainPlayer.playerStrength;
		
		probabilityOfMissing = Random.Range (5f, 30f);
	}

}
