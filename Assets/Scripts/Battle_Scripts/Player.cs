using UnityEngine;
using System.Collections;

public class Player : Fighter 
{

	private void Start()
	{
		health = 20 * playerStats.playerEndurance;
        damage = 4 * playerStats.playerStrength;

		probabilityOfMissing = Random.Range (5f, 30f);
	}




}
