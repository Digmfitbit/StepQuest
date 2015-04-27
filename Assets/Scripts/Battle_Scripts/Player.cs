using UnityEngine;
using System.Collections;

public class Player : Fighter 
{

	private void Start()
	{
		health = 100f;
		damage = 15f;

		probabilityOfMissing = Random.Range (5f, 30f);
	}




}
