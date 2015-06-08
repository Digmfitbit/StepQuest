using UnityEngine;
using System.Collections;

public class PlayerAppearance : MonoBehaviour {
	
	public Sprite[] sprites;

	public string playerIcon;

	void Awake () {
		Debug.Log (PlayerManager.mainPlayer.playerClassID);

		playerIcon = PlayerManager.mainPlayer.playerClassID;

		switch(playerIcon){
		case "character_01":
			GetComponent<SpriteRenderer>().sprite = sprites[0];
			break;
		case "character_02":
			GetComponent<SpriteRenderer>().sprite = sprites[1];
			break;
		case "character_03":
			GetComponent<SpriteRenderer>().sprite = sprites[2];
			break;
		case "character_04":
			GetComponent<SpriteRenderer>().sprite = sprites[3];
			break;
		default:
			break;
		}
	}
}
