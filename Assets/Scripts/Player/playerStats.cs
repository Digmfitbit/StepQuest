using System;
using Assets.Scripts;


//[Serializable]
public class PlayerStats : JSONable {

    public string id;

	//Overall level.
	public int playerLvl;
	//How much to level.
    public int expToNext;
	//How much do they currently have.
    public int currentExp;

	//How much damage.
    public int playerStrength = 5;

	//How much hits can combo.
    public int playerStamina = 5;

	//How much health.
    public int playerEndurance = 5;

	//How fast you recover.
    public int playerRecovery = 5;

	//**Player Looks**//
	//What sprite do I have to load.
	public string playerClassID;
	//User picked color for the character.
	public string playerColor;
	// >> **Armor** What visible items does the player have?

	//**Showroom**, how does the showroom look.
	//BG sprite for showroom
	public int showroomBG;
	// >> **Items** Visible objects in the showroom




    public PlayerStats(string id)
    {
        this.id = id;
        Load();
    }

    /**
     * TODO make this load the proper stat values
     * from local cache and/or network on Awake()
     * */
	void Load () {
        //TODO load this from playerPrefs/database
        playerLvl = 1;
        expToNext = 100;
        currentExp = 0;
        playerStrength = 5;
        playerStamina = 5;
        playerEndurance = 5;
        playerRecovery = 5;

		//player looks
		playerClassID = "class_01";
		playerColor = "red";

		//showroom
		showroomBG = 1;
	}

    JSONObject JSONable.getJSON()
    {
        JSONObject json = new JSONObject(JSONObject.Type.OBJECT);

        json.AddField("playerLevel", playerLvl);
        json.AddField("playerStrength", playerStrength);
        json.AddField("playerStamina", playerStamina);
        json.AddField("playerEndurance", playerEndurance);
        json.AddField("playerRecovery", playerRecovery);

		//player looks
		json.AddField ("playerClassID", playerClassID);
		json.AddField ("playerColor", playerColor);

		//showroom
		json.AddField ("showroomBG", showroomBG);

        return json;
    }

    void JSONable.fromJSON(JSONObject json)
    {
        playerLvl = Convert.ToInt32(json.GetField("playerLevel").ToString());
        playerStrength = Convert.ToInt32(json.GetField("playerStrength").ToString());
        playerStamina = Convert.ToInt32(json.GetField("playerStamina").ToString());
        playerEndurance = Convert.ToInt32(json.GetField("playerEndurance").ToString());
        playerRecovery = Convert.ToInt32(json.GetField("playerRecovery").ToString());

		//player looks
		playerClassID = Convert.ToString (json.GetField("playerClassID").ToString());
		playerColor = Convert.ToString (json.GetField("playerColor").ToString());

		//showroom
		showroomBG = Convert.ToString (json.GetField("showroomBG").ToString());
    }
}
