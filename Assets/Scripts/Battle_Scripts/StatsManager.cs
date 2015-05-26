using UnityEngine;
using System.Collections;

public class StatsManager : MonoBehaviour {

	private int expToWin;
	public GameObject ui;
	public GameObject gui_wonExpPoints;
	public GameObject gui_playerExpPoints;

	void Start(){
		//Hide UI panel
		ui.SetActive (false);
		CalculateExperiences ();
	}

	 


    // TODO: read playerStats from PlayerManager
    PlayerStats playerStats = new PlayerStats("");

	public void CalculateExperiences()
	{
		//placeholder until we find something better.
		expToWin = playerStats.playerLvl * 2;
	}



	public void AddPlayerExperiences()
	{
		ui.SetActive(true);
		StartCoroutine (UIAddExperiencPoints());
		//Add exp to player stats
		playerStats.currentExp += expToWin;
	}

	public IEnumerator UIAddExperiencPoints()
	{

		yield return new WaitForSeconds(1);
	}
}
