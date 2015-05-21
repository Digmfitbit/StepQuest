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
	}

    JSONObject JSONable.getJSON()
    {
        JSONObject json = new JSONObject(JSONObject.Type.OBJECT);

        json.AddField("playerLevel", playerLvl);
        json.AddField("playerStrength", playerStrength);
        json.AddField("playerStamina", playerStamina);
        json.AddField("playerEndurance", playerEndurance);
        json.AddField("playerRecovery", playerRecovery);

        return json;
    }

    void JSONable.fromJSON(JSONObject json)
    {
        playerLvl = Convert.ToInt32(json.GetField("playerLevel").ToString());
        playerStrength = Convert.ToInt32(json.GetField("playerStrength").ToString());
        playerStamina = Convert.ToInt32(json.GetField("playerStamina").ToString());
        playerEndurance = Convert.ToInt32(json.GetField("playerEndurance").ToString());
        playerRecovery = Convert.ToInt32(json.GetField("playerRecovery").ToString());
    }
}
