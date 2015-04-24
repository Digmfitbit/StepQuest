using UnityEngine;
using System.Collections;

public class Enemy : Fighter 
{

	private void Start()
	{
		health = Random.Range (80,120);

	}

}
