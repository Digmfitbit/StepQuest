using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StatusBarScript : MonoBehaviour {

	public GameObject[] HealthBarSegments;
	private Renderer[] HealthBarSegmentsRenderer;

	public float healthLevel;
	public int segmentsToDisplay = 3;

	// Use this for initialization
	void Awake () 
	{
		HealthBarSegmentsRenderer = new Renderer[HealthBarSegments.Length];

		for (int i = 0; i < HealthBarSegments.Length; i++) 
		{
			HealthBarSegmentsRenderer[i] = HealthBarSegments[i].GetComponent<Renderer>();
		}

		segmentsToDisplay = HealthBarSegments.Length;
	}
	
	// Update is called once per frame
	void Update () 
	{
//		displaySegments ();
	}

	public void UpdateStatusBar(Vector2 healthValues)
	{
        float curHealth = healthValues.x;
        float maxHealth = healthValues.y;

		if (curHealth <= 0) {
			segmentsToDisplay = 0;
		} 
		else 
		{
			segmentsToDisplay = Mathf.RoundToInt(map(0f,maxHealth,0f,6f,curHealth));

			if(segmentsToDisplay == 0 && !(curHealth <= 0))
				segmentsToDisplay = 1;
		}



		for (int i = 0; i < HealthBarSegments.Length; i++) 
		{
			HealthBarSegmentsRenderer[i].material.color = new Color(1f,1f,1f,0f);
		}

		if (segmentsToDisplay > HealthBarSegments.Length)
			segmentsToDisplay = HealthBarSegments.Length;
		else if (segmentsToDisplay < 0)
			segmentsToDisplay = 0;

		for (int i = 0; i < segmentsToDisplay; i++) 
		{
			HealthBarSegmentsRenderer[i].material.color = new Color(1f,1f,1f,1f);
		}
	}

	public float map(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue){
		
		float OldRange = (OldMax - OldMin);
		float NewRange = (NewMax - NewMin);
		float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;
		
		return(NewValue);
	}

}
