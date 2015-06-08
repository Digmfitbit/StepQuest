using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatsManager : MonoBehaviour {

	private int expToWin;
	public GameObject ui;
	public GameObject gui_wonExpPoints;
	public GameObject gui_playerExpPoints;

    private GameObject endGamePanel;

	void Start(){
		//Hide UI panel
        endGamePanel = GameObject.Find("Panel_BattleOver");
		endGamePanel.SetActive (false);

		CalculateExperiences ();
	}

	public void CalculateExperiences()
	{
		//placeholder until we find something better.
		expToWin = PlayerManager.mainPlayer.playerLvl * 2;
	}



    public void AddPlayerExperiences()
    {
        //Add exp to player stats
        PlayerManager.mainPlayer.currentExp += expToWin;
        // show end game panel
        endGamePanel.SetActive(true);
        endGamePanel.transform.Find("Text_WonExp/Text_WonExpNr").GetComponent<Text>().text = expToWin.ToString();
        endGamePanel.transform.Find("Text_PlayerExp/Text_PlayerExpNr").GetComponent<Text>().text = PlayerManager.mainPlayer.currentExp.ToString();
    }
}
