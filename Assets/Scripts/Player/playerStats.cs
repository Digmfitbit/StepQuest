using System;
using Assets.Scripts;


//[Serializable]
public class playerStats : JSONable {
    
	//Overall level.
	public static int playerLvl;
	//How much to level.
    public static int expToNext;
	//How much do they currently have.
    public static int currentExp;

	//How much damage.
    public static int playerStrength = 5;

	//How much hits can combo.
    public static int playerStamina = 5;

	//How much health.
    public static int playerEndurance = 5;

	//How fast you recover.
    public static int playerRecovery = 5;

    public playerStats()
    {
        playerLvl = 1;
        expToNext = 100;
        currentExp = 0;
        playerStrength = 5;
        playerStamina = 5;
        playerEndurance = 5;
        playerRecovery = 5;
    }

    /**
     * TODO make this load the proper stat values
     * from local cache and/or network
     * */
	void Load () {
        //TODO load this from playerPrefs/database
        
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
