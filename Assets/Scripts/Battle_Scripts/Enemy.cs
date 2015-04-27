using UnityEngine;
using System.Collections;

public class Enemy : Fighter 
{

	private void Start()
	{
		health = 100f;
		damage = 5f;
		probabilityOfMissing = Random.Range (30f,40f);

	}

}
