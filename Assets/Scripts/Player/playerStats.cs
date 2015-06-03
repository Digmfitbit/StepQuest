using System;
using Assets.Scripts;
using UnityEngine;
using ResponseObjects;

//[Serializable]
public class PlayerStats : JSONable {

    public string id;

	public string playerName;

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

    public string fitbitPictureUrl;
	// >> **Items** Visible objects in the showroom

    /**
     * The constructor to use most times when constructing from
     * Either the local cache or the network call
     * */
    public PlayerStats(JSONObject jsonObject)
    {
        
        this.fromJSON(jsonObject);
    }

    /**
     * Call only when a player first starts the game.
     * 
     * */
    public PlayerStats(FriendModel playerModel)
    {
        this.id = playerModel.encodedId;
        this.fitbitPictureUrl = playerModel.avatar;
		playerName = playerModel.fullName;

        //initialize game only features
        playerLvl = 1;
        expToNext = 100;
        currentExp = 0;
        playerStrength = 5;
        playerStamina = 5;
        playerEndurance = 5;
        playerRecovery = 5;

        //player looks
        playerClassID = "character_01";
        playerColor = "red";

        //showroom
        showroomBG = 1;
    }

    public override JSONObject getJSON()
    {
        JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
        json.AddField("id", id);
		json.AddField("playerName", playerName);
        json.AddField("playerLevel", playerLvl);
        json.AddField("playerStrength", playerStrength);
        json.AddField("playerStamina", playerStamina);
        json.AddField("playerEndurance", playerEndurance);
        json.AddField("playerRecovery", playerRecovery);

		//player looks
		json.AddField ("playerClassID", playerClassID);
		json.AddField ("playerColor", playerColor);

		//showroom
		json.AddField("showroomBG", showroomBG);
        json.AddField("fitbitPictureUrl", fitbitPictureUrl);
        return json;
    }

    public override void fromJSON(JSONObject json)
    {
        
        json.GetField("id", delegate(JSONObject str)
        {
            id = str.ToString().Substring(1,str.ToString().Length-2);
        });
        json.GetField("stats", delegate(JSONObject stats)
        {
            string strTemp = WWW.UnEscapeURL(stats.ToString());
            stats = new JSONObject(strTemp.Substring(1,strTemp.Length-2));
            stats.GetField("playerName", delegate(JSONObject str)
            {
                playerName = str.ToString().Substring(1, str.ToString().Length - 2);
            });
            stats.GetField("playerLevel", delegate(JSONObject numb)
            {
                playerLvl = Convert.ToInt32(numb.ToString());
            });
            stats.GetField("playerStrength", delegate(JSONObject numb)
            {
                playerStrength = Convert.ToInt32(numb.ToString());
            });
            stats.GetField("playerStamina", delegate(JSONObject numb)
            {
                playerStamina = Convert.ToInt32(numb.ToString());
            });
            stats.GetField("playerEndurance", delegate(JSONObject numb)
            {
                playerEndurance = Convert.ToInt32(numb.ToString());
            });
            //player looks
            stats.GetField("playerClassID", delegate(JSONObject str)
            {
                playerClassID = str.ToString().Substring(1, str.ToString().Length - 2);
            });
            stats.GetField("playerColor", delegate(JSONObject str)
            {
                playerColor = str.ToString().Substring(1, str.ToString().Length - 2); ;
            });

            //showroom
            stats.GetField("showroomBG", delegate(JSONObject numb)
            {
                showroomBG = Convert.ToInt32(numb.ToString());
            });
            stats.GetField("fitbitPictureUrl", delegate(JSONObject str)
            {
                fitbitPictureUrl = str.ToString().Substring(1, str.ToString().Length - 2);
            });
        });
    }

    public override string ToString()
    {
        return ((JSONable)this).getJSON().ToString();
    }

}
