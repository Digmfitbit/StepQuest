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


	override public void attack()
	{
		//Attacking Animation - Move the fighter to the opponent. If the fighter is close enough he hits him by calling the hit function on the opponent. 
		if (isAttacking) 
		{
			newPos = Vector3.Slerp (transform.position, enemy.transform.position, Time.deltaTime * attackSpeed);
			transform.position = newPos;
			
			if(Vector3.Distance(transform.position,enemy.transform.position) < 0.2)
			{

				isAttacking = false;
				
				//how high is the propability of a failed attack, call hit function on selected opponent
				if (probabilityOfMissing < Random.Range (0, 100)) 
				{								
					enemy.SendMessage ("Hit", damage);
				} 
				else 
				{
					Debug.Log ("Shit I missed!!!");
				}
			}
		}
	}


}
